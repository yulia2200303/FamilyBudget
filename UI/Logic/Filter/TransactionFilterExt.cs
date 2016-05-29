using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Model;

namespace UI.Logic.Filter
{
    public static class TransactionFilterExt
    {
        public static IEnumerable<Transaction> Filter(this IEnumerable<Transaction> transactions, FilterData data)
        {
            if (Enum.IsDefined(typeof(FilterType), data.Type))
            {
                var filterType = (FilterType)data.Type;

                switch (filterType)
                {
                    case FilterType.Sort:
                        return SortFilter(transactions, data);
                    case FilterType.Category:
                        return FilterByCategory(transactions, data);
                    case FilterType.Operation:
                        return FilterByOperation(transactions, data);
                    case FilterType.SubCategory:
                        return FilterBySubCategory(transactions, data);
                    case FilterType.Currency:
                        return FilterByCurrency(transactions, data);
                    case FilterType.Date:
                        return FilterByDate(transactions, data);
                }
            }

            return transactions;
        }

        public static IEnumerable<Transaction> Filter(this IEnumerable<Transaction> transactions, IEnumerable<FilterData> filters)
        {
            foreach (var filterData in filters)
            {
                transactions = transactions.Filter(filterData);
            }

            return transactions;
        }


        private static IEnumerable<Transaction> SortFilter(this IEnumerable<Transaction> transactions, FilterData data)
        {
            if (!Enum.IsDefined(typeof(SortFilter), data.Value)) return transactions;
            var value = (SortFilter)(int)data.Value;

            switch (value)
            {
                case Logic.Filter.SortFilter.ByCategory:
                    return transactions.OrderBy(t => t.Product.Name);
                case Logic.Filter.SortFilter.ByOperationType:
                    return transactions.OrderBy(t => t.Type);
                case Logic.Filter.SortFilter.ByCurrency:
                    return transactions.OrderBy(t => t.Currency.Code);
                case Logic.Filter.SortFilter.ByCost:
                    return transactions.OrderBy(t => t.Cost * t.Currency.Converter);
                default:
                    return transactions.OrderByDescending(t => t.Date);
            }
        }

        private static IEnumerable<Transaction> FilterByOperation(IEnumerable<Transaction> transactions, FilterData data)
        {
            if (data.Value == null) return transactions;
            var selectedValue = (int)data.Value;
            return transactions.Where(t => t.Type == selectedValue);
        }

        private static IEnumerable<Transaction> FilterByCategory(IEnumerable<Transaction> transactions, FilterData data)
        {
            if (data.Value == null) return transactions;
            var selectedValue = (int)data.Value;
            return transactions.Where(t =>t.Product.ParentId == selectedValue);
        }

        private static IEnumerable<Transaction> FilterBySubCategory(IEnumerable<Transaction> transactions, FilterData data)
        {
            if (data.Value == null) return transactions;
            var selectedValue = (int)data.Value;
            return transactions.Where(t => t.ProductId == selectedValue);
        }

        private static IEnumerable<Transaction> FilterByCurrency(IEnumerable<Transaction> transactions, FilterData data)
        {
            if (data.Value == null) return transactions;
            var selectedValue = (int)data.Value;
            return transactions.Where(t => t.CurrencyId == selectedValue);
        }

        private static IEnumerable<Transaction> FilterByDate(IEnumerable<Transaction> transactions, FilterData data)
        {
            if (data.Value == null || !Enum.IsDefined(typeof(DateFilter), data.Value)) return transactions;

            var selectedValue = (DateFilter)data.Value;
            var endDate = DateTime.Now.Date;
            DateTime startDate = new DateTime(1900, 01, 01);
            switch (selectedValue)
            {
                case DateFilter.Today:
                    startDate = endDate;
                    break;
                case DateFilter.Yesterday:
                    startDate = endDate.AddDays(-1);
                    endDate = startDate;
                    break;
                case DateFilter.Week:
                    startDate = endDate.AddDays(-7);
                    break;
                case DateFilter.Month:
                    startDate = endDate.AddMonths(-1);
                    break;
                case DateFilter.ThreeMonth:
                    startDate = endDate.AddMonths(-3);
                    break;
                case DateFilter.HalfYear:
                    startDate = endDate.AddMonths(-6);
                    break;
                case DateFilter.Year:
                    startDate = endDate.AddYears(-1);
                    break;
            }

            return transactions.Where(t => t.Date >= startDate && t.Date <= endDate);


        }
    }

}
