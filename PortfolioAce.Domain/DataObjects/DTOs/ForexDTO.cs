using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.Domain.DataObjects.DTOs
{
    public class ForexDTO
    {
        /*
         * GBP/USD, GBP is the Base Currency, USD is the Quote Currency and GBP is the Base Currency. 
         * If you BUY a currency pair then you buy Base Currency and Sell the Quote Currency
         * If you sell a Currency pair then you sell the quote currency and buy the base currency.
         * 
         */

        public string BuyCurrency { get; set; }
        public string SellCurrency { get; set; }
        public decimal BuyAmount { get; set; }
        public decimal SellAmount { get; set; }
        public decimal Price { get; set; }
        public DateTime TradeDate { get; set; }
        public DateTime SettleDate { get; set; }
        public string Custodian { get; set; }
        public int FundId { get; set; }

        public string Description
        {
            get
            {
                return $"{BuyCurrency}/{SellCurrency} {Price} {SettleDate.ToString("ddMMyy")}";
            }
        }
        public string Symbol
        {
            get
            {
                return $"{BuyCurrency}{SellCurrency}{SettleDate.ToString("ddMMyy")}";
            }
        }
        public string Name
        {
            get
            {
                return $"FX FWD {BuyCurrency}/{SellCurrency} {SettleDate.ToString("dd/MM/yy")}";
            }
        }

    }
}
