using PortfolioAce.Commands;
using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore.Services;
using PortfolioAce.EFCore.Services.DimensionServices;
using PortfolioAce.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PortfolioAce.ViewModels.Modals
{
    public class AddFXTradeWindowViewModel : ViewModelWindowBase, INotifyDataErrorInfo
    {
        private Fund _fund;
        private readonly ValidationErrors _validationErrors;
        private IStaticReferences _staticReferences;
        private ITransactionService _transactionService;
        private DateTime _lastLockedDate;
        public AddFXTradeWindowViewModel(ITransactionService transactionService, IStaticReferences staticReferences, Fund fund)
        {
            _fund = fund;
            _transactionService = transactionService;
            _staticReferences = staticReferences;
            _lastLockedDate = _staticReferences.GetMostRecentLockedDate(fund.FundId);
            _validationErrors = new ValidationErrors();
            _validationErrors.ErrorsChanged += ChangedErrorsEvents;
            _tradeDate = DateExtentions.InitialDate();
            _settleDate = DateExtentions.InitialDate();
            AddFXTradeCommand = new AddFXTradeCommand(this, transactionService);

        }

        public ICommand AddFXTradeCommand { get; set; }



        private string _buyCurrency;
        public string BuyCurrency
        {
            get
            {
                return _buyCurrency;
            }
            set
            {
                _buyCurrency = value;
                _validationErrors.ClearErrors(nameof(BuyCurrency));
                if (_buyCurrency==_sellCurrency)
                {
                    _validationErrors.AddError(nameof(BuyCurrency), "You cannot exchange the same currency");
                }
                OnPropertyChanged(nameof(BuyCurrency));
                OnPropertyChanged(nameof(NoteLabel));
            }
        }

        private string _sellCurrency;
        public string SellCurrency
        {
            get
            {
                return _sellCurrency;
            }
            set
            {
                _sellCurrency = value;
                _validationErrors.ClearErrors(nameof(SellCurrency));
                if (_buyCurrency == _sellCurrency)
                {
                    _validationErrors.AddError(nameof(SellCurrency), "You cannot exchange the same currency");
                }
                OnPropertyChanged(nameof(SellCurrency));
                OnPropertyChanged(nameof(NoteLabel));
                OnPropertyChanged(nameof(SellAmountLabel));
            }
        }

        private decimal _buyAmount;
        public decimal BuyAmount
        {
            get
            {
                return _buyAmount;
            }
            set
            {
                _buyAmount = value;
                _validationErrors.ClearErrors(nameof(BuyAmount));
                if (_buyAmount < 0)
                {
                    _validationErrors.AddError(nameof(BuyAmount), "The buy amount cannot be negative");
                }
                OnPropertyChanged(nameof(BuyAmount));
                OnPropertyChanged(nameof(SellAmount));
                OnPropertyChanged(nameof(SellAmountLabel));
            }
        }

        private decimal _price;
        public decimal Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
                _validationErrors.ClearErrors(nameof(Price));
                if (_price <= 0)
                {
                    _validationErrors.AddError(nameof(Price), "The price cannot be a negative number");
                }

                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(SellAmount));
                OnPropertyChanged(nameof(SellAmountLabel));
                OnPropertyChanged(nameof(NoteLabel));

            }
        }

        public string SellAmountLabel
        {
            get
            {
                decimal amount = Math.Round(_sellAmount = _buyAmount * _price * -1, 2);
                return $"{amount} {_sellCurrency}";
            }
        }

        private decimal _sellAmount;
        public decimal SellAmount
        {
            get
            {
               return Math.Round(_sellAmount = _buyAmount * _price * -1, 2);
            }
        }

        public string NoteLabel
        {
            get
            {
                return $"BUY {_buyCurrency}/ SELL {_sellCurrency} @ {_price}. SD {_settleDate.ToString("dd/MM/yy")}";
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
                if (_tradeDate <= _lastLockedDate)
                {
                    _validationErrors.AddError(nameof(TradeDate), "You cannot book trades on a locked period");
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
                OnPropertyChanged(nameof(NoteLabel));
            }
        }

        public DateTime CreatedDate
        {
            get
            {
                return DateTime.Now;
            }
        }
        public DateTime LastModifiedDate
        {
            get
            {
                return DateTime.Now;
            }
        }
        public bool isActive
        {
            get
            {
                return true;
            }
        }
        public bool isLocked
        {
            get
            {
                return false;
            }
        }

        private string _custodian;
        public string Custodian
        {
            get
            {
                return _custodian;
            }
            set
            {
                _custodian = value;
                OnPropertyChanged(nameof(Custodian));
            }
        }

        public List<string> cmbCurrency
        {
            get
            {
                return _staticReferences.GetAllCurrencies().Select(c => c.Symbol).ToList();
            }
        }

        public List<string> cmbCustodians
        {
            get
            {
                return _staticReferences.GetAllCustodians().Select(c => c.Name).ToList();
            }
        }


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
