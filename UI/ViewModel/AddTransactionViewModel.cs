using System.Collections.Generic;
using System.Collections.ObjectModel;
using Shared.Enum;
using UI.ViewModel.Common;

namespace UI.ViewModel
{
    public class AddTransactionViewModel : BaseViewModel
    {
        public AddTransactionViewModel()
        {
            Operations = new ObservableCollection<Operation>(new List<Operation>()
            {
                new Operation {Name = "Debit", Value = (int) OperationType.Debit},
                new Operation {Name = "Credit", Value = (int) OperationType.Credit}
            });
        }

        private int _type;

        public int Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        ObservableCollection<Operation> _operations;

        public ObservableCollection<Operation> Operations
        {
            get { return _operations; }
            set { SetProperty(ref _operations, value); }
        }
    }

    public class Operation
    {
        public int Value { get; set; }

        public string Name { get; set; }
    }
}
