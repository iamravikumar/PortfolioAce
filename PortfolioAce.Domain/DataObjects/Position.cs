﻿using PortfolioAce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioAce.Domain.DataObjects
{
    public class Position
    {
        public readonly string symbol;
        public decimal AverageCost { get; set; }
        public decimal NetQuantity { get; set; }
        private decimal RealisedPnL { get; set; }
        private bool IsLong { get; set; }

        protected List<PositionSnapshot> positionBreakdown;
        private Queue<OpenLots> openLots;

        public Position(string symbol)
        {
            this.symbol = symbol;
            this.AverageCost = 0;
            this.NetQuantity = 0;
            this.RealisedPnL = 0;
            this.positionBreakdown = new List<PositionSnapshot>();
            this.openLots = new Queue<OpenLots>();
        }

        public void AddTransaction(Trade transaction)
        {
            if (transaction.Symbol != this.symbol)
            {
                throw new InvalidOperationException("The transaction ticker does not match the ticker of this position");
            }
            // raise an error if the if this transaction occurs earlier than the most recent transaction.
            if (positionBreakdown.Count > 0)
            {
                if(positionBreakdown[positionBreakdown.Count-1].date < transaction.TradeDate)
                {
                    throw new InvalidOperationException("A trade must take place after the most recent trade.");
                }
            }

            if (transaction.TradeType == "Corporate Action")
            {
                NetQuantity += transaction.Quantity;
                RealisedPnL += transaction.TradeAmount;
                return;
            }
            OpenLots lot = new OpenLots(transaction.TradeDate, transaction.Quantity, transaction.Price);

            // make sure a transaction cannot equal zero in my models
            // position has no trades
            // used to be (transaction.Quantity * NetQuantity == 0) but if the net quantity is zero then this will always be true
            // and i need to ignore trades where the transaction.quantity is zero;
            if (NetQuantity == 0)
            {
                this.IsLong = (transaction.Quantity >= 0);
            }

            //trades in diff direction to position
            else if (transaction.Quantity * NetQuantity < 0)
            {
                while (openLots.Count > 0 && transaction.Quantity != 0)
                {
                    decimal pnl;
                    int multiplier = (this.IsLong) ? 1 : -1; // this is important because it allows me to calculate pnl taking direction into account.
                    if (Math.Abs(transaction.Quantity) >= Math.Abs(openLots.Peek().quantity))
                    {
                        pnl = openLots.Peek().quantity * (transaction.Price - openLots.Peek().price) * multiplier;
                        // decimal pnl = (transaction.Quantity * transaction.Price) - (openLots.Peek().GetTradeValue())
                        transaction.Quantity += openLots.Peek().quantity;
                        openLots.Dequeue();
                    }
                    else
                    {
                        pnl = transaction.Quantity * (openLots.Peek().price - transaction.Price) * multiplier;
                        openLots.Peek().quantity += transaction.Quantity;
                        transaction.Quantity = 0;
                    }
                    RealisedPnL += pnl;
                }
                if (transaction.Quantity != 0)
                {
                    lot.quantity = transaction.Quantity;
                }

            }
            if (transaction.Quantity != 0)
            {
                openLots.Enqueue(lot);
            }

            UpdatePosition(transaction);
            // i need to update position regardless, 
            // i need update the closed lots regardless(give it another name like trade summary), 
            // if transaction.Quantity!=0 need to push the lots)

            //when all said and done, net position should equal total open lots
        }

        public decimal GetTotalMarketValue()
        {
            decimal result = 0;
            if (this.openLots.Count > 0)
            {
                foreach (OpenLots lot in this.openLots)
                {
                    result += lot.GetTradeValue();
                }
            }
            return result;
        }
        public List<PositionSnapshot> GetBreakdown()
        {
            return this.positionBreakdown;
        }

        private void UpdatePosition(Trade transaction)
        {
            this.NetQuantity = this.openLots.Sum(lots => lots.quantity);
            if (transaction.Quantity == 0 && this.openLots.Count > 0)
            {
                this.AverageCost = this.openLots.Peek().price;
            }
            else
            {
                this.AverageCost = GetAverageCost();
            }
            CheckDirection();
            AppendBreakdown(transaction.TradeDate);
        }

        private void AppendBreakdown(DateTime tradeDate)
        {
            PositionSnapshot snapshot = new PositionSnapshot(tradeDate, this.NetQuantity, this.AverageCost);
            // if theres no trades add this trade
            if (positionBreakdown.Count == 0)
            {
                this.positionBreakdown.Add(snapshot);
            }
            else
            {
                //if theres a trade that took place on the same day then override it;
                int lastIndex = this.positionBreakdown.Count - 1;
                DateTime lastTradeDate = this.positionBreakdown[lastIndex].date;
                if (tradeDate == lastTradeDate)
                {
                    this.positionBreakdown[lastIndex] = snapshot;
                }
                else
                {
                    this.positionBreakdown.Add(snapshot);
                }
            }

        }

        private decimal GetAverageCost()
        {
            if (this.NetQuantity == 0)
            {
                return Decimal.Zero;
            }
            decimal marketValue = GetTotalMarketValue();

            return marketValue / this.NetQuantity;
        }

        private void CheckDirection()
        {
            if (this.NetQuantity < 0 && this.IsLong)
            {
                this.IsLong = false;
            }
            else if (this.NetQuantity > 0 && !this.IsLong)
            {
                this.IsLong = true;
            }
        }
    }


    public class PositionSnapshot
    {
        public DateTime date;
        public decimal quantity;
        public decimal averageCost;
        public PositionSnapshot(DateTime date, decimal quantity, decimal averageCost)
        {
            this.date = date;
            this.quantity = quantity;
            this.averageCost = averageCost;
        }

        public override bool Equals(object other)
        {
            PositionSnapshot otherSnapshot = other as PositionSnapshot;
            if (otherSnapshot == null)
            {
                return false;
            }

            return (this.date == otherSnapshot.date &&
                    this.averageCost == otherSnapshot.averageCost &&
                    this.quantity == otherSnapshot.quantity);
        }
    }

    public class OpenLots
    {
        public DateTime date;
        public decimal quantity;
        public decimal price;
        public OpenLots(DateTime date, decimal quantity, decimal price)
        {
            this.date = date;
            this.quantity = quantity;
            this.price = price;
        }

        public decimal GetTradeValue()
        {
            return this.quantity * this.price;
        }
    }
}
