using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            Frame.Navigate(typeof (Currency));
        }
    }
}