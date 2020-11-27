using PortfolioAce.Commands;
using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore.Services;
using PortfolioAce.EFCore.Services.DimensionServices;
using PortfolioAce.Models;
using PortfolioAce.ViewModels.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PortfolioAce.ViewModels.Modals
{
    public class AddCashTradeWindowViewModel: ViewModelWindowBase, INotifyDataErrorInfo
    {
        private Fund _fund;
        private readonly ValidationErrors _validationErrors;
        private IStaticReferences _staticReferences;
        public AddCashTradeWindowViewModel(ICashTradeService cashService, IStaticReferences staticReferences, Fund fund)
        {
            AddCashTradeCommand = new AddCashTradeCommand(this, cashService);
            _fund = fund;
            _staticReferences = staticReferences;
            _validationErrors = new ValidationErrors();
            _validationErrors.ErrorsChanged += ChangedErrorsEvents;
            _tradeDate = DateExtentions.InitialDate();
            _settleDate = DateExtentions.InitialDate();
            
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
                if (_tradeDate.DayOfWeek == DayOfWeek.Saturday || _tradeDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    _validationErrors.AddError(nameof(TradeDate), "Your actions can't be booked on weekends");
                }
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
                if (_settleDate.DayOfWeek == DayOfWeek.Saturday || _settleDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    _validationErrors.AddError(nameof(SettleDate), "Your actions can't be booked on weekends");
                }
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

        public List<string> cmbCashType
        {
            get
            {
                return _staticReferences.GetAllCashTradeTypes().Select(ctt => ctt.TypeName).ToList();
            }
        }

        public List<string> cmbCurrency
        {
            get
            {
                return _staticReferences.GetAllCurrencies().Select(sr=>sr.Symbol).ToList();
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
