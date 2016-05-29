using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DAL.Model;
using UI.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DeleteTransactions : Page
    {
        private DeleteTransactionsViewModel model;
        public DeleteTransactions()
        {
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            model = new DeleteTransactionsViewModel((int)e.Parameter);
            DataContext = model;
        }

        private async void DeleteTransactionBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItems = TransactionGridView.SelectedItems.Cast<Transaction>().ToList();
                await model.Remove(selectedItems);
            }
            catch (Exception ex)
            {
                var x = ex;
            }

        }
    }
}
