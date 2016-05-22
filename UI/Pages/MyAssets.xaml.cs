using System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UI.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UI.Pages
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyAssets : Page
    {
        public MyAssets()
        {
            InitializeComponent();
            var model = new MyAssetsViewModel();
            DataContext = model;

            var frame = Window.Current.Content as Frame;
            var page = frame.Content as Page;
            //Popup.Height = page.ActualHeight;
            //Popup.Width = page.ActualWidth;

            //var view = ApplicationView.GetForCurrentView();
            //if (view.IsFullScreenMode)
            //{
            //    view.ExitFullScreenMode();
            //    ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.Auto;
            //    // The SizeChanged event will be raised when the exit from full-screen mode is complete.
            //}
            //else
            //{
            //    if (view.TryEnterFullScreenMode())
            //    {
            //        ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;
            //        // The SizeChanged event will be raised when the entry to full-screen mode is complete.
            //    }
            //}
        }

        private void Hide_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Show_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Navigate_ToCurrency(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Currency));
        }

        private void FlyoutBase_OnOpened(object sender, object e)
        {
            //BottomAppBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            //DD.IsSticky = false;
            //DD.IsOpen = false;
            //BottomAppBar.ClosedDisplayMode = AppBarClosedDisplayMode.Hidden;
       
            //BottomAppBar.IsOpen = false;

        }

        private void FlyoutBase_OnClosed(object sender, object e)
        {
       
        }

        private void Popup_OnOpened(object sender, object e)
        {
            //Popup.Height = ActualHeight;
            //Popup.Width = ActualWidth;
        }


    }
}