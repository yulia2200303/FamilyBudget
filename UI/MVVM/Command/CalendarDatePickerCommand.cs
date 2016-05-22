using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UI.MVVM.Command
{
   public sealed class CalendarDatePickerCommand
    {
        public static DependencyProperty CommandProperty =
           DependencyProperty.RegisterAttached("Command",
               typeof(ICommand),
               typeof(CalendarDatePickerCommand),
               new PropertyMetadata(null, CommandChanged));

        public static DependencyProperty SelectedDateProperty =
          DependencyProperty.RegisterAttached("SelectedDate",
              typeof(DateTimeOffset?),
              typeof(CalendarDatePickerCommand),
              new PropertyMetadata(null, SetDateChanged));

      


        public static void SetSelectedDate(DependencyObject target, DateTimeOffset? value)
        {
            target.SetValue(SelectedDateProperty, value);
        }

        public static DateTimeOffset? GetSelectedDate(DependencyObject target)
        {
            return (DateTimeOffset?)target.GetValue(SelectedDateProperty);
        }

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
            var calendar = target as CalendarDatePicker;
            if (calendar == null) return;
         
            if ((e.NewValue != null) && (e.OldValue == null))
            {
                calendar.DateChanged += ListViewOnDateChanged;
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                calendar.DateChanged -= ListViewOnDateChanged;
            }
        }

        private static void SetDateChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var calendar = target as CalendarDatePicker;
            if (calendar == null) return;
            DateTimeOffset? command = (DateTimeOffset?)calendar.GetValue(SelectedDateProperty);
        }

        private static void ListViewOnDateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            var calendar = sender;
            ICommand command = (ICommand)calendar.GetValue(CommandProperty);
            var selectedItem = calendar.Date;
            command.Execute(selectedItem);
        }
    }
}
