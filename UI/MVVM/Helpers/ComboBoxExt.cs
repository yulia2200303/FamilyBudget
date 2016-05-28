using System.Collections;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace UI.MVVM.Helpers
{
    public sealed class ComboBoxExt : ItemsControl
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(ComboBoxExt), new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(ComboBoxExt), new PropertyMetadata(null, SelectedItemChange));

        private static void SelectedItemChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var combobox = d as ItemsControl;

            if (combobox != null && e.NewValue != null)
            {
                combobox.SetValue(SelectedItemProperty, e.NewValue);
                combobox.SetValue(SelectedTextProperty, e.NewValue.ToString());
            }
        }

        public static readonly DependencyProperty SelectedTextProperty = DependencyProperty.Register("SelectedText", typeof(string), typeof(ComboBoxExt), new PropertyMetadata(null));

        public static readonly DependencyProperty OnSelectedCommandProperty = DependencyProperty.Register("OnSelectedCommand", typeof(ICommand), typeof(ComboBoxExt), new PropertyMetadata(null));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set
            {
                SetValue(SelectedItemProperty, value);
                SetValue(SelectedTextProperty, value.ToString());
                ICommand command = GetValue(OnSelectedCommandProperty) as ICommand;
                if (command != null)
                {
                    command.Execute(SelectedItem);
                }
            }
        }

        public string SelectedText
        {
            get { return (string)GetValue(SelectedTextProperty); }
            set { SetValue(SelectedTextProperty, value); }
        }

        public ICommand OnSelectedCommand
        {
            get { return (ICommand)GetValue(OnSelectedCommandProperty); }
            set { SetValue(OnSelectedCommandProperty, value); }
        }

        private Flyout menuFlyout;

        public ComboBoxExt()
        {
            this.DefaultStyleKey = typeof(ComboBoxExt);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var x = this; 
            ContentPresenter cp = this.ItemContainerGenerator.ContainerFromItem(SelectedItem) as ContentPresenter;
            var y = GetTemplateChild("Test");
            var t = ItemContainerGenerator.ContainerFromIndex(0);
            //var y1 = FindChildControl<MenuFlyoutItem>(y, "Test");
            menuFlyout = GetTemplateChild("") as Flyout;

            BindableFlyout.SetItemsSource(menuFlyout, (IEnumerable)ItemsSource);
            BindableFlyout.SetItemTemplate(menuFlyout, ItemTemplate);
           // BindableFlyout.SetParentBox(menuFlyout, this);
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

        protected override void OnItemsChanged(object e)
        {
            base.OnItemsChanged(e);

            //var items = (IList)ItemsSource;
            //if (items != null && items.Count > 0 && SelectedItem == null)
            //{
            //    var item = items[0];
            //    SetValue(SelectedItemProperty, item);
            //    SetValue(SelectedTextProperty, item.ToString());
            //}

            if (menuFlyout == null)
                return;

            BindableFlyout.SetItemsSource(menuFlyout, (IEnumerable)ItemsSource);
            BindableFlyout.SetItemTemplate(menuFlyout, ItemTemplate);
           // BindableFlyout.SetParentBox(menuFlyout, this);
        }
    }
}
