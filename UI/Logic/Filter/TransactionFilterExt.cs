using System.Collections.Generic;
using System.Linq;
using DAL.Model;

namespace UI.Logic.Filter
{
    public static class TransactionFilterExt
    {
        public static void DoFiltering(List<Transaction> transactions, Filter filter)
        {
            var filterData = filter.SelectedItem;
        }

        public static IEnumerable<Transaction> Filter(this IEnumerable<Transaction> transactions, FilterData data)
        {
            var filterType = (FilterType) data.Type;

            switch (filterType)
            {
                case FilterType.Sort:
                    return SortFilter(transactions, data);
          
                default:
                    return transactions.OrderByDescending(t => t.Date);
            }
        }


        private static IEnumerable<Transaction> SortFilter(this IEnumerable<Transaction> transactions, FilterData data)
        {
            var value = data.Value as SortFilter? ?? Logic.Filter.SortFilter.ByDate;

            switch (value)
            {
                case Logic.Filter.SortFilter.ByCategory:
                    return transactions.OrderBy(t => t.Product.Name);
                case Logic.Filter.SortFilter.ByOperationType:
                    return transactions.OrderBy(t => t.Type);
                case Logic.Filter.SortFilter.ByCurrency:
                    return transactions.OrderBy(t => t.Currency.Code);
                default:
                    return transactions.OrderByDescending(t => t.Date);
            }
        }

        private static IEnumerable<Transaction> FilterByOperation(this IEnumerable<Transaction> transactions, FilterData data)
        {
            var selectedValue = (int) data.Value;
            return selectedValue == (int) OperationFilter.All ? transactions : transactions.Where(t => t.Type == selectedValue);
        }

        private static IEnumerable<Transaction> FilterByCategory(this IEnumerable<Transaction> transactions,
            FilterData data)
        {
            var selectedValue = data.Value;
            if (string.IsNullOrEmpty(selectedValue?.ToString()))
                return transactions;

            return transactions.Where(t => t.Product.ParentId == null && t.Product.Name.Equals(selectedValue));
        } 
    }






}
