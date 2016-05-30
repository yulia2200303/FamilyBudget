using System;
using System.Linq;
using Windows.UI;
using Windows.UI.ViewManagement;
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
                // Not a framework element or is null
                if (fe == null) return null;

                if (child is T && fe.Name == ctrlName)
                {
                    // Found the control so return
                    return child;
                }
                else
                {
                    // Not found it - search children
                    DependencyObject nextLevel = FindChildControl<T>(child, ctrlName);
                    if (nextLevel != null)
                        return nextLevel;
                }
            }
            return null;
        }

        public static T FindParent<T>(DependencyObject element) where T : DependencyObject
        {
            while (element != null)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(element);
                T candidate = parent as T;
                if (candidate != null)
                {
                    return candidate;
                }

                element = parent;
            }

            return default(T);
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