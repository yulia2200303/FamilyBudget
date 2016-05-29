using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Shared.Enum;
using UI.Logic;

namespace UI.MVVM.Converter
{
    public class OperationTypeToStringConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var type = (OperationType) (int) value;
            return EnumHelper<OperationType>.GetDisplayValue(type);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
