using System.ComponentModel.DataAnnotations;
using Shared.Enum;

namespace UI.Logic.Filter
{
    public enum FilterType
    {
        Sort = 1,
        Category = 2,
        SubCategory = 3,
        Date = 4,
        Operation = 5,
        Currency = 6
    }

    public enum SortFilter
    {
        [Display(Name = "Date")]
        ByDate = 1,
        [Display(Name = "Currency")]
        ByCurrency = 2,
        [Display(Name = "Category")]
        ByCategory = 3,
        [Display(Name = "Operation")]
        ByOperationType = 4,
        [Display(Name = "Cost")]
        ByCost = 5
           
    }

    public enum DateFilter
    {
        [Display(Name = "Today")]
        Today = 1,
        [Display(Name = "Yesterday")]
        Yesterday = 2,
        [Display(Name = "Week")]
        Week = 3,
        [Display(Name = "Month")]
        Month = 4,
        [Display(Name = "Three Month")]
        ThreeMonth = 5,
        [Display(Name = "Half Year")]
        HalfYear = 6,
        [Display(Name = "Year")]
        Year = 7
    }

    public enum OperationFilter
    {
        [Display(Name = "Debit")]
        Debit = OperationType.Debit,
        [Display(Name = "Credit")]
        Credit = OperationType.Credit
    }
}