using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DAL.Common;
using DAL.Model;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
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
                // var user1 = uow.UserRepository.GetByQuery(u => u.Id == Logic.UserContext.Current.UserId, null, x => x.Assets);

                User = uow.UserRepository.GetById(Logic.UserContext.Current.UserId);
                var assetHepler = new AssetHelper(User.Id);

                var assets = assetHepler.GetAssets();

                Assets = new ObservableCollection<AssetViewModel>(assets);
            }

            AddTransactionCommand = new Microsoft.Practices.Prism.Commands.DelegateCommand<AssetViewModel>(OnAddTransactionClick);
            AddAssetCommand = new DelegateCommand(OnAddAsset);
            NavigateToTransactionList = new DelegateCommand<AssetViewModel>(OnNavigateToTransactionList);
            NavigateToDeleteTransaction = new Microsoft.Practices.Prism.Commands.DelegateCommand<AssetViewModel>(OnNavigateToDeleteTransaction);
            FilterChangeCommand = new DelegateCommand<object>(OnFilterChange);
        }


        //private async void Test()
        //{
        //    var savePicker = new Windows.Storage.Pickers.FileSavePicker();
        //    savePicker.SuggestedStartLocation =
        //        Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
        //    // Dropdown of file types the user can save the file as
        //    savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
        //    // Default file name if the user does not type one in or select a file to replace
        //    savePicker.SuggestedFileName = "New Document";
        //    Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
        //}

        public ICommand AddTransactionCommand { get; }

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

        [MaxLength(20, ErrorMessage = "Максимальная длина не превышает 20 символов")]
        [Required(ErrorMessage = "Поле не заполнено")]
        [MinLength(4, ErrorMessage = "Минимум 4 символа")]
        [UniqueAssetAttibute(ErrorMessage = "Такой кошелек уже создан")]
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


        private void OnAddTransactionClick(AssetViewModel asset)
        {

            var rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(AddTransaction), asset);
        }

        private string test;

        public string Test
        {
            get { return test; }
            set { SetProperty(ref test, value); }
        }

        public ICommand FilterChangeCommand { get; }

        private void OnFilterChange(object o)
        {
            var x = 1;
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
