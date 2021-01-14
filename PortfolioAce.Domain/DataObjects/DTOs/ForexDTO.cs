using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.Domain.DataObjects.DTOs
{
    public class ForexDTO
    {
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
