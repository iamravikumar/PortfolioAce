using AutoCompleteTextBox.Editors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PortfolioAce.Models.Providers
{
    public class CountrySuggestionProvider:ISuggestionProvider
    {
        public IEnumerable<RegionInfo> AllRegions { get; set; }

        public RegionInfo GetExactSuggestion(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter)) return null;
            return
                AllRegions
                    .FirstOrDefault(r => string.Equals(r.EnglishName, filter, StringComparison.CurrentCultureIgnoreCase));
        }

        public IEnumerable<RegionInfo> GetSuggestions(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter)) return null;
            System.Threading.Thread.Sleep(500);
            return
                AllRegions
                    .Where(r => r.EnglishName.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) > -1)
                    .ToList();
        }



        IEnumerable GetFullCollection()
        {
            return AllRegions.ToList();
        }

        IEnumerable ISuggestionProvider.GetSuggestions(string filter)
        {
            return GetSuggestions(filter);
        }

        public CountrySuggestionProvider()
        {
            AllRegions = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new RegionInfo(x.LCID)).Distinct().ToList();
        }
    }
}
