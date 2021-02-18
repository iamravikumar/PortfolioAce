using AutoCompleteTextBox.Editors;
using PortfolioAce.Domain.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioAce.Models.Providers
{
    public class FundSuggestionProvider: ISuggestionProvider
    {
        public IEnumerable<Fund> FundList { get; set; }

        public Fund GetExactSuggestion(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter)) return null;
            return
                FundList
                    .FirstOrDefault(fund => string.Equals(fund.FundName, filter, StringComparison.CurrentCultureIgnoreCase));
        }

        public IEnumerable<Fund> GetSuggestions(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter)) return null;
            System.Threading.Thread.Sleep(500);
            return
                FundList
                    .Where(fund => fund.FundName.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) > -1)
                    .ToList();
        }



        IEnumerable GetFullCollection()
        {
            return FundList.ToList();
        }

        IEnumerable ISuggestionProvider.GetSuggestions(string filter)
        {
            return GetSuggestions(filter);
        }

        public FundSuggestionProvider(IEnumerable<Fund> funds)
        {
            FundList = funds;
        }
    }
}
