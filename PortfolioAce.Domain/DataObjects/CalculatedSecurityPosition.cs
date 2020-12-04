using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioAce.Domain.DataObjects
{
    public class CalculatedSecurityPosition
    {
        public SecuritiesDIM security { get; }
        public CustodiansDIM custodian { get; }
        public decimal AverageCost { get; set; }
        public decimal NetQuantity { get; set; }
        public decimal RealisedPnL { get; set; } // think about how to incorporate commission here
        private bool IsLong { get; set; }

        protected List<PositionSnapshot> positionBreakdown;
        private Queue<OpenLots> openLots;

        public CalculatedSecurityPosition(SecuritiesDIM security, CustodiansDIM custodian)
        {
            this.security = security;
            this.custodian = custodian;
            this.AverageCost = 0;
            this.NetQuantity = 0;
            this.RealisedPnL = 0;
            this.positionBreakdown = new List<PositionSnapshot>();
            this.openLots = new Queue<OpenLots>();
            
        }

        public void AddTransactions(List<TransactionsBO> transactions)
        {
            foreach (TransactionsBO transaction in transactions)
            {
                this.AddTransaction(transaction);
            }
        }

        public void AddTransaction(TransactionsBO transaction)
        {

            if (transaction.Security.SecurityName != this.security.SecurityName || transaction.Security.Currency.Symbol.ToString() != this.security.Currency.Symbol.ToString())
            {
                throw new InvalidOperationException("The transaction ticker does not match the ticker of this position");
            }

            if (transaction.Custodian.Name != this.custodian.Name)
            {
                throw new InvalidOperationException("These transactions belongs to a different custodian");
            }

            // raise an error if the if this transaction occurs earlier than the most recent transaction.
            if (positionBreakdown.Count > 0)
            {
                if (positionBreakdown[positionBreakdown.Count - 1].date > transaction.TradeDate)
                {
                    throw new InvalidOperationException("A trade must take place after the most recent trade.");
                }
            }

            decimal quantityRef = transaction.Quantity; // a reference to the transaction quantity so it doesn't get manipulated

            if (transaction.TransactionType.TypeName.ToString() == "CorporateAction")
            {
                NetQuantity += quantityRef;
                RealisedPnL += transaction.TradeAmount;
                // to breakdown before return
                return;
            }

            OpenLots lot = new OpenLots(transaction.TradeDate, quantityRef, transaction.Price);

            // make sure a transaction cannot equal zero in my models
            // position has no trades
            // used to be (quantityRef * NetQuantity == 0) but if the net quantity is zero then this will always be true
            // and i need to ignore trades where the transaction.quantity is zero;
            if (NetQuantity == 0)
            {
                this.IsLong = (quantityRef >= 0);
            }

            //trades in diff direction to position
            else if (quantityRef * NetQuantity < 0)
            {
                while (openLots.Count > 0 && quantityRef != 0)
                {
                    decimal pnl;
                    int multiplier = (this.IsLong) ? 1 : -1; // this is important because it allows me to calculate pnl taking direction into account.
                    if (Math.Abs(quantityRef) >= Math.Abs(openLots.Peek().quantity))
                    {
                        pnl = Math.Abs(openLots.Peek().quantity) * (transaction.Price - openLots.Peek().price) * multiplier;
                        quantityRef += openLots.Peek().quantity;
                        openLots.Dequeue();
                    }
                    else
                    {
                        pnl = Math.Abs(quantityRef) * (transaction.Price - openLots.Peek().price) * multiplier;
                        openLots.Peek().quantity += quantityRef;
                        quantityRef = 0;
                    }
                    RealisedPnL += pnl;
                }
                if (quantityRef != 0)
                {
                    lot.quantity = quantityRef;
                }

            }
            if (quantityRef != 0)
            {
                openLots.Enqueue(lot);
            }

            UpdatePosition(transaction);
            // i need to update position regardless, 
            // i need update the closed lots regardless(give it another name like trade summary), 
            // if transaction.Quantity!=0 need to push the lots)

            //when all said and done, net position should equal total open lots
        }

        public decimal GetTotalTradeValue()
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

        public List<OpenLots> GetOpenLots()
        {
            return this.openLots.ToList(); // i MIGHT need to reverse the list.
        }

        private void UpdatePosition(TransactionsBO transaction)
        {
            this.NetQuantity = this.openLots.Sum(lots => lots.quantity);
            if (transaction.Quantity == 0 && this.openLots.Count > 0)
            {
                this.AverageCost = this.openLots.Peek().price; //i believe this might need refactoring for certain edgecases i.e. going from long to short.
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
            decimal totalTradeValue = GetTotalTradeValue();

            return totalTradeValue / this.NetQuantity;
        }

        private void CheckDirection()
        {
            // if we flip position pnl might need to be reset. except for inception to date...
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
