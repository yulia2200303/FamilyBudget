using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.MVVM.Command
{
    public class PasswordChangeTextCommnand
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
                typeof (ICommand),
                typeof (PasswordChangeTextCommnand),
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
            var passwordBox = target as PasswordBox;
            if (passwordBox == null) return;

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                passwordBox.PasswordChanged += OnListViewItemClick;
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                passwordBox.PasswordChanged -= OnListViewItemClick;
            }
        }

        private static void OnListViewItemClick(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                ICommand command = (ICommand)passwordBox.GetValue(CommandProperty);
                var password = passwordBox.Password;
                command.Execute(password);
            }
        }
    }
}
