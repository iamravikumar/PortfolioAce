using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace PortfolioAce.Models
{
    //<Summary>Converts ENum to strings, uses Description for the parameter passed in, or parameter as string</summary>
    [ValueConversion(typeof(Enum), typeof(String))]
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string parameterString = parameter as string;
            if (parameterString == null)
                return DependencyProperty.UnsetValue;

            if (Enum.IsDefined(value.GetType(), value) == false)
                return DependencyProperty.UnsetValue;

            var desc = (value.GetType().GetField(parameterString).GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute);
            if (desc != null)
                return desc.Description;
            else
                return parameter.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string enumString = parameter.ToString();
            return enumString;
        }
    }
}
