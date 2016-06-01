using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using DAL.Common;
using DAL.Model;
using Newtonsoft.Json;
using Prism.Commands;
using UI.Logic;
using UI.Model;
using UI.Pages;
using UI.ViewModel.Common;

namespace UI.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel()
        {
            var supportedLanguages = LocalizationHelper.GetSupportedLocales()
                .Select(l => new ComboboxItem()
                {
                    Text = l.Value,
                    Value = l.Key
                }).ToList();

            Languages = new ObservableCollection<ComboboxItem>(supportedLanguages);
            SelectedLanguage = supportedLanguages.First(l => l.Value.ToString() == LocalizationHelper.GetCurrentLocale());
            ChangeLocaleCommand = new DelegateCommand(OnChangeLocale);
            ExportCommand = new DelegateCommand(OnExportCommand);
            ImportCommand = new DelegateCommand(OnImportCommand);
        }


        private ObservableCollection<ComboboxItem> _languges;

        public ObservableCollection<ComboboxItem> Languages
        {
            get { return _languges; }
            set { SetProperty(ref _languges, value); }
        }

        private ComboboxItem _selectedLanguage;

        public ComboboxItem SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                SetProperty(ref _selectedLanguage, value);
            }
        }

        public ICommand ChangeLocaleCommand { get; }

        private async void OnChangeLocale()
        {
            if (SelectedLanguage == null) return;
            LocalizationHelper.SetLocale(SelectedLanguage.Value.ToString());
            Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
            Windows.ApplicationModel.Resources.Core.ResourceContext.ResetGlobalQualifierValues();
            Windows.ApplicationModel.Resources.Core.ResourceContext.SetGlobalQualifierValue("language", SelectedLanguage.Value.ToString());

            await Task.Delay(500);
            var rootFrame = Window.Current.Content as Frame;
            rootFrame?.Navigate(rootFrame.Content.GetType());
        }

        public ICommand ExportCommand { get; }

        private async void OnExportCommand()
        {
            var userId = UserContext.Current.UserId;
            var model = new ImportModel();
            using (var uow = new UnitOfWork())
            {
                var categoryList = new List<ImportCategoryModel>();
                var categories = uow.CategoryRepository.GetCategories();
                foreach (var category in categories)
                {
                    categoryList.Add(new ImportCategoryModel()
                    {
                        Name = category.Name,
                        Subcategories = category.SubCategories.Select(c => c.Name).ToList()
                    });
                }

                model.Categories = categoryList;

                var assets = uow.AssetRepository.GetByUserId(userId);

                model.Assets = assets.Select(a => a.Name).ToList();
                var transactions = new List<ImportTransactionModel>();
                foreach (var asset in assets)
                {
                    var transactionList = uow.TransactionRepository.GetByAssetId(asset.Id);
                    foreach (var transaction in transactionList)
                    {
                        transactions.Add(new ImportTransactionModel()
                        {
                            Asset = asset.Name,
                            Subcategory = transaction.Product.Name,
                            Category = transaction.Product.Parent.Name,
                            Comment = transaction.Comment,
                            Cost = transaction.Cost,
                            Curency = transaction.Currency.Code,
                            Date = transaction.Date,
                            Type = transaction.Type
                        });
                    }
                }

                model.Transactions = transactions;
            }

            var sm = JsonConvert.SerializeObject(model);

            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".json" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "New Document";
            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();

            await Windows.Storage.FileIO.WriteTextAsync(file, sm);

        }

        public ICommand ImportCommand { get; }

        private async void OnImportCommand()
        {
            var dialog =new MessageDialog("Восстановить данные? Всяк текущая информация будет потеряна");
            dialog.Commands.Add(new UICommand("Yes", null, 1));
            dialog.Commands.Add(new UICommand("No", null, 0));
            var result = await dialog.ShowAsync();
            if((int)result.Id == 0) return;

            var userId = UserContext.Current.UserId;

            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".json");


            StorageFile file = await openPicker.PickSingleFileAsync();


            if (file != null)
            {
                var sm = await Windows.Storage.FileIO.ReadTextAsync(file);

                var model = JsonConvert.DeserializeObject<ImportModel>(sm);

                using (var uow = new UnitOfWork())
                {
                    uow.AssetRepository.RemoveByUserId(userId);
                    uow.Commit();

                    uow.AssetRepository.Insert(userId, model.Assets);
                    uow.Commit();

                    uow.CategoryRepository.InsertCategories(model.Categories.Select(s => s.Name));
                    uow.Commit();

                    foreach (var importCategory in model.Categories)
                    {
                        uow.CategoryRepository.InsertSubcategories(importCategory.Name, importCategory.Subcategories);
                    }
                    uow.Commit();

                    var assets = uow.AssetRepository.GetByUserId(userId).ToDictionary(c=> c.Name, c => c.Id);
                    var categories = uow.CategoryRepository.GetByQuery(c => c.Parent == null)
                        .ToDictionary(c => c.Name, c => c.SubCategories.ToDictionary(s => s.Name, s => s.Id));
                    var currencies = uow.CurrencyRepository.GetAll().ToDictionary(c => c.Code, c => c.Id);


                    foreach (var importTransactionModel in model.Transactions)
                    {
                        var transaction = new Transaction
                        {
                            Comment = importTransactionModel.Comment,
                            Cost = importTransactionModel.Cost,
                            Date = importTransactionModel.Date,
                            Type = importTransactionModel.Type,
                            AssetId = assets[importTransactionModel.Asset],
                            CurrencyId = currencies[importTransactionModel.Curency],
                            ProductId = categories[importTransactionModel.Category][importTransactionModel.Subcategory]
                        };
                        uow.TransactionRepository.Insert(transaction);
                    }

                    uow.Commit();
                }
            }
        }
    }

    public class ComboboxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
