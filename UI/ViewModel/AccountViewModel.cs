using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DAL.Common;
using DAL.Model;
using Microsoft.Practices.Prism.Commands;
using UI.Logic;
using UI.Model;
using UI.MVVM;
using UI.MVVM.Validation;
using UI.Pages;
using UI.ViewModel.Common;
using User = DAL.Model.User;

namespace UI.ViewModel
{
    class AccountViewModel : BaseViewModel
    {
        private const string DefaultAssetName = "My Asset";

        public AccountViewModel()
        {
            using (var uow = new UnitOfWork())
            {
                var users = uow.UserRepository.GetByQuery(orderBy: o => o.OrderBy(u => u.Name)).Select(UserModel.Convert).ToList();
                Users = new ObservableCollection<UserModel>(users);
            }

            AddUserCommand = new DelegateCommand(OnAddUserCommand);
            LoginCommand = new DelegateCommand<UserModel>(OnLoginCommand);
            RemoveCommand = new DelegateCommand<UserModel>(OnRemoveCommand);
            EnteredPasswordCommand = new DelegateCommand<string>(OnEnteredPasswordChange);

            _timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 1, 500) };
            _timer.Tick += TimerOnTick;
        }

        private void TimerOnTick(object sender, object o)
        {
            _timer.Stop();

            if (SelectedUser == null) return;

            if (SaltedHash.Verify(SelectedUser.Salt, SelectedUser.Hash, SelectedUser.EnteredPassword))
            {
                UserContext.Current.Authenticate(SelectedUser.Name, SelectedUser.EnteredPassword, SelectedUser.Id, null);
                var rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(MyAssets));
            }
        }

        private ObservableCollection<UserModel> _users;

        public ObservableCollection<UserModel> Users
        {
            get { return _users; }
            set { SetProperty(ref _users, value); }
        }

        private string _login;


        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RequiredErrorMessage")]
        [StringLength(16, MinimumLength = 4, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "LengthRange")]
        [UniqueUserName(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "NameAlreadyExists")]
        public string Login
        {
            get { return _login; }
            set { SetProperty(ref _login, value); }
        }

        private string _password;

        [MinLengthIf("IsPasswordSet", true, 4, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "LengthRange")]
        [StringLength(16, MinimumLength = 4, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "LengthRange")]
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private bool _isPasswordSet;

        public bool IsPasswordSet
        {
            get { return _isPasswordSet; }
            set { SetProperty(ref _isPasswordSet, value); }
        }

        private UserModel _userModel;

        public UserModel SelectedUser
        {
            get { return _userModel; }
            set { this.SetProperty(ref _userModel, value); }
        }

        public ICommand AddUserCommand { get; }

        private void OnAddUserCommand()
        {
            if (!ValidateProperties()) return;

            var user = new User
            {
                Name = Login,
            };

            if (IsPasswordSet)
            {
                SaltedHash saltedHash = new SaltedHash(Password);
                user.IsPasswordSet = true;
                user.Hash = saltedHash.Hash;
                user.Salt = saltedHash.Salt;
            }

            var asset = new Asset
            {
                Name = DefaultAssetName,
                User = user
            };

            using (var uow = new UnitOfWork())
            {
                uow.UserRepository.Insert(user);
                uow.AssetRepository.Insert(asset);

                uow.Commit();
            }

            Users.Add(UserModel.Convert(user));
            ClearCredentials();
        }

        public ICommand RemoveCommand { get; }

        public async void OnRemoveCommand(UserModel userModel)
        {
            if (userModel != null)
                SelectedUser = userModel;
            
            var dialog = new MessageDialog(string.Format(ResourceLoader.GetForCurrentView().GetString("WantToDeleteProfile"), SelectedUser.Name));
            dialog.Commands.Add(new UICommand(ResourceLoader.GetForCurrentView().GetString("Yes"), null, 1));
            dialog.Commands.Add(new UICommand(ResourceLoader.GetForCurrentView().GetString("No"), null, 0));
            var result = await dialog.ShowAsync();
            if ((int)result.Id == 0) return;

            using (var uow = new UnitOfWork())
            {
                uow.UserRepository.DeleteById(SelectedUser.Id);
                uow.Commit();
            }

            Users.Remove(SelectedUser);
            SelectedUser = null;
        }

        public ICommand LoginCommand { get; }

        private void OnLoginCommand(UserModel selecredUser)
        {
            if (selecredUser == null) return;

            SelectedUser = selecredUser;
            if (!SelectedUser.IsPasswordSet)
            {
                UserContext.Current.Authenticate(SelectedUser.Name, "", SelectedUser.Id, null);
                var rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(MyAssets));
                return;
            }

            selecredUser.IsPanelShow = true;
        }


        readonly DispatcherTimer _timer;
        public ICommand EnteredPasswordCommand { get; }

        private void OnEnteredPasswordChange(string password)
        {
            if (_timer.IsEnabled)
            {
                _timer.Stop();
            }

            _timer.Start();
        }

        private void ClearCredentials()
        {
            Login = string.Empty;
            Password = string.Empty;
            IsPasswordSet = false;
            SelectedUser = null;
            Errors.SetAllErrors(new Dictionary<string, ReadOnlyCollection<string>>());
        }
    }
}
