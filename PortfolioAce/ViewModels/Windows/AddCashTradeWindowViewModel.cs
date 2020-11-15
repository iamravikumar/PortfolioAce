using PortfolioAce.Commands;
using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PortfolioAce.ViewModels.Windows
{
    public class AddCashTradeWindowViewModel: ViewModelWindowBase
    {
        private Fund _fund;
        public AddCashTradeWindowViewModel(ICashTradeService cashService, Fund fund)
        {
            AddCashTradeCommand = new AddCashTradeCommand(this, cashService);
            _fund = fund;
            _tradeDate = DateTime.Today;
            _settleDate = DateTime.Today;
            
        }

        public int FundId
        {
            get
            {
                return _fund.FundId;
            }
            private set
            {
            }
        }

        private string _cashType;
        public string CashType
        {
            get
            {
                return _cashType;
            }
            set
            {
                _cashType = value;
                OnPropertyChanged(nameof(CashType));
            }
        }

        private decimal _cashAmount;
        public decimal CashAmount
        {
            get
            {
                return _cashAmount;
            }
            set
            {
                _cashAmount = value;
                OnPropertyChanged(nameof(CashAmount));
            }
        }

        private string _currency;
        public string TradeCurrency
        {
            get
            {
                return _currency;
            }
            set
            {
                _currency = value;
                OnPropertyChanged(nameof(TradeCurrency));
            }
        }


        private DateTime _tradeDate;
        public DateTime TradeDate
        {
            get
            {
                return _tradeDate;
            }
            set
            {
                _tradeDate = value;
                OnPropertyChanged(nameof(TradeDate));
            }
        }

        private DateTime _settleDate;
        public DateTime SettleDate
        {
            get
            {
                return _settleDate;
            }
            set
            {
                _settleDate = value;
                OnPropertyChanged(nameof(SettleDate));
            }
        }

        private string _notes;
        public string Notes
        {
            get
            {
                return _notes;
            }
            set
            {
                _notes = value;
                OnPropertyChanged(nameof(Notes));
            }
        }

        public ICommand AddCashTradeCommand { get; set; }
    }
}
