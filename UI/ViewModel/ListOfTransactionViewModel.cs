using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Popups;
using DAL.Common;
using DAL.Model;
using Prism.Commands;
using Shared.Constant;
using Shared.Enum;
using UI.Logic;
using UI.Logic.Filter;
using UI.Model;
using UI.ViewModel.Common;

namespace UI.ViewModel
{
    class ListOfTransactionViewModel : BaseViewModel
    {
        public ListOfTransactionViewModel(int assetId)
        {
            FilterChangeCommand = new DelegateCommand<object>(OnFilterChange);
            Filters = new ObservableCollection<Filter>(FilterBuilder.BuildFilters());

            using (var uow = new UnitOfWork())
            {
                _sourceTransactions = uow.TransactionRepository.GetByAssetId(assetId);
                Transactions = new ObservableCollection<Transaction>(_sourceTransactions.Filter(Filters.Select(f => f.SelectedItem)));
                Sum = GetBalanseString(Transactions);

                SetPieChartData(_sourceTransactions);
                SetColumnChartData(_sourceTransactions);
                SetLineChartData(_sourceTransactions);
            }
        }

        private readonly List<Transaction> _sourceTransactions;

        private const int IndexOfSubCategoryFilter = 4;
        private const int IndexOfCurrencyFilter = 2;

        private string _sum;

        public string Sum
        {
            get { return _sum; }
            set { SetProperty(ref _sum, value); }
        }

        private void SetPieChartData(IEnumerable<Transaction> transactions)
        {
            PieChartCollection = new ObservableCollection<PieChartData>(transactions.GroupBy(t => t.Type)
                .Select(g => new PieChartData
                {
                    Name = EnumHelper<OperationType>.GetDisplayValue((OperationType)(g.Key)),
                    Count = g.Count()
                }));
        }

        private void SetColumnChartData(IEnumerable<Transaction> transactions)
        {
            ColumnChartCollection = new ObservableCollection<ColumnChartData>(transactions.GroupBy(t => t.Product.Parent.Name)
                .Select(g => new ColumnChartData()
                {
                    Name = g.Key,
                    Count = g.Count()
                }));

        }

        private void SetLineChartData(IEnumerable<Transaction> transactions)
        {
            var listOfDate = transactions.OrderBy(t => t.Date);
            var result = new List<LineChartData>();

            if (listOfDate.Any())
            {
                var start = listOfDate.First().Date;
                var endDate = DateTime.Now;
                int months = (endDate.Year - start.Year) * 12 + endDate.Month - start.Month;
                for (var i = months; i >= 0; i--)
                {
                    var local = i;
                    var firstDayOfMonth = new DateTime(endDate.AddMonths(-local).Year, endDate.AddMonths(-local).Month, 1);
                    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                    var el =
                        listOfDate.Where(
                            e => e.Date.Date >= firstDayOfMonth && e.Date.Date <= lastDayOfMonth);

                    var cost = GetBalance(el);

                    result.Add(new LineChartData()
                    {
                        Name = firstDayOfMonth.ToString("MM.yyyy"),
                        Cost = (double)cost
                    });
                }
            }

            LineChartCollection = new ObservableCollection<LineChartData>(result);
        }

        private decimal GetBalance(IEnumerable<Transaction> transactions)
        {
            var currencyConverter = new CurrencyConverter();
            string code = GetCurrencyCode();
            decimal sum = 0;
            foreach (var transaction in transactions)
            {
                if (transaction.Type == (int)OperationType.Debit)
                {
                    sum -= (decimal)(transaction.Cost * currencyConverter.Convert(transaction.Currency.Code, code));
                }
                else
                {
                    sum += (decimal)(transaction.Cost * currencyConverter.Convert(transaction.Currency.Code, code));
                }
            }
            return sum;
        }

        private string GetCurrencyCode()
        {

            var filter = Filters[IndexOfCurrencyFilter].SelectedItem;

            string code = CurrencyCode.BelarussianRub;
            if (filter.Value != null)
            {
                code = new UnitOfWork().CurrencyRepository.GetById((int)filter.Value).Code;
            }
            return code;
        }

        private string GetBalanseString(IEnumerable<Transaction> transactions)
        {
            var sum = GetBalance(transactions);
            string code = GetCurrencyCode();
            return $"{Math.Round(sum, 2)} {code}";
        }

        private ObservableCollection<Filter> _filters;

        public ObservableCollection<Filter> Filters
        {
            get { return _filters; }
            set { SetProperty(ref _filters, value); }
        }

        private ObservableCollection<Transaction> _transactions;

        public ObservableCollection<Transaction> Transactions
        {
            get { return _transactions; }
            set { SetProperty(ref _transactions, value); }
        }

        private ObservableCollection<PieChartData> _pieChartCollection;

        public ObservableCollection<PieChartData> PieChartCollection
        {
            get { return _pieChartCollection; }
            set { SetProperty(ref _pieChartCollection, value); }
        }

        private ObservableCollection<ColumnChartData> _columnChartCollection;

        public ObservableCollection<ColumnChartData> ColumnChartCollection
        {
            get { return _columnChartCollection; }
            set { SetProperty(ref _columnChartCollection, value); }
        }

        private ObservableCollection<LineChartData> _lineChartCollection;

        public ObservableCollection<LineChartData> LineChartCollection
        {
            get { return _lineChartCollection; }
            set { SetProperty(ref _lineChartCollection, value); }
        }

        public ICommand FilterChangeCommand { get; }

        private async void OnFilterChange(object o)
        {
            try
            {
                var filterDate = o as FilterData;
                if (filterDate == null) return;

                if (filterDate.Type == (int)FilterType.Category)
                {
                    Filters[IndexOfSubCategoryFilter] = FilterBuilder.BuildSubCategoryFilter(filterDate.Name);
                }

                var filterIndex = GetFilterIndex((FilterType)filterDate.Type);
                Filters[filterIndex].SelectedItem = filterDate;


                var filteredSource = _sourceTransactions.Filter(Filters.Select(f => f.SelectedItem));
                Transactions = new ObservableCollection<Transaction>(filteredSource);
                Sum = GetBalanseString(Transactions);

                SetPieChartData(filteredSource);
                SetColumnChartData(filteredSource);
                SetLineChartData(filteredSource);
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message);
                await dialog.ShowAsync();
            }
        }

        private int GetFilterIndex(FilterType type)
        {
            for (int i = 0; i < Filters.Count; i++)
            {
                if ((FilterType)Filters[i].SelectedItem.Type == type) return i;
            }
            return -1;
        }

        public ICommand FilterCommand { get; private set; }

    }
}

