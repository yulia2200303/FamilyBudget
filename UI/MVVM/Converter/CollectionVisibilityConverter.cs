using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UI.MVVM.Converter
{
    public class CollectionVisibilityConverter : IValueConverter
    {
        private static Visibility ConvertBooleanToVisibility(bool value)
        {
            return value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var isVisible = false;
            var collection = value as ICollection;
            if (collection != null)
            {
                isVisible = collection.Count > 0;
            }

            return parameter != null && parameter.ToString() == "!" ?ConvertBooleanToVisibility(!isVisible) :ConvertBooleanToVisibility(isVisible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
