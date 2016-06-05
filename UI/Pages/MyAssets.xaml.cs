using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
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
        }

        private void Navigate_ToCurrency(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Currency));
        }

        private DependencyObject FindChildControl<T>(DependencyObject control, string ctrlName)
        {
            int childNumber = VisualTreeHelper.GetChildrenCount(control);
            for (int i = 0; i < childNumber; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(control, i);
                FrameworkElement fe = child as FrameworkElement;
                if (fe == null) return null;

                if (child is T && fe.Name == ctrlName)
                {
                    return child;
                }
                else
                {
                    DependencyObject nextLevel = FindChildControl<T>(child, ctrlName);
                    if (nextLevel != null)
                        return nextLevel;
                }
            }
            return null;
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var element = sender as UIElement;
            var stackPanel = FindChildControl<StackPanel>(element, "StackPanelWrapper") as StackPanel;
            if (stackPanel == null) return;
            object value = 0;
            if (stackPanel.ActualHeight > 68)
            {
                stackPanel?.Resources.TryGetValue("Hide", out value);
                Storyboard storyboard = value as Storyboard;
                storyboard?.Begin();
            }
            else
            {
                stackPanel?.Resources.TryGetValue("Show", out value);
                Storyboard storyboard = value as Storyboard;
                storyboard?.Begin();
            }
        }

        private void Navigate_ToSettings(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (Settings));
        }
    }
}