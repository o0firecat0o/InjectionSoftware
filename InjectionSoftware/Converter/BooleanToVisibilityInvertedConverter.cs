using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace InjectionSoftware.Converter
{
    /// <summary>
    /// Convert bool of false to visible
    /// Convert bool of true to invisible/collapsed
    /// </summary>
    public class BooleanToVisibilityInvertedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool param = false;

            if (parameter != null)
                param = System.Convert.ToBoolean(parameter);

            bool state = (bool)value;

            Visibility visibility = Visibility.Visible;

            if (state != param)
                visibility = Visibility.Collapsed;

            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
