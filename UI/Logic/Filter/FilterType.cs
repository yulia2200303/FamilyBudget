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
        ByDate = 1,
        ByCurrency = 2,
        ByCategory = 3,
        ByOperationType = 4
    }

    public enum OperationFilter
    {
        All = -1,
        Debit = (int)OperationType.Debit,
        Credit = (int)OperationType.Credit
    }
}