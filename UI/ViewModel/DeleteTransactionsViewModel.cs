using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using DAL.Common;
using DAL.Model;
using Microsoft.Practices.Prism.Commands;
using Shared.Constant;
using Shared.Enum;
using UI.Logic;
using UI.Logic.Filter;
using UI.ViewModel.Common;

namespace UI.ViewModel
{
    public class DeleteTransactionsViewModel : BaseViewModel
    {
        private int assetId;
        public DeleteTransactionsViewModel(int assetId)
        {
            this.assetId = assetId;
            FilterChangeCommand = new DelegateCommand<object>(OnFilterChange);
            Filters = new ObservableCollection<Filter>(FilterBuilder.BuildFilters());

            InitializeTransactions();
        }

        private void InitializeTransactions()
        {
            using (var uow = new UnitOfWork())
            {
                _sourceTransactions = uow.TransactionRepository.GetByAssetId(assetId);
                Transactions = new ObservableCollection<Transaction>(_sourceTransactions.Filter(Filters.Select(f => f.SelectedItem)));
            }
        }

        private List<Transaction> _sourceTransactions;

        private const int IndexOfSubCategoryFilter = 4;


        public async Task Remove(IEnumerable<Transaction> transactions)
        {
            var dialog = new MessageDialog("Вы точно хотите удалить выбранные элементы");

            dialog.Commands.Add(new UICommand("Ok") { Id = 0 });

            dialog.Commands.Add(new UICommand("Cancel") { Id = 1 });

            dialog.DefaultCommandIndex = 0;

            dialog.CancelCommandIndex = 1;

            var result = await dialog.ShowAsync();

            if ((int)result.Id == 1) return;

            using (var uow = new UnitOfWork())
            {
                foreach (var transaction in transactions)
                {
                    uow.TransactionRepository.Delete(transaction);
                }

                uow.Commit();
            }
            InitializeTransactions();
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
