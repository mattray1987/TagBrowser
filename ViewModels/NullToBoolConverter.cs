using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagBrowser.ViewModels
{
    internal class NullToBoolConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            bool isNull = value == null ? true : false;
            bool invert = false;
            if (parameter != null)
            {
                invert = parameter.ToString().ToUpper() == "Invert".ToUpper() ? true : false;
            }
            if (invert)
            {
                return !isNull;
            }
            return isNull;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
