using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UI.Pages
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            //using (var uow = new UnitOfWork())
            //{
            //    var currency = new Currency()
            //    {
            //        Code = "USD",
            //        Name = "American dollar",
            //        Converter = 1,
            //    };
            //    uow.CurrencyRepository.Insert(currency);
            //    uow.Commit();

            //    var currencies = uow.CurrencyRepository.GetAll().ToList();


            //}
        }

        private void click_b(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (MyAssets));
        }
    }
}