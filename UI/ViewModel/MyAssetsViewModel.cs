using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.Practices.Prism.Commands;
using UI.Pages;
using UI.ViewModel.Common;

namespace UI.ViewModel
{
    public class MyAssetsViewModel: BaseViewModel
    {
        public MyAssetsViewModel()
        {
            var userName = Logic.UserContext.Current.UserName;

            AddTransactionCommand = new DelegateCommand(OnAddTransactionClick);
        }

        public ICommand AddTransactionCommand { get; }

        private void OnAddTransactionClick()
        {
            var rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof (AddTransaction));
        }
    }
}
