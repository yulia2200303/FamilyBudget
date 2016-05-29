using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common;
using Shared.Enum;

namespace UI.Logic.Filter
{
    public static class FilterBuilder
    {
        public static Filter BuildDateFilter()
        {
            var items = new List<FilterData>
            {
                new FilterData
                {
                    Name = "All",
                    Type = (int) FilterType.Date,
                    Value = null
                }
            };

            foreach (var el in EnumHelper<DateFilter>.GetValues())
            {
                items.Add(new FilterData
                {
                    Name = EnumHelper<DateFilter>.GetDisplayValue(el),
                    Type = (int)FilterType.Date,
                    Value = (int)el
                });
            }

            return new Filter
            {
                Name = "Date",
                Items = items,
                SelectedItem = items[0]
            };
        }

        public static Filter BuildCategoryFilter()
        {
            var items = new List<FilterData>()
            {
                new FilterData
                {
                    Name = "All",
                    Type = (int) FilterType.Category,
                    Value = null
                }
            };

            using (var uow = new UnitOfWork())
            {
                var categories = uow.CategoryRepository.GetCategories();
                foreach (var category in categories)
                {
                    items.Add(new FilterData()
                    {
                        Name = category.Name,
                        Type = (int)FilterType.Category,
                        Value = category.Id
                    });
                }
            }

            return new Filter
            {
                Items = items,
                Name = "Category",
                SelectedItem = items[0]
            };
        }

        public static Filter BuildSubCategoryFilter(string category)
        {
            var items = new List<FilterData>
            {
                new FilterData
                {
                    Name = "All",
                    Type = (int) FilterType.SubCategory,
                    Value = null
                }
            };

            using (var uow = new UnitOfWork())
            {
                var subcategories = uow.CategoryRepository.GetSubCategories(category);
                foreach (var subcategory in subcategories)
                {
                    items.Add(new FilterData
                    {
                        Name = subcategory.Name,
                        Type = (int)FilterType.SubCategory,
                        Value = subcategory.Id
                    });
                }
            }

            return new Filter
            {
                Items = items,
                Name = "Sub Category",
                SelectedItem = items[0]
            };
        }

        public static Filter BuildSubCategoryFilter()
        {
            var items = new List<FilterData>
            {
                new FilterData
                {
                    Name = "All",
                    Type = (int) FilterType.SubCategory,
                    Value = null
                }
            };

            return new Filter
            {
                Items = items,
                Name = "Sub Category",
                SelectedItem = items[0]
            };
        }

        public static Filter BuildCurrencyFilter()
        {
            var items = new List<FilterData>
            {
                new FilterData
                {
                    Name = "All",
                    Type = (int) FilterType.Currency,
                    Value = null
                }
            };

            using (var uow = new UnitOfWork())
            {
                var currencies = uow.CurrencyRepository.GetAll();
                foreach (var currency in currencies)
                {
                    items.Add(new FilterData
                    {
                        Name = currency.Code,
                        Type = (int)FilterType.Currency,
                        Value = currency.Id
                    });
                }
            }

            return new Filter
            {
                Items = items,
                Name = "Curency",
                SelectedItem = items[0]
            };
        }

        public static Filter BuildOperationFilter()
        {
            var items = new List<FilterData>()
            {
                new FilterData
                {
                    Name = "All",
                    Type = (int) FilterType.Operation,
                    Value = null
                },
                new FilterData
                {
                    Name = "Debit",
                    Type = (int) FilterType.Operation,
                    Value = (int) OperationType.Debit
                },
                new FilterData
                {
                    Name = "Credit",
                    Type = (int) FilterType.Operation,
                    Value = (int) OperationType.Credit
                },
            };

            return new Filter()
            {
                Name = "Operation",
                Items = items,
                SelectedItem = items[0]
            };
        }

        public static Filter BuildSortFilter()
        {
            var items = new List<FilterData>();
            foreach (var el in EnumHelper<SortFilter>.GetValues())
            {
                items.Add(new FilterData()
                {
                    Name = EnumHelper<SortFilter>.GetDisplayValue(el),
                    Type = (int)FilterType.Sort,
                    Value = (int)el
                });
            }

            return new Filter
            {
                Name = "Sort By",
                Items = items,
                SelectedItem = items[0]
            };
        }

        public static List<Filter> BuildFilters()
        {
            return new List<Filter> { BuildSortFilter(), BuildDateFilter(), BuildCurrencyFilter(), BuildCategoryFilter(), BuildSubCategoryFilter(), BuildOperationFilter() };
        }
    }
}
