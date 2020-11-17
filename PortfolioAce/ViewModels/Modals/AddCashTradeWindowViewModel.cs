using PortfolioAce.Commands;
using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore.Services;
using PortfolioAce.Models;
using PortfolioAce.ViewModels.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace PortfolioAce.ViewModels.Modals
{
    public class AddCashTradeWindowViewModel: ViewModelWindowBase, INotifyDataErrorInfo
    {
        private Fund _fund;
        private readonly ValidationErrors _validationErrors;
        public AddCashTradeWindowViewModel(ICashTradeService cashService, Fund fund)
        {
            AddCashTradeCommand = new AddCashTradeCommand(this, cashService);
            _fund = fund;
            _validationErrors = new ValidationErrors();
            _validationErrors.ErrorsChanged += ChangedErrorsEvents;
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
                _validationErrors.ClearErrors(nameof(CashAmount));
                if (_cashAmount < 0)
                {
                    _validationErrors.AddError(nameof(CashAmount), "The cash amount cannot be a negative number");
                }
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
                _validationErrors.ClearErrors(nameof(TradeDate));
                if (_tradeDate < _fund.LaunchDate)
                {
                    // validation not showing at the moment because it is bound to TextBox at the moment
                    _validationErrors.AddError(nameof(TradeDate), "You cannot book trades before the funds launch date.");
                }
                if (_settleDate < _tradeDate)
                {
                    _settleDate = _tradeDate;
                }
                OnPropertyChanged(nameof(TradeDate));
                OnPropertyChanged(nameof(SettleDate));
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
                _validationErrors.ClearErrors(nameof(SettleDate));
                if (_settleDate < _tradeDate)
                {
                    // validation not showing at the moment because it is bound to TextBox at the moment
                    _validationErrors.AddError(nameof(SettleDate), "The SettleDate cannot take place before the trade date");
                }
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

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool CanCreate => !HasErrors;

        public bool HasErrors => _validationErrors.HasErrors;

        public IEnumerable GetErrors(string propertyName)
        {
            return _validationErrors.GetErrors(propertyName);
        }

        private void ChangedErrorsEvents(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanCreate));
        }
    }
}
