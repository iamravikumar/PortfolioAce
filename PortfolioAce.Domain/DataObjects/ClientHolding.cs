using PortfolioAce.Domain.Models.BackOfficeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioAce.Domain.DataObjects
{
    public class ClientHolding
    {
        public string ClientName { get; set; }
        public decimal Units { get; set; }
        public decimal AverageCost { get; set; }
        public decimal RealisedPnL { get; set; }
        public List<TransferAgencyBO> Transactions {get;set;}
        private Queue<OpenLots> openLots;
        public ClientHolding(string client)
        {
            ClientName = client;
            Units = 0;
            AverageCost = 0;
            RealisedPnL = 0;
            Transactions = new List<TransferAgencyBO>();
            openLots = new Queue<OpenLots>();
        }

        public void AddClientTransactions(List<TransferAgencyBO> clientTransactions)
        {
            foreach(TransferAgencyBO clientTransaction in clientTransactions)
            {
                this.AddClientTransaction(clientTransaction);
            }
        }
        public void AddClientTransaction(TransferAgencyBO clientTransaction)
        {
            if(clientTransaction.InvestorName != this.ClientName)
            {
                throw new InvalidOperationException("This client does not match the client of this holding");
            }
            if (Transactions.Count > 0)
            {
                if (Transactions[Transactions.Count - 1].TransactionDate > clientTransaction.TransactionDate)
                {
                    throw new InvalidOperationException("A transaction must take place after the most recent trade.");
                }
            }
            Transactions.Add(clientTransaction);
            decimal unitsReference = clientTransaction.Units; // a reference to the transaction quantity so it doesn't get manipulated
            OpenLots lot = new OpenLots(clientTransaction.TransactionDate, unitsReference, clientTransaction.NAVPrice);
            //trades in diff direction to position
            if (unitsReference * Units < 0)
            {
                // ol 500, 200, ref 400
                while (openLots.Count > 0 && unitsReference != 0)
                {
                    decimal pnl;
                    if (Math.Abs(unitsReference) >= Math.Abs(openLots.Peek().quantity))
                    {
                        pnl = Math.Abs(openLots.Peek().quantity) * (clientTransaction.NAVPrice - openLots.Peek().price);
                        unitsReference += openLots.Peek().quantity;
                        openLots.Dequeue();
                    }
                    else
                    {
                        pnl = Math.Abs(unitsReference) * (clientTransaction.NAVPrice - openLots.Peek().price);
                        openLots.Peek().quantity += unitsReference;
                        unitsReference = 0;
                    }
                    RealisedPnL += pnl;
                }
                if (unitsReference != 0)
                {
                    lot.quantity = unitsReference;
                }

            }
            if (unitsReference != 0)
            {
                openLots.Enqueue(lot);
            }

            UpdatePosition(clientTransaction);
            // i need to update position regardless, 
            // i need update the closed lots regardless(give it another name like trade summary), 
            // if transaction.Quantity!=0 need to push the lots)

            //when all said and done, net position should equal total open lots
        }
        private void UpdatePosition(TransferAgencyBO transaction)
        {
            Units = this.openLots.Sum(lots => lots.quantity);
            if (Units < 0)
            {
                throw new InvalidOperationException("You cannot have negative units");
            }
            if (transaction.Units == 0 && this.openLots.Count > 0)
            {
                this.AverageCost = this.openLots.Peek().price; //i believe this might need refactoring for certain edgecases i.e. going from long to short.
            }
            else
            {
                this.AverageCost = Math.Round(GetAverageCost(), 2);
            }
        }
        private decimal GetAverageCost()
        {
            if (Units == 0)
            {
                return Decimal.Zero;
            }
            decimal totalTradeValue = GetTotalTradeValue();

            return totalTradeValue / Units;
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
    }
}
