using System;
using System.Windows;
using System.Windows.Data;

namespace PortfolioAce.Models
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class MultiValueBoolToInvisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool a = (bool)values[0];
            bool b = (bool)values[1];

            return a && b ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
