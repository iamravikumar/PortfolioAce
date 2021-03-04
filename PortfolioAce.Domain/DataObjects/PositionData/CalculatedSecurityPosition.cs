using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PortfolioAce.Domain.DataObjects.PositionData
{
    public abstract class CalculatedSecurityPosition : CalculatedPosition
    {
        // Calculations follow the FIFO methodology. I can eventually expand on this to inside follow LIFO (using a stack instead of a queue)
        public abstract SecuritiesDIM Security { get; }
        public abstract CustodiansDIM Custodian { get; }
        public abstract decimal AverageCost { get; }
        public abstract decimal NetQuantity { get; }
        public abstract decimal RealisedPnL { get; }
        public abstract List<PositionSnapshot> PositionBreakdown { get; }
        public abstract List<TaxLotsOpen> OpenLots { get; }
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

        public override decimal AverageCost { get { return Math.Round(_averageCost, 2); } }

        public override decimal NetQuantity { get { return _netQuantity; } }

        public override decimal RealisedPnL { get { return Math.Round(_realisedPnL, 2); } }

        public override List<PositionSnapshot> PositionBreakdown { get { return _positionBreakdown; } }

        public override List<TaxLotsOpen> OpenLots { get { return _openLots.ToList(); } }

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

        public override void AddTransactionRange(List<TransactionsBO> transactions)
        {
            foreach (TransactionsBO transaction in transactions)
            {
                this.AddTransaction(transaction);
            }
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

            if (_positionBreakdown.Count > 0)
            {
                if (_positionBreakdown[_positionBreakdown.Count - 1].date > transaction.TradeDate)
                {
                    throw new InvalidOperationException("A trade must take place after the most recent trade.");
                }
            }

            if (transaction.TransactionType.TypeName == "Trade")
            {
                AddTradeEvent(transaction);
            }
            else
            {
                AddCorporateActionEvent(transaction);
            }
        }

        private void AddTradeEvent(TransactionsBO transaction)
        {
            // I need to ignore/prevent trades where quantity = 0

            decimal quantityRef = transaction.Quantity; // a reference to the transaction quantity so it doesn't get manipulated


            TaxLotsOpen lot = new TaxLotsOpen(transaction.TradeDate, quantityRef, transaction.Price);

            if (_netQuantity == 0)
            {
                _isLong = (quantityRef >= 0);
            }

            // this means the trade is not in the same direction as the quantity
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
            Debug.Assert(_netQuantity == _openLots.Sum(s => s.quantity));
        }

        private void AddCorporateActionEvent(TransactionsBO transaction)
        {
            if (transaction.TransactionType.TypeName == "Dividends")
            {
                if (transaction.Quantity == 0)
                {
                    // cash dividend
                    _realisedPnL += transaction.TradeAmount;
                }
                else
                {
                    // stock dividend
                    TaxLotsOpen dividendLot = new TaxLotsOpen(transaction.TradeDate, transaction.Quantity, transaction.Price);
                    _openLots.Enqueue(dividendLot);
                    UpdatePosition(transaction);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
            Debug.Assert(_netQuantity == _openLots.Sum(s => s.quantity));
        }


        private void UpdatePosition(TransactionsBO transaction)
        {
            _netQuantity = _openLots.Sum(lots => lots.quantity);
            _averageCost = CalculateWeightedAverageCost();
            AppendBreakdown(transaction.TradeDate);

            // If the position changes direction it gets updated here. i.e. long to short from a single trade.
            if (_netQuantity < 0 && _isLong)
            {
                _isLong = false;
            }
            else if (_netQuantity > 0 && !_isLong)
            {
                _isLong = true;
            }
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

        private decimal CalculateWeightedAverageCost()
        {
            // weighted average price = (Q1*P1)+(Q2*P2)+(Q3*P3)+..../(SUM(Q)); where Q=quantity, P=price
            if (_netQuantity == 0)
            {
                return Decimal.Zero;
            }
            decimal totalCost = _openLots.Sum(ol => ol.LotCost);
            decimal totalQuantity = _openLots.Sum(ol => ol.quantity);
            decimal weightedCost = totalCost/totalQuantity;
            return weightedCost;
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

        public override decimal AverageCost { get { return Math.Round(_averageCost, 2); } }

        public override decimal NetQuantity { get { return _netQuantity; } }

        public override decimal RealisedPnL { get { return Math.Round(_realisedPnL, 2); } }

        public override List<PositionSnapshot> PositionBreakdown { get { return _positionBreakdown; } }

        public override List<TaxLotsOpen> OpenLots { get { return _openLots.ToList(); } }

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

        public override void AddTransactionRange(List<TransactionsBO> transactions)
        {
            foreach (TransactionsBO transaction in transactions)
            {
                this.AddTransaction(transaction);
            }
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

            if (_positionBreakdown.Count > 0)
            {
                if (_positionBreakdown[_positionBreakdown.Count - 1].date > transaction.TradeDate)
                {
                    throw new InvalidOperationException("A trade must take place after the most recent trade.");
                }
            }

            if (transaction.TransactionType.TypeName == "Trade")
            {
                AddTradeEvent(transaction);
            }
            else
            {
                throw new InvalidOperationException("Cryptocurrencies do not have corporate action events.");
            }
        }

        private void AddTradeEvent(TransactionsBO transaction)
        {
            // I need to ignore/prevent trades where quantity = 0

            decimal quantityRef = transaction.Quantity; // a reference to the transaction quantity so it doesn't get manipulated


            TaxLotsOpen lot = new TaxLotsOpen(transaction.TradeDate, quantityRef, transaction.Price);

            if (_netQuantity == 0)
            {
                _isLong = (quantityRef >= 0);
            }

            // this means the trade is not in the same direction as the quantity
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
            Debug.Assert(_netQuantity == _openLots.Sum(s => s.quantity));
        }


        private void UpdatePosition(TransactionsBO transaction)
        {
            _netQuantity = _openLots.Sum(lots => lots.quantity);
            _averageCost = CalculateWeightedAverageCost();
            AppendBreakdown(transaction.TradeDate);

            // If the position changes direction it gets updated here. i.e. long to short from a single trade.
            if (_netQuantity < 0 && _isLong)
            {
                _isLong = false;
            }
            else if (_netQuantity > 0 && !_isLong)
            {
                _isLong = true;
            }
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

        private decimal CalculateWeightedAverageCost()
        {
            // weighted average price = (Q1*P1)+(Q2*P2)+(Q3*P3)+..../(SUM(Q)); where Q=quantity, P=price
            if (_netQuantity == 0)
            {
                return Decimal.Zero;
            }
            decimal totalCost = _openLots.Sum(ol => ol.LotCost);
            decimal totalQuantity = _openLots.Sum(ol => ol.quantity);
            decimal weightedCost = totalCost / totalQuantity;
            return weightedCost;
        }
    }


    public class FXForwardPosition : CalculatedSecurityPosition
    {
        private decimal _averageCost;
        private decimal _netQuantity;
        private decimal _realisedPnL;
        private bool _isLong;
        private List<PositionSnapshot> _positionBreakdown;
        private Queue<TaxLotsOpen> _openLots;


        public override SecuritiesDIM Security { get; }

        public override CustodiansDIM Custodian { get; }

        public override decimal AverageCost { get { return Math.Round(_averageCost, 2); } }

        public override decimal NetQuantity { get { return _netQuantity; } }

        public override decimal RealisedPnL { get { return Math.Round(_realisedPnL, 2); } }

        public override List<PositionSnapshot> PositionBreakdown { get { return _positionBreakdown; } }

        public override List<TaxLotsOpen> OpenLots { get { return _openLots.ToList(); } }

        public FXForwardPosition(SecuritiesDIM security, CustodiansDIM custodian)
        {
            this.Security = security;
            this.Custodian = custodian;
            _averageCost = 0;
            _netQuantity = 0;
            _realisedPnL = 0;
            _positionBreakdown = new List<PositionSnapshot>();
            _openLots = new Queue<TaxLotsOpen>();

        }

        public override void AddTransactionRange(List<TransactionsBO> transactions)
        {
            foreach (TransactionsBO transaction in transactions)
            {
                this.AddTransaction(transaction);
            }
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

            if (_positionBreakdown.Count > 0)
            {
                if (_positionBreakdown[_positionBreakdown.Count - 1].date > transaction.TradeDate)
                {
                    throw new InvalidOperationException("A trade must take place after the most recent trade.");
                }
            }

            if (transaction.TransactionType.TypeName == "FXTrade" || transaction.TransactionType.TypeName == "FXTradeCollapse")
            {
                AddTradeEvent(transaction);
            }
            else
            {
                throw new InvalidOperationException("FX Trades do not have corporate action events.");
            }
        }

        private void AddTradeEvent(TransactionsBO transaction)
        {
            // I need to ignore/prevent trades where quantity = 0

            decimal quantityRef = transaction.Quantity; // a reference to the transaction quantity so it doesn't get manipulated


            TaxLotsOpen lot = new TaxLotsOpen(transaction.TradeDate, quantityRef, transaction.Price);

            if (_netQuantity == 0)
            {
                _isLong = (quantityRef >= 0);
            }

            // this means the trade is not in the same direction as the quantity
            else if (quantityRef * _netQuantity < 0)
            {
                while (_openLots.Count > 0 && quantityRef != 0)
                {
                    if (Math.Abs(quantityRef) >= Math.Abs(_openLots.Peek().quantity))
                    {
                        quantityRef += _openLots.Peek().quantity;
                        _openLots.Dequeue();
                    }
                    else
                    {
                        _openLots.Peek().quantity += quantityRef;
                        quantityRef = 0;
                    }
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
            Debug.Assert(_netQuantity == _openLots.Sum(s => s.quantity));
        }


        private void UpdatePosition(TransactionsBO transaction)
        {
            _netQuantity = _openLots.Sum(lots => lots.quantity);
            _averageCost = CalculateWeightedAverageCost();
            AppendBreakdown(transaction.TradeDate);

            // If the position changes direction it gets updated here. i.e. long to short from a single trade.
            if (_netQuantity < 0 && _isLong)
            {
                _isLong = false;
            }
            else if (_netQuantity > 0 && !_isLong)
            {
                _isLong = true;
            }
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

        private decimal CalculateWeightedAverageCost()
        {
            // weighted average price = (Q1*P1)+(Q2*P2)+(Q3*P3)+..../(SUM(Q)); where Q=quantity, P=price
            if (_netQuantity == 0)
            {
                return Decimal.Zero;
            }
            decimal totalCost = _openLots.Sum(ol => ol.LotCost);
            decimal totalQuantity = _openLots.Sum(ol => ol.quantity);
            decimal weightedCost = totalCost / totalQuantity;
            return weightedCost;
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
        public decimal LotCost
        {
            get
            {
                return quantity * price;
            }
        }
        public TaxLotsOpen(DateTime date, decimal quantity, decimal price)
        {
            this.date = date;
            this.quantity = quantity;
            this.price = price;
        }
    }
}
