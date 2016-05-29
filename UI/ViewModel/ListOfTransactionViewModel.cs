﻿using System;
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

                Transactions = new ObservableCollection<Transaction>(_sourceTransactions.Filter(Filters.Select(f => f.SelectedItem)));

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

