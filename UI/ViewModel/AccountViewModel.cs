using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Security.Credentials;
using Windows.System;
using Windows.UI.Popups;
using DAL.Common;
using Microsoft.Practices.Prism.Commands;
using Prism.Windows.Validation;
using UI.Logic;
using UI.MVVM;
using UI.ViewModel.Common;
using User = DAL.Model.User;

namespace UI.ViewModel
{
    class AccountViewModel : BaseViewModel
    {
        public AccountViewModel()
        {
            using (var uow = new UnitOfWork())
            {
                var users = uow.UserRepository.GetByQuery(orderBy: o => o.OrderBy(u => u.Name));
                Users = new ObservableCollection<User>(users);
            }

            AddUserCommand = new DelegateCommand(OnAddUserCommand);


        }

        private ObservableCollection<User> _users;

        public ObservableCollection<User> Users
        {
            get { return _users; }
            set { SetProperty(ref _users, value); }
        }

        private string _login;

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(16, MinimumLength = 4, ErrorMessage = "Длинна от 4 до 16 символов")]
        public string Login
        {
            get { return _login; }
            set { SetProperty(ref _login, value); }
        }


        private string _password;

        [MinLengthIf("IsPasswordSet", true, 6, "Длинна пароля должна быть не менее 6 символов")]
        [StringLength(16, ErrorMessage = "Длинна до 16 символов")]
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

        public ICommand AddUserCommand { get; }

        private void OnAddUserCommand()
        {
            SaltedHash saltedHash = new SaltedHash("123456");
            var hash = saltedHash.Hash;
            var salt = saltedHash.Salt;

            var result = SaltedHash.Verify(salt, hash, "123456");


            //var dialog = new MessageDialog(Login, Password);
            //dialog.ShowAsync();
            if (!ValidateProperties()) return;


            //var user = new User()
            //{
            //    Name = Login,

            //};
            //using (var uow = new UnitOfWork())
            //{
            //    uow.UserRepository.Insert(user);
            //    uow.Commit();
            //}
        }
    }
}
