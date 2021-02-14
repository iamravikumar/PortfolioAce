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
                GainPercent = Math.Round(((position.Price / position.AverageCost) - 1), 2);
                GainValue = Math.Round(position.Quantity * (position.Price - position.AverageCost), 2);
            }
            else
            {
                // if the position is short.
                GainPercent = Math.Round(((position.Price / position.AverageCost) - 1) * -1, 2);
                GainValue = Math.Round(position.Quantity * ((position.Price - position.AverageCost) * -1), 2);

            }
        }
    }
}
