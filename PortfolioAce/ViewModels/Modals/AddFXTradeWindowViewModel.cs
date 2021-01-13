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
            //AddFXTradeCommand = new AddFXTradeCommand(this, transactionService);
            _fund = fund;
            _transactionService = transactionService;
            _staticReferences = staticReferences;
            _lastLockedDate = _staticReferences.GetMostRecentLockedDate(fund.FundId);
            _validationErrors = new ValidationErrors();
            _validationErrors.ErrorsChanged += ChangedErrorsEvents;
            _tradeDate = DateExtentions.InitialDate();
            _settleDate = DateExtentions.InitialDate();

        }

        private decimal _quantity;
        public decimal Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(TradeAmount));
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
                if (_price < 0)
                {
                    _validationErrors.AddError(nameof(Price), "The price cannot be a negative number");
                }

                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(TradeAmount));
            }
        }

        private decimal _tradeAmount;
        public decimal TradeAmount
        {

            get
            {


                return _tradeAmount;
            }
            set
            {

                _tradeAmount = value;
                OnPropertyChanged(nameof(TradeAmount));
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
