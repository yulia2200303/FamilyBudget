using System.Collections;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UI.UserControl
{
    public sealed partial class CustomCombobox : Windows.UI.Xaml.Controls.UserControl
    {
        public CustomCombobox()
        {
            this.InitializeComponent();
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuFlyoutItem;
            if (item == null) return;

            SelectedItem = item.DataContext;
            RootButton.Flyout.Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RootButton.Flyout.ShowAt(RootButton);
        }


        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem",
            typeof (object), typeof (CustomCombobox), new PropertyMetadata(null, SelectedItemChange));

        private static void SelectedItemChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var combobox = d as CustomCombobox;

            if (combobox != null && e.NewValue != null)
            {
                ICommand command = combobox.GetValue(OnSelectedCommandProperty) as ICommand;
                command?.Execute(combobox.SelectedItem);
            }
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource",
            typeof(IEnumerable), typeof(CustomCombobox), new PropertyMetadata(null));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", 
            typeof(string), typeof(CustomCombobox), new PropertyMetadata(null));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set {SetValue(SelectedItemProperty, value);}
        }

        public static readonly DependencyProperty OnSelectedCommandProperty = DependencyProperty.Register("OnSelectedCommand",
            typeof(ICommand), typeof(CustomCombobox), new PropertyMetadata(null));

        public ICommand OnSelectedCommand
        {
            get { return (ICommand)GetValue(OnSelectedCommandProperty); }
            set { SetValue(OnSelectedCommandProperty, value); }
        }
    }
}
