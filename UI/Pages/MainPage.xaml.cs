using Windows.Devices.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using UI.Model;
using UI.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UI.Pages
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly AccountViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();

            _viewModel = new AccountViewModel();
            DataContext = _viewModel;
        }

        private void click_b(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MyAssets));
        }

        private void UIElement_OnRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (e.PointerDeviceType != PointerDeviceType.Mouse) return;

            ListView listView = (ListView)sender;
            UserMwnuLayout.ShowAt(listView, e.GetPosition(listView));
            _viewModel.SelectedUser = (UserModel)((FrameworkElement)e.OriginalSource).DataContext;
        }

        private void UIElement_OnHolding(object sender, HoldingRoutedEventArgs e)
        {
            if (e.PointerDeviceType == PointerDeviceType.Mouse) return;

            ListView listView = (ListView)sender;
            UserMwnuLayout.ShowAt(listView, e.GetPosition(listView));
            _viewModel.SelectedUser = (UserModel)((FrameworkElement)e.OriginalSource).DataContext;
        }
    }
}