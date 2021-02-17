using System;
using System.Collections.Generic;
using System.Text;
using AutoCompleteTextBox.Editors;
using System.Linq;
using PortfolioAce.Domain.Models.Dimensions;
using System.Collections;

namespace PortfolioAce.Models.Providers
{
    public class SecuritySuggestionProvider:ISuggestionProvider
    {
        public IEnumerable<SecuritiesDIM> SecurityList { get; set; }

        public SecuritiesDIM GetExactSuggestion(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter)) return null;
            return
                SecurityList
                    .FirstOrDefault(security => string.Equals(security.Symbol, filter, StringComparison.CurrentCultureIgnoreCase));
        }

        public IEnumerable<SecuritiesDIM> GetSuggestions(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter)) return null;
            System.Threading.Thread.Sleep(1000);
            return
                SecurityList
                    .Where(security => security.Symbol.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) > -1)
                    .ToList();
        }



        IEnumerable GetFullCollection()
        {
            return SecurityList.ToList();
        }

        IEnumerable ISuggestionProvider.GetSuggestions(string filter)
        {
            return GetSuggestions(filter);
        }

        public SecuritySuggestionProvider(IEnumerable<SecuritiesDIM> sec)
        {
            /*
            IEnumerable<SecuritiesDIM> securities = new List<SecuritiesDIM> { 
                                                    new SecuritiesDIM { SecurityName="Advanced Micro Devices", Symbol="AMD"},
                                                    new SecuritiesDIM { SecurityName="Apple", Symbol="AAPL"}
                                                    }; // i need a way to pass in the list of securities.... 
            SecurityList = securities;
            */
            SecurityList = sec;
        }
    }
}
