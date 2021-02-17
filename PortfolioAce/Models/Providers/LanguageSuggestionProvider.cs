using AutoCompleteTextBox.Editors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PortfolioAce.Models.Providers
{
    public class LanguageSuggestionProvider:ISuggestionProvider
    {
        public IEnumerable<string> ListOfLanguages { get; set; }

        public string GetExactSuggestion(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter)) return null;
            return
                ListOfLanguages
                    .FirstOrDefault(lang => string.Equals(lang, filter, StringComparison.CurrentCultureIgnoreCase));
        }

        public IEnumerable<string> GetSuggestions(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter)) return null;
            System.Threading.Thread.Sleep(500);
            return
                ListOfLanguages
                    .Where(lang => lang.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) > -1)
                    .ToList();
        }



        IEnumerable GetFullCollection()
        {
            return ListOfLanguages.ToList();
        }

        IEnumerable ISuggestionProvider.GetSuggestions(string filter)
        {
            return GetSuggestions(filter);
        }

        public LanguageSuggestionProvider()
        {
            CultureInfo[] AllCultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
            ListOfLanguages = AllCultures.Select(c => c.EnglishName).ToArray();
        }
    }
}
