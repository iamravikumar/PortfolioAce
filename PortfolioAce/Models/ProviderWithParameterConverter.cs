using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.Models.Providers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace PortfolioAce.Models
{
    public class ProviderWithParameterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // unorthodox but i could create a factory for doing this...
            if(value is IEnumerable<SecuritiesDIM>)
            {
                IEnumerable<SecuritiesDIM> ListOfSecurities = (List<SecuritiesDIM>)value;
                return new SecuritySuggestionProvider(ListOfSecurities);
            }
            else if(value is IEnumerable<Fund>)
            {
                IEnumerable<Fund> ListOfFunds = (List<Fund>)value;
                return new FundSuggestionProvider(ListOfFunds);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
