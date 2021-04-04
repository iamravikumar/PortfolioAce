using PortfolioAce.Domain.Models.FactTables;
using System;

namespace PortfolioAce.Domain.DataObjects
{
    public class PositionFactPerformance
    {
        public PositionFACT Position { get; set; }
        public decimal GainPercent { get; set; }
        public decimal GainValue { get; set; }
        public PositionFactPerformance(PositionFACT position)
        {
            this.Position = position;
            if (position.Quantity >= 0)
            {
                GainPercent = (position.Price / position.AverageCost) - 1;
                GainValue =position.Quantity * (position.Price - position.AverageCost);
            }
            else
            {
                // if the position is short.
                GainPercent = ((position.Price / position.AverageCost) - 1) * -1;
                GainValue = position.Quantity * ((position.Price - position.AverageCost) * -1);

            }
        }
    }
}
