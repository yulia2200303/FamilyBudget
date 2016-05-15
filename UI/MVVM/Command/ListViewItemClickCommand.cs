using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.MVVM.Command
{
    public sealed class ListViewItemClickCommand
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
                typeof (ICommand),
                typeof (ListViewItemClickCommand),
                new PropertyMetadata(null, CommandChanged));

    
        public static void SetCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject target)
        {
            return (ICommand)target.GetValue(CommandProperty);
        }

        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var listView = target as ListView;
            if (listView == null) return;

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                listView.SelectionChanged += OnListViewItemClick;
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                listView.SelectionChanged -= OnListViewItemClick;
            }
        }

        private static void OnListViewItemClick(object sender, RoutedEventArgs e)
        {
            ListView listView = sender as ListView;
            if (listView != null)
            {
                ICommand command = (ICommand)listView.GetValue(CommandProperty);
                var selectedItem = listView.SelectedItem;
                command.Execute(selectedItem);
            }
        }
    }
}
