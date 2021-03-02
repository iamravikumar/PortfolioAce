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
using System.Windows.Input;

namespace PortfolioAce.ViewModels.Modals
{
    public class AddCashTradeWindowViewModel : ViewModelWindowBase, INotifyDataErrorInfo
    {
        // Transfer fees will need to be seperate lines in the future if they are greater than zero!
        private Fund _fund;
        private readonly ValidationErrors _validationErrors;
        private IStaticReferences _staticReferences;
        private ITransactionService _transactionService;
        private DateTime _lastLockedDate;
        public AddCashTradeWindowViewModel(ITransactionService transactionService, IStaticReferences staticReferences, Fund fund)
        {
            _fund = fund;
            _transactionService = transactionService;
            _staticReferences = staticReferences;
            _lastLockedDate = _staticReferences.GetMostRecentLockedDate(fund.FundId);
            _validationErrors = new ValidationErrors();
            _validationErrors.ErrorsChanged += ChangedErrorsEvents;
            AddCashTradeCommand = new AddCashTradeCommand(this, transactionService);
            TransferCashCommand = new TransferCashCommand(this, transactionService);
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
                OnPropertyChanged(nameof(isTransfer));
                CashAmount = _cashAmount; // this is to iniate the setter on the CashAmount
            }
        }

        public bool isTransfer
        {
            get
            {
                return (_cashType == "CashTransfer");
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
                string direction = "None";
                if (_cashType != null)
                {
                    direction = _staticReferences.GetTransactionType(_cashType).Direction;
                }

                if (direction == "Inflow")
                {
                    if (_cashAmount < 0)
                    {
                        _validationErrors.AddError(nameof(CashAmount), "You cannot have a negative inflow");
                    }
                }
                else if (direction == "Outflow")
                {
                    if (_cashAmount > 0)
                    {
                        _validationErrors.AddError(nameof(CashAmount), "You cannot have a positive outflow");
                    }
                }
                else if (direction == "None" && _cashType == "CashTransfer")
                {
                    if (_cashAmount <= 0)
                    {
                        _validationErrors.AddError(nameof(CashAmount), "Transfers must be greater than 0");
                    }
                    OnPropertyChanged(nameof(RecipientFee));
                    OnPropertyChanged(nameof(PayeeFee));
                }
                OnPropertyChanged(nameof(CashAmount));
            }
        }

        public decimal Quantity
        {
            get
            {
                return _cashAmount;
            }
            // my reason for having a quantity is because i am securitising cash.
            // this means the quantity is the CashPosition and the CashAmount is the cash balance.
        }

        public decimal Price
        {
            get
            {
                return decimal.One;
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

        public string Symbol
        {
            get
            {
                return $"{_currency}c";
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
            }
        }

        public decimal Fees
        {
            get
            {
                return decimal.Zero;
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

        private string _recipientCustodian;
        public string RecipientCustodian
        {
            get
            {
                return _recipientCustodian;
            }
            set
            {
                _recipientCustodian = value;
                _validationErrors.ClearErrors(nameof(RecipientCustodian));
                if (_recipientCustodian == _payeeCustodian)
                {
                    _validationErrors.AddError(nameof(RecipientCustodian), "You cannot transfer to the same account");
                }
                OnPropertyChanged(nameof(RecipientCustodian));
            }
        }

        private string _payeeCustodian;
        public string PayeeCustodian
        {
            get
            {
                return _payeeCustodian;
            }
            set
            {
                _payeeCustodian = value;
                _validationErrors.ClearErrors(nameof(PayeeCustodian));
                if (_recipientCustodian == _payeeCustodian)
                {
                    _validationErrors.AddError(nameof(PayeeCustodian), "You cannot transfer to the same account");
                }
                OnPropertyChanged(nameof(PayeeCustodian));
            }
        }

        public decimal PayeeAmount
        {
            get
            {
                // example you transfer £800, £50 of which is fees...
                return _cashAmount * -1;
            }
        }

        public decimal RecipientAmount
        {
            get
            {
                // using the PayeeAmount example you recieve £750 minus any fees you are charged.
                return _cashAmount - _payeeFee - _recipientFee;
            }
        }

        private decimal _recipientFee;
        public decimal RecipientFee
        {
            get
            {
                return _recipientFee;
            }
            set
            {
                _recipientFee = value;
                _validationErrors.ClearErrors(nameof(RecipientFee));
                if (_recipientFee < 0)
                {
                    _validationErrors.AddError(nameof(RecipientFee), "The fee cannot be a negative number");
                }
                else if (_recipientFee > _cashAmount)
                {
                    _validationErrors.AddError(nameof(RecipientFee), "The fee cannot greater than the transfer amount");

                }
            }
        }

        private decimal _payeeFee;
        public decimal PayeeFee
        {
            get
            {
                return _payeeFee;
            }
            set
            {
                _payeeFee = value;
                _validationErrors.ClearErrors(nameof(PayeeFee));
                if (_payeeFee < 0)
                {
                    _validationErrors.AddError(nameof(PayeeFee), "The fee cannot be a negative number");
                }
                else if (_payeeFee > _cashAmount)
                {
                    _validationErrors.AddError(nameof(PayeeFee), "The fee cannot greater than the transfer amount");

                }
            }
        }

        public string RecipientNotes
        {
            get
            {
                return $"Recieve {_cashAmount:N2} {_currency} FROM {_payeeCustodian}. Total Fees {_recipientFee + _payeeFee:N2}";
            }
        }

        public string PayeesNotes
        {
            get
            {
                return $"Transfer {_cashAmount:N2} {_currency} TO {_recipientCustodian}.";
            }
        }
        public List<string> cmbCashType
        {
            get
            {
                return _staticReferences.GetAllTransactionTypes().Where(t => t.TypeClass == "CashTrade" && t.TypeName != "FXBuy" && t.TypeName != "FXSell").Select(t => t.TypeName).ToList();
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

        public ICommand AddCashTradeCommand { get; }
        public ICommand TransferCashCommand { get; set; }

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
