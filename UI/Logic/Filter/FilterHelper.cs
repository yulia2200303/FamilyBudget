using System.Collections.Generic;
using DAL.Model;

namespace UI.Logic.Filter
{
    public static class FilterHelper
    {
        public static void DoFiltering(List<Transaction> transactions, Filter filter)
        {
            var filterData = filter.SelectedItem;
        }

        private static void SortFilter(List<Transaction> transactions, FilterData data)
        {
            var value = data.Value as SortFilter? ?? Logic.Filter.SortFilter.ByDate;

            switch (value)
            {
                case Logic.Filter.SortFilter.ByCategory:
                    break;
                case Logic.Filter.SortFilter.ByOperationType:
                    break;
                case Logic.Filter.SortFilter.ByCurrency:
                    break;
                default:
                    break;
            }
        }
    }






}
