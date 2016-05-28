using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DAL.Model;
using Prism.Commands;
using UI.Logic.Filter;
using UI.ViewModel.Common;

namespace UI.ViewModel
{
    class ListOfTransactionViewModel : BaseViewModel
    {

        public ListOfTransactionViewModel()
        {
            var categories = new List<FilterData>();
            categories.Add(new FilterData()
            {
                Name = "fffff",
                Type = 1,
                Value = 1
            });

            Sort = new Filter()
            {
                Name = "dfdfdfdf",
                Categories = categories
            };

            this.SortCommand = new DelegateCommand<object>(OnSortChange);
            //this.FilterCommand = new DelegateCommand<object>(OnFilterChange);
            FilterChangeCommand = new DelegateCommand<object>(OnFilterChange);

            var list = new List<Transaction>();
            
        }

        private Filter _sort;

        public Filter Sort
        {
            get { return _sort; }
            set { this.SetProperty(ref _sort, value); }
        }

        private ObservableCollection<Filter> _filters;

        public ObservableCollection<Filter> Filters
        {
            get { return _filters; }
            set { this.SetProperty(ref _filters, value); }
        }

        public ICommand SortCommand { get; private set; }

        private async void OnSortChange(object arg)
        {
            var param = arg as FilterData;
            if (param != null)
            {
                Sort.SelectedItem = param;
                // var filteredList = await PoiList.FilterPoiModelList(Filters).SortPoiList((SortOptions)Convert.ToInt32(param.Value));
                //FilteredPoiList = new IncrementalLoadingCollection<PoiModel>(filteredList);

                //IsShowNoResultMessage = !filteredList.Any();
            }
        }

        public ICommand FilterChangeCommand { get; }

        private void OnFilterChange(object o)
        {
            var x = 1;
        }

        public ICommand FilterCommand { get; private set; }

        private async void OnFilterChange1(object arg)
        {
            var param = arg as FilterData;
            if (param != null)
            {
                //foreach (var filter in Filters)
                //{
                //    if (filter.Categories.Any(c => c.Type == param.Type))
                //    {
                //        filter.SelectedItem = param;
                //        break;
                //    }
                //}

                //DataFiltering();
            }
        }
    }
}

