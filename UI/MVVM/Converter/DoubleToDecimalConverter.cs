using System;
using Windows.UI.Xaml.Data;

namespace UI.MVVM.Converter
{
    public class DoubleToDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                double d = (double) value;
                return (decimal)d;
            }
            catch (Exception e)
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
