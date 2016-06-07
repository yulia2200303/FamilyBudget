using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DAL.Common;
using DAL.Model;
using Prism.Commands;
using Shared.Enum;
using UI.Logic;
using UI.MVVM.Validation;
using UI.Pages;
using UI.ViewModel.Common;

namespace UI.ViewModel
{
    public class MyAssetsViewModel : BaseViewModel
    {
        public MyAssetsViewModel()
        {
            using (var uow = new UnitOfWork())
            {
                User = uow.UserRepository.GetById(Logic.UserContext.Current.UserId);

                var assetHepler = new AssetHelper(User.Id);
                var assets = assetHepler.GetAssets();
                Assets = new ObservableCollection<AssetViewModel>(assets);
            }

            AddTransactionCommand = new Microsoft.Practices.Prism.Commands.DelegateCommand<AssetViewModel>(OnAddTransactionClick);
            AddAssetCommand = new DelegateCommand(OnAddAsset);
            NavigateToTransactionList = new DelegateCommand<AssetViewModel>(OnNavigateToTransactionList);
            NavigateToDeleteTransaction = new Microsoft.Practices.Prism.Commands.DelegateCommand<AssetViewModel>(OnNavigateToDeleteTransaction);
        }

        private User _user;

        public User User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        private ObservableCollection<AssetViewModel> _assets;

        public ObservableCollection<AssetViewModel> Assets
        {
            get { return _assets; }
            set { SetProperty(ref _assets, value); }
        }

        private string _assetName;

        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RequiredErrorMessage")]
        [StringLength(16, MinimumLength = 4, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "LengthRange")]
        [UniqueAssetAttibute(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "NameAlreadyExists")]
        public string AssetName
        {
            get { return _assetName; }
            set { SetProperty(ref _assetName, value); }
        }

        private bool _isFlyoutOpen;
        public bool IsFlyoutOpen
        {
            get { return _isFlyoutOpen; }
            set { SetProperty(ref _isFlyoutOpen, value); }
        }

        public ICommand AddTransactionCommand { get; }

        private void OnAddTransactionClick(AssetViewModel asset)
        {
            var rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(AddTransaction), asset);
        }

        public ICommand AddAssetCommand { get; }

        private void OnAddAsset()
        {
            if (!ValidateProperty("AssetName")) return;

            IsFlyoutOpen = false;

            var asset = new Asset()
            {
                UserId = User.Id,
                Name = AssetName,
                Type = (int)AssetType.Default,
            };

            using (var uow = new UnitOfWork())
            {
                uow.AssetRepository.Insert(asset);
                uow.Commit();
            }

            Assets.Add(new AssetViewModel()
            {
                Id = asset.Id,
                Cost = 0,
                Name = asset.Name,
                UserId = asset.UserId
            });

            AssetName = string.Empty;
            Errors.SetAllErrors(new Dictionary<string, ReadOnlyCollection<string>>());

        }

        public ICommand NavigateToTransactionList { get; }

        private void OnNavigateToTransactionList(AssetViewModel assetModel)
        {
            var root = Window.Current.Content as Frame;
            root.Navigate(typeof(ListOfTransactions), assetModel.Id);
        }

        public ICommand NavigateToDeleteTransaction { get; }

        private void OnNavigateToDeleteTransaction(AssetViewModel assetModel)
        {
            var root = Window.Current.Content as Frame;
            root.Navigate(typeof(DeleteTransactions), assetModel.Id);
        }
    }
}
