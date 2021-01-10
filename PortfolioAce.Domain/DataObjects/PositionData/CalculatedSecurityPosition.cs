using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioAce.Domain.DataObjects.PositionData
{
    public abstract class CalculatedSecurityPosition
    {
        public abstract SecuritiesDIM Security{ get; }
        public abstract CustodiansDIM Custodian { get; }
        public abstract decimal AverageCost { get; }
        public abstract decimal NetQuantity { get; }
        public abstract decimal RealisedPnL { get; }
        public abstract List<PositionSnapshot> PositionBreakdown { get; }
        public abstract Queue<TaxLotsOpen> OpenLots { get; }
        public abstract void AddTransaction(TransactionsBO transaction);
        public abstract void AddTransactionRange(List<TransactionsBO> transactions);
    }

    public class EquityPosition : CalculatedSecurityPosition
    {
        private decimal _averageCost;
        private decimal _netQuantity;
        private decimal _realisedPnL;
        private bool _isLong;
        private List<PositionSnapshot> _positionBreakdown;
        private Queue<TaxLotsOpen> _openLots;


        public override SecuritiesDIM Security { get; }

        public override CustodiansDIM Custodian { get; }

        public override decimal AverageCost { get { return _averageCost; } }

        public override decimal NetQuantity{get{ return _netQuantity; }}

        public override decimal RealisedPnL{get{ return _realisedPnL; }}

        public override List<PositionSnapshot> PositionBreakdown{get{ return _positionBreakdown; }}

        public override Queue<TaxLotsOpen> OpenLots{get{ return _openLots; }}

        public EquityPosition(SecuritiesDIM security, CustodiansDIM custodian)
        {
            this.Security = security;
            this.Custodian = custodian;
            _averageCost = 0;
            _netQuantity = 0;
            _realisedPnL = 0;
            _positionBreakdown = new List<PositionSnapshot>();
            _openLots = new Queue<TaxLotsOpen>();

        }

        public override void AddTransaction(TransactionsBO transaction)
        {

            if (transaction.Security.SecurityName != Security.SecurityName || transaction.Security.Currency.Symbol != Security.Currency.Symbol)
            {
                throw new InvalidOperationException("The transaction ticker does not match the ticker of this position");
            }

            if (transaction.Custodian.Name != Custodian.Name)
            {
                throw new InvalidOperationException("These transactions belongs to a different custodian");
            }

            // raise an error if the if this transaction occurs earlier than the most recent transaction.
            if (_positionBreakdown.Count > 0)
            {
                if (_positionBreakdown[_positionBreakdown.Count - 1].date > transaction.TradeDate)
                {
                    throw new InvalidOperationException("A trade must take place after the most recent trade.");
                }
            }

            decimal quantityRef = transaction.Quantity; // a reference to the transaction quantity so it doesn't get manipulated

            if (transaction.TransactionType.TypeName == "CorporateAction")
            {
                _netQuantity += quantityRef;
                _realisedPnL += transaction.TradeAmount;
                // to breakdown before return
                return;
            }

            TaxLotsOpen lot = new TaxLotsOpen(transaction.TradeDate, quantityRef, transaction.Price);

            // make sure a transaction cannot equal zero in my models
            // position has no trades
            // used to be (quantityRef * NetQuantity == 0) but if the net quantity is zero then this will always be true
            // and i need to ignore trades where the transaction.quantity is zero;
            if (_netQuantity == 0)
            {
                _isLong = (quantityRef >= 0);
            }

            //trades in diff direction to position
            else if (quantityRef * _netQuantity < 0)
            {
                while (_openLots.Count > 0 && quantityRef != 0)
                {
                    decimal pnl;
                    int multiplier = (_isLong) ? 1 : -1; // this is important because it allows me to calculate pnl taking direction into account.
                    if (Math.Abs(quantityRef) >= Math.Abs(_openLots.Peek().quantity))
                    {
                        pnl = Math.Abs(_openLots.Peek().quantity) * (transaction.Price - _openLots.Peek().price) * multiplier;
                        quantityRef += _openLots.Peek().quantity;
                        _openLots.Dequeue();
                    }
                    else
                    {
                        pnl = Math.Abs(quantityRef) * (transaction.Price - _openLots.Peek().price) * multiplier;
                        _openLots.Peek().quantity += quantityRef;
                        quantityRef = 0;
                    }
                    _realisedPnL += pnl;
                }
                if (quantityRef != 0)
                {
                    lot.quantity = quantityRef;
                }

            }
            if (quantityRef != 0)
            {
                _openLots.Enqueue(lot);
            }
            UpdatePosition(transaction);
            // i need to update position regardless, 
            // i need update the closed lots regardless(give it another name like trade summary), 
            // if transaction.Quantity!=0 need to push the lots)

            //when all said and done, net position should equal total open lots
        }

        public override void AddTransactionRange(List<TransactionsBO> transactions)
        {
            foreach (TransactionsBO transaction in transactions)
            {
                this.AddTransaction(transaction);
            }
        }


        public decimal GetTotalTradeValue()
        {
            decimal result = 0;
            if (_openLots.Count > 0)
            {
                foreach (TaxLotsOpen lot in _openLots)
                {
                    result += lot.GetTradeValue();
                }
            }
            return result;
        }
        public List<PositionSnapshot> GetBreakdown()
        {
            return this._positionBreakdown;
        }

        public List<TaxLotsOpen> GetOpenLots()
        {
            return _openLots.ToList(); // i MIGHT need to reverse the list.
        }

        private void UpdatePosition(TransactionsBO transaction)
        {
            _netQuantity = _openLots.Sum(lots => lots.quantity);
            if (transaction.Quantity == 0 && _openLots.Count > 0)
            {
                _averageCost = _openLots.Peek().price; //i believe this might need refactoring for certain edgecases i.e. going from long to short.
            }
            else
            {
                _averageCost = Math.Round(GetAverageCost(), 2);
            }
            CheckDirection();
            AppendBreakdown(transaction.TradeDate);
        }

        private void AppendBreakdown(DateTime tradeDate)
        {
            PositionSnapshot snapshot = new PositionSnapshot(tradeDate, _netQuantity, _averageCost);
            // if theres no trades add this trade
            if (_positionBreakdown.Count == 0)
            {
                this._positionBreakdown.Add(snapshot);
            }
            else
            {
                //if theres a trade that took place on the same day then override it;
                int lastIndex = this._positionBreakdown.Count - 1;
                DateTime lastTradeDate = this._positionBreakdown[lastIndex].date;
                if (tradeDate == lastTradeDate)
                {
                    this._positionBreakdown[lastIndex] = snapshot;
                }
                else
                {
                    this._positionBreakdown.Add(snapshot);
                }
            }

        }

        private decimal GetAverageCost()
        {
            if (_netQuantity == 0)
            {
                return Decimal.Zero;
            }
            decimal totalTradeValue = GetTotalTradeValue();

            return totalTradeValue / _netQuantity;
        }

        private void CheckDirection()
        {
            // if we flip position pnl might need to be reset. except for inception to date...
            if (_netQuantity < 0 && _isLong)
            {
                _isLong = false;
            }
            else if (_netQuantity > 0 && !_isLong)
            {
                _isLong = true;
            }
        }
    }


    public class CryptoPosition : CalculatedSecurityPosition
    {
        private decimal _averageCost;
        private decimal _netQuantity;
        private decimal _realisedPnL;
        private bool _isLong;
        private List<PositionSnapshot> _positionBreakdown;
        private Queue<TaxLotsOpen> _openLots;


        public override SecuritiesDIM Security { get; }

        public override CustodiansDIM Custodian { get; }

        public override decimal AverageCost { get { return _averageCost; } }

        public override decimal NetQuantity { get { return _netQuantity; } }

        public override decimal RealisedPnL { get { return _realisedPnL; } }

        public override List<PositionSnapshot> PositionBreakdown { get { return _positionBreakdown; } }

        public override Queue<TaxLotsOpen> OpenLots { get { return _openLots; } }

        public CryptoPosition(SecuritiesDIM security, CustodiansDIM custodian)
        {
            this.Security = security;
            this.Custodian = custodian;
            _averageCost = 0;
            _netQuantity = 0;
            _realisedPnL = 0;
            _positionBreakdown = new List<PositionSnapshot>();
            _openLots = new Queue<TaxLotsOpen>();

        }

        public override void AddTransaction(TransactionsBO transaction)
        {

            if (transaction.Security.SecurityName != Security.SecurityName || transaction.Security.Currency.Symbol != Security.Currency.Symbol)
            {
                throw new InvalidOperationException("The transaction ticker does not match the ticker of this position");
            }

            if (transaction.Custodian.Name != Custodian.Name)
            {
                throw new InvalidOperationException("These transactions belongs to a different custodian");
            }

            // raise an error if the if this transaction occurs earlier than the most recent transaction.
            if (_positionBreakdown.Count > 0)
            {
                if (_positionBreakdown[_positionBreakdown.Count - 1].date > transaction.TradeDate)
                {
                    throw new InvalidOperationException("A trade must take place after the most recent trade.");
                }
            }

            decimal quantityRef = transaction.Quantity; // a reference to the transaction quantity so it doesn't get manipulated

            if (transaction.TransactionType.TypeName == "CorporateAction")
            {
                _netQuantity += quantityRef;
                _realisedPnL += transaction.TradeAmount;
                // to breakdown before return
                return;
            }

            TaxLotsOpen lot = new TaxLotsOpen(transaction.TradeDate, quantityRef, transaction.Price);

            // make sure a transaction cannot equal zero in my models
            // position has no trades
            // used to be (quantityRef * NetQuantity == 0) but if the net quantity is zero then this will always be true
            // and i need to ignore trades where the transaction.quantity is zero;
            if (_netQuantity == 0)
            {
                _isLong = (quantityRef >= 0);
            }

            //trades in diff direction to position
            else if (quantityRef * _netQuantity < 0)
            {
                while (_openLots.Count > 0 && quantityRef != 0)
                {
                    decimal pnl;
                    int multiplier = (_isLong) ? 1 : -1; // this is important because it allows me to calculate pnl taking direction into account.
                    if (Math.Abs(quantityRef) >= Math.Abs(_openLots.Peek().quantity))
                    {
                        pnl = Math.Abs(_openLots.Peek().quantity) * (transaction.Price - _openLots.Peek().price) * multiplier;
                        quantityRef += _openLots.Peek().quantity;
                        _openLots.Dequeue();
                    }
                    else
                    {
                        pnl = Math.Abs(quantityRef) * (transaction.Price - _openLots.Peek().price) * multiplier;
                        _openLots.Peek().quantity += quantityRef;
                        quantityRef = 0;
                    }
                    _realisedPnL += pnl;
                }
                if (quantityRef != 0)
                {
                    lot.quantity = quantityRef;
                }

            }
            if (quantityRef != 0)
            {
                _openLots.Enqueue(lot);
            }
            UpdatePosition(transaction);
            // i need to update position regardless, 
            // i need update the closed lots regardless(give it another name like trade summary), 
            // if transaction.Quantity!=0 need to push the lots)

            //when all said and done, net position should equal total open lots
        }

        public override void AddTransactionRange(List<TransactionsBO> transactions)
        {
            foreach (TransactionsBO transaction in transactions)
            {
                this.AddTransaction(transaction);
            }
        }


        public decimal GetTotalTradeValue()
        {
            decimal result = 0;
            if (_openLots.Count > 0)
            {
                foreach (TaxLotsOpen lot in _openLots)
                {
                    result += lot.GetTradeValue();
                }
            }
            return result;
        }
        public List<PositionSnapshot> GetBreakdown()
        {
            return this._positionBreakdown;
        }

        public List<TaxLotsOpen> GetOpenLots()
        {
            return _openLots.ToList(); // i MIGHT need to reverse the list.
        }

        private void UpdatePosition(TransactionsBO transaction)
        {
            _netQuantity = _openLots.Sum(lots => lots.quantity);
            if (transaction.Quantity == 0 && _openLots.Count > 0)
            {
                _averageCost = _openLots.Peek().price; //i believe this might need refactoring for certain edgecases i.e. going from long to short.
            }
            else
            {
                _averageCost = Math.Round(GetAverageCost(), 2);
            }
            CheckDirection();
            AppendBreakdown(transaction.TradeDate);
        }

        private void AppendBreakdown(DateTime tradeDate)
        {
            PositionSnapshot snapshot = new PositionSnapshot(tradeDate, _netQuantity, _averageCost);
            // if theres no trades add this trade
            if (_positionBreakdown.Count == 0)
            {
                this._positionBreakdown.Add(snapshot);
            }
            else
            {
                //if theres a trade that took place on the same day then override it;
                int lastIndex = this._positionBreakdown.Count - 1;
                DateTime lastTradeDate = this._positionBreakdown[lastIndex].date;
                if (tradeDate == lastTradeDate)
                {
                    this._positionBreakdown[lastIndex] = snapshot;
                }
                else
                {
                    this._positionBreakdown.Add(snapshot);
                }
            }

        }

        private decimal GetAverageCost()
        {
            if (_netQuantity == 0)
            {
                return Decimal.Zero;
            }
            decimal totalTradeValue = GetTotalTradeValue();

            return totalTradeValue / _netQuantity;
        }

        private void CheckDirection()
        {
            // if we flip position pnl might need to be reset. except for inception to date...
            if (_netQuantity < 0 && _isLong)
            {
                _isLong = false;
            }
            else if (_netQuantity > 0 && !_isLong)
            {
                _isLong = true;
            }
        }
    }


public class FXPosition : CalculatedSecurityPosition
    {
        private decimal _averageCost;
        private decimal _netQuantity;
        private decimal _realisedPnL;
        private bool _isLong;
        private List<PositionSnapshot> positionBreakdown;
        private Queue<TaxLotsOpen> openLots;

        public override SecuritiesDIM Security { get; }

        public override CustodiansDIM Custodian { get; }

        public override decimal AverageCost => throw new NotImplementedException();

        public override decimal NetQuantity => throw new NotImplementedException();

        public override decimal RealisedPnL => throw new NotImplementedException();

        public override List<PositionSnapshot> PositionBreakdown => throw new NotImplementedException();

        public override Queue<TaxLotsOpen> OpenLots => throw new NotImplementedException();

        public FXPosition(SecuritiesDIM security, CustodiansDIM custodian)
        {
            this.Security = security;
            this.Custodian = custodian;

        }

        public override void AddTransaction(TransactionsBO transaction)
        {
            throw new NotImplementedException();
        }

        public override void AddTransactionRange(List<TransactionsBO> transactions)
        {
            throw new NotImplementedException();
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

    public class TaxLotsOpen
    {
        public DateTime date { get; set; }
        public decimal quantity { get; set; }
        public decimal price { get; set; }
        public TaxLotsOpen(DateTime date, decimal quantity, decimal price)
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
