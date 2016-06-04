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
                PieChartCollection = new ObservableCollection<PieChartData>(_sourceTransactions.GroupBy(t => t.Type).Select(g => new PieChartData { Name = EnumHelper<OperationType>.GetDisplayValue((OperationType)(g.Key)), Count = g.Count() }));
                ColumnChartCollection = new ObservableCollection<ColumnChartData>(_sourceTransactions.GroupBy(t => t.Product.Parent.Name).Select(g => new ColumnChartData() { Name = g.Key, Count = g.Count() }));


                var tx = _sourceTransactions.OrderBy(t => t.Date);
                var res = new List<LineChartData>();
                if (tx.Any())
                {
                    var start = tx.First().Date;
                    var endDate = DateTime.Now;
                    int months = (endDate.Year - start.Year) * 12 + endDate.Month - start.Month;
                    for (var i = months; i >= 0; i--)
                    {
                        var i1 = i;
                        var firstDayOfMonth = new DateTime(endDate.AddMonths(-i1).Year, endDate.AddMonths(-i1).Month, 1);
                        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                        var el =
                            tx.Where(
                                e => e.Date.Date >= firstDayOfMonth && e.Date.Date <= lastDayOfMonth);

                        var cost = GetBalanceD(el);


                        res.Add(new LineChartData()
                        {
                            Name = firstDayOfMonth.ToString("MM.yyyy"),
                            Cost = (double) cost
                        });
                    }
                }


                LineChartCollection = new ObservableCollection<LineChartData>(res);
            }

            Sum = GetBalance();
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

        private string GetBalance()
        {
            var filter = Filters[IndexOfCurrencyFilter].SelectedItem;

            string code = CurrencyCode.BelarussianRub;
            if (filter.Value != null)
            {
                code = new UnitOfWork().CurrencyRepository.GetById((int)filter.Value).Code;
            }

            var currencyConverter = new CurrencyConverter();

            decimal sum = 0;
            foreach (var transaction in Transactions)
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
            return $"{Math.Round(sum, 2)} {code}";
        }

        private decimal GetBalanceD(IEnumerable<Transaction> transactions)
        {
            var filter = Filters[IndexOfCurrencyFilter].SelectedItem;

            string code = CurrencyCode.BelarussianRub;
            if (filter.Value != null)
            {
                code = new UnitOfWork().CurrencyRepository.GetById((int)filter.Value).Code;
            }

            var currencyConverter = new CurrencyConverter();

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
                PieChartCollection = new ObservableCollection<PieChartData>(filteredSource.GroupBy(t => t.Type).Select(g => new PieChartData() { Name = EnumHelper<OperationType>.GetDisplayValue((OperationType)(g.Key)), Count = g.Count() }));

                ColumnChartCollection = new ObservableCollection<ColumnChartData>(filteredSource.GroupBy(t => t.Product.Parent.Name).Select(g => new ColumnChartData { Name = g.Key, Count = g.Count() }));

                var tx = filteredSource.OrderBy(t => t.Date);
                var res = new List<LineChartData>();
                if (tx.Any())
                {
                    var start = tx.First().Date;
                    var endDate = DateTime.Now;
                    int months = (endDate.Year - start.Year) * 12 + endDate.Month - start.Month;
                    for (var i = months; i >= 0; i--)
                    {
                        var i1 = i;
                        var firstDayOfMonth = new DateTime(endDate.AddMonths(-i1).Year, endDate.AddMonths(-i1).Month, 1);
                        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                        var el =
                            tx.Where(
                                e => e.Date.Date >= firstDayOfMonth && e.Date.Date <= lastDayOfMonth);

                        var cost = GetBalanceD(el);
                       

                        res.Add(new LineChartData()
                        {
                            Name = firstDayOfMonth.ToString("MM.yyyy"),
                            Cost = (double) cost
                        });
                    }  
                }


                LineChartCollection= new ObservableCollection<LineChartData>(res);

                Sum = GetBalance();
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

