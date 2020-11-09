using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.HelperObjects
{

    public static class StaticDataObjects
    {
        // MAYBE use Enums for all of these instead, they are more flexible. 
        public static readonly string[] Currencies = new string[]
        {
            "GBP", "EUR", "USD", "JPY", "CHF", "CAD",
            "AUD", "NZD", "BTC"
        };

        public static readonly string[] NavFrequency = new string[]
        {
            "Daily", "Monthly"
        };

        public static readonly string[] SecurityTradeTypes = new string[]
        {
            "Security Trade", "Corporate Action"
        };

        public static readonly string[] CashTradeTypes = new string[]
        {
            "Income", "Expense"
        };

        public static readonly string[] IssueTypes = new string[]
        {
            "Subscription", "Redemption"
        };

    }
}
