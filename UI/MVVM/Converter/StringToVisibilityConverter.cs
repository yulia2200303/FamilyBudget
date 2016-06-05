using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UI.MVVM.Converter
{
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (String.IsNullOrEmpty(value?.ToString())) return Visibility.Collapsed;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
