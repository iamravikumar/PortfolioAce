using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.HelperObjects
{
    public class PriceContainer
    {
        public DateTime Date { get; set; }
        public decimal ClosePrice { get; set; }
        public PriceContainer(DateTime date, decimal price)
        {
            this.Date = date;
            this.ClosePrice = price;
        }
        public PriceContainer()
        {

        }
    }
}
