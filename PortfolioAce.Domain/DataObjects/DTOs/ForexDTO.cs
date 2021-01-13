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
    }
}
