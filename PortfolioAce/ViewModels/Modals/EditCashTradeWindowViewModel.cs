using PortfolioAce.Commands;
using PortfolioAce.Domain.Models.BackOfficeModels;
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
    public class EditCashTradeWindowViewModel : ViewModelWindowBase, INotifyDataErrorInfo
    {
        private TransactionsBO _transaction;
        private readonly ValidationErrors _validationErrors;
        private IStaticReferences _staticReferences;
        private ITransactionService _transactionService;
        private DateTime _launchDate;
        private DateTime _lastLockedDate;
        public EditCashTradeWindowViewModel(ITransactionService transactionService, IStaticReferences staticReferences, TransactionsBO transaction)
        {
            _transactionService = transactionService;
            _transaction = transaction;
            _staticReferences = staticReferences;
            _validationErrors = new ValidationErrors();
            _validationErrors.ErrorsChanged += ChangedErrorsEvents;
            _lastLockedDate = _staticReferences.GetMostRecentLockedDate(transaction.FundId);
            _cashType = transaction.TransactionType.TypeName;
            _cashAmount = transaction.TradeAmount;
            _tradeCurrency = transaction.Currency.Symbol;
            _tradeDate = transaction.TradeDate;
            _settleDate = transaction.SettleDate;
            _custodian = transaction.Custodian.Name;
            _launchDate = transaction.Fund.LaunchDate;
            _notes = transaction.Comment;
            EditCashTradeCommand = new EditCashTradeCommand(this, transactionService, transaction);

        }

        public ICommand EditCashTradeCommand { get; }


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
                CashAmount = _cashAmount; // this is to iniate the setter on the CashAmount
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
                OnPropertyChanged(nameof(CashAmount));
            }
        }

        private string _tradeCurrency;
        public string TradeCurrency
        {
            get
            {
                return _tradeCurrency;
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
                    _validationErrors.AddError(nameof(TradeDate), "Your trades can't be booked on weekends");
                }
                if (_tradeDate < _launchDate)
                {
                    // validation not showing at the moment because it is bound to TextBox at the moment
                    _validationErrors.AddError(nameof(TradeDate), "You cannot trade before the funds launch date.");
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
                // include on property changed for settle date here if trade date< settle date.
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
                    _validationErrors.AddError(nameof(SettleDate), "Your trades can't settle on weekends");
                }
                if (_settleDate < _tradeDate)
                {
                    // validation not showing at the moment because it is bound to TextBox at the moment
                    _validationErrors.AddError(nameof(SettleDate), "The SettleDate cannot take place before the trade date");
                }
                OnPropertyChanged(nameof(SettleDate));
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

        public DateTime LastModifiedDate
        {
            get
            {
                return DateTime.Now;
            }
        }

        public List<string> cmbCashType
        {
            get
            {
                return _staticReferences.GetAllTransactionTypes().Where(t => t.TypeClass == "CashTrade").Select(t => t.TypeName).ToList();
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
