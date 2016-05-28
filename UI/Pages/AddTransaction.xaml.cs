using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using DAL.Model;
using UI.ViewModel;
using UI.ViewModel.Common;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddTransaction : Page
    {
        public AddTransaction()
        {
            this.InitializeComponent();
            //CalendarDatePicker.Date = null;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var model = new AddTransactionViewModel((AssetViewModel)e.Parameter);
            DataContext = model;
        }
    }
}
