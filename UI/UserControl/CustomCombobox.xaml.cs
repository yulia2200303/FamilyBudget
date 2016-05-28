using System.Collections;
using System.ComponentModel;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using UI.MVVM.Helpers;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UI.UserControl
{
    public sealed partial class CustomCombobox : Windows.UI.Xaml.Controls.UserControl, INotifyPropertyChanged
    {
        public CustomCombobox()
        {
            this.InitializeComponent();
            selectedItem = "";
        }

        private string _selectedItem;

        public string selectedItem
        {
            get { return _selectedItem; }

            set
            {
                _selectedItem = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("selectedItem"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuFlyoutItem;
            if (item == null) return;
            selectedItem = item.Text;
            SelectedItem = item.DataContext;

            RootButton.Flyout.Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RootButton.Flyout.ShowAt(RootButton);
            //var button = sender as Button;
            //button.Flyout.ShowAt(button);
            //FlyoutBase.ShowAttachedFlyout(sender as Button);
        }


        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem",
            typeof (object), typeof (CustomCombobox), new PropertyMetadata(null, SelectedItemChange));

        private static void SelectedItemChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var combobox = d as CustomCombobox;

            if (combobox != null && e.NewValue != null)
            {
                combobox.SetValue(SelectedItemProperty, e.NewValue);
                // combobox.SetValue(SelectedTextProperty, e.NewValue.ToString());
            }
        }

        public static readonly DependencyProperty SelectedTextProperty = DependencyProperty.Register("SelectedText", typeof(string), typeof(CustomCombobox), new PropertyMetadata(null));


        public string SelectedText
        {
            get { return (string)GetValue(SelectedTextProperty); }
            set { SetValue(SelectedTextProperty, value); }
        }


        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(CustomCombobox), new PropertyMetadata(null));


        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }


        
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(CustomCombobox), new PropertyMetadata(null));

        public object SelectedItem
        {
            get { return (object) GetValue(SelectedItemProperty); }
            set
            {
                SetValue(SelectedItemProperty, value);
                //    SetValue(SelectedTextProperty, value.ToString());
                ICommand command = GetValue(OnSelectedCommandProperty) as ICommand;
                if (command != null)
                {
                    command.Execute(SelectedItem);
                }
            }
        }

        public ICommand OnSelectedCommand
        {
            get { return (ICommand)GetValue(OnSelectedCommandProperty); }
            set { SetValue(OnSelectedCommandProperty, value); }
        }


        public static readonly DependencyProperty OnSelectedCommandProperty = DependencyProperty.Register("OnSelectedCommand", typeof(ICommand), typeof(CustomCombobox), new PropertyMetadata(null));


    }
}
