using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Popups;
using DAL.Common;
using DAL.Model;
using Prism.Commands;
using Shared.Enum;
using UI.ViewModel.Common;
using UserContext = UI.Logic.UserContext;

namespace UI.ViewModel
{
    public class AddTransactionViewModel : BaseViewModel
    {
        public AddTransactionViewModel()
        {
            Operations = new ObservableCollection<Operation>(new List<Operation>()
            {
                new Operation {Name = "Debit", Value = (int) OperationType.Debit},
                new Operation {Name = "Credit", Value = (int) OperationType.Credit}
            });

            using (var uow = new UnitOfWork())
            {
                var assets = uow.AssetRepository.GetByQuery(a => a.UserId == UserContext.Current.UserId);
                Assets = new ObservableCollection<Asset>(assets);

                var curresncies = uow.CurrencyRepository.GetAll().OrderBy(c => c.Code);
                Currencies = new ObservableCollection<Currency>(curresncies);
                SelectedCurrnecy = Currencies[0];

                var categories = uow.CategoryRepository.GetCategories();
                Categories = new ObservableCollection<Category>(categories);
            }

            AddCommand = new DelegateCommand(OnAddCommand);
            DateChangedCommand = new DelegateCommand<object>(OnDateChangedCommand);
        }

        public AddTransactionViewModel(AssetViewModel asset) : this()
        {
            SelectedAsset = Assets.First(a => a.Id == asset.Id);
        }

        private int _type;
        [Range(1, 2, ErrorMessage = "Выберете тип операции")]
        public int Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        private Asset _selectedAsset;

        public Asset SelectedAsset
        {
            get { return _selectedAsset; }
            set { SetProperty(ref _selectedAsset, value); }
        }

        ObservableCollection<Operation> _operations;

        public ObservableCollection<Operation> Operations
        {
            get { return _operations; }
            set { SetProperty(ref _operations, value); }
        }

        private ObservableCollection<Category> _categories;

        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }

        private Category _selectedCategory;

        [Required(ErrorMessage = "Выберете категорию")]
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                SetProperty(ref _selectedCategory, value);
                if (value == null)
                {
                    SubCategories = new ObservableCollection<Category>();
                }
                else
                {
                    OnCategoryChange(value.Name);
                }
               
            }
        }

        private Category _selectedSubCategory;

        [Required(ErrorMessage = "Выберете под категорию")]
        public Category SelectedSubCategory
        {
            get { return _selectedSubCategory; }
            set { SetProperty(ref _selectedSubCategory, value); }
        }

        private ObservableCollection<Category> _subCategories;

        public ObservableCollection<Category> SubCategories
        {
            get { return _subCategories; }
            set { SetProperty(ref _subCategories, value); }
        }

        private void OnCategoryChange(string category)
        {
            using (var uow = new UnitOfWork())
            {
                var subCategories = uow.CategoryRepository.GetSubCategories(category);
                SubCategories = new ObservableCollection<Category>(subCategories);
            }
        }

        private ObservableCollection<Asset> _assets;

        public ObservableCollection<Asset> Assets
        {
            get { return _assets; }
            set { SetProperty(ref _assets, value); }
        }

        private ObservableCollection<Currency> _currencies;

        public ObservableCollection<Currency> Currencies
        {
            get { return _currencies; }
            set { SetProperty(ref _currencies, value); }
        }

        private Currency _selectedCurrency;

        public Currency SelectedCurrnecy
        {
            get { return _selectedCurrency; }
            set { SetProperty(ref _selectedCurrency, value); }
        }

        private string _cost;

        [Required(ErrorMessage = "Заполните поле стоимость")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Неправильный формат стоимости")]
        public string Cost
        {
            get { return _cost; }
            set { SetProperty(ref _cost, value); }
        }

        private DateTimeOffset? _date;
        [Required(ErrorMessage = "Выб дату")]
        public DateTimeOffset? SelectedDate
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        private string _comment;
        public string Comment { get { return _comment; } set { SetProperty(ref _comment, value); } }

        public ICommand AddCommand { get; }

        private async void OnAddCommand()
        {
            if(!ValidateProperties()) return;

            var transaction = new Transaction()
            {
                AssetId = SelectedAsset.Id,
                Comment = Comment,
                Cost = Double.Parse(Cost),
                CurrencyId = SelectedCurrnecy.Id,
                Date = SelectedDate.Value.Date,
                ProductId = SelectedSubCategory.Id,
                Type = Type
            };

            using (var uow = new UnitOfWork())
            {
                uow.TransactionRepository.Insert(transaction);
                uow.Commit();
            }

            var dialog = new MessageDialog("Все Доавлено", "Добавлено");
            await dialog.ShowAsync();

            ClearFields();
        }

        private void ClearFields()
        {
            Cost = string.Empty;
            SelectedCategory = null;
            SelectedSubCategory = null;
            Comment = string.Empty;
            Errors.SetAllErrors(new Dictionary<string, ReadOnlyCollection<string>>());
        }

        public ICommand DateChangedCommand { get; }

        private void OnDateChangedCommand(object o)
        {
            SelectedDate = (DateTimeOffset?) o;
        }
    }

    public class Operation
    {
        public int Value { get; set; }
        public string Name { get; set; }
    }
}
