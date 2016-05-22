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
using UI.MVVM.Validation;
using UI.Pages;
using UI.ViewModel.Common;

namespace UI.ViewModel
{
    public class MyAssetsViewModel : BaseViewModel
    {
        public MyAssetsViewModel()
        {
            var userName = Logic.UserContext.Current.UserName;

            using (var uow = new UnitOfWork())
            {
                User = uow.UserRepository.GetById(Logic.UserContext.Current.UserId);
                var assets = uow.AssetRepository.GetAll().Where(a => a.Id == User.Id).ToList();
              
                Assets = new ObservableCollection<Asset>(assets);

                var transaction = uow.TransactionRepository.GetAll().ToList();
            }

            AddTransactionCommand = new Microsoft.Practices.Prism.Commands.DelegateCommand<Asset> (OnAddTransactionClick);
            AddAssetCommand = new DelegateCommand(OnAddAsset);
        }

        public ICommand AddTransactionCommand { get; }

        private User _user;

        public User User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        private ObservableCollection<Asset> _assets;

        public ObservableCollection<Asset> Assets
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


        private void OnAddTransactionClick(Asset asset)
        {
            
            var rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(AddTransaction), asset);
        }


        public ICommand AddAssetCommand { get; }

        private void OnAddAsset()
        {
            if (ValidateProperty("AssetName"))
            {
                IsFlyoutOpen = false;
            }
        }

    }
}
