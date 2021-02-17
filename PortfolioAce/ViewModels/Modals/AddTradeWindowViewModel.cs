using PortfolioAce.Commands;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.Dimensions;
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
    public class AddTradeWindowViewModel : ViewModelWindowBase, INotifyDataErrorInfo
    {
        private Fund _fund;
        private ITransactionService _transactionService;
        private IStaticReferences _staticReferences;
        private DateTime _lastLockedDate;
        private readonly ValidationErrors _validationErrors;
        public AddTradeWindowViewModel(ITransactionService transactionService, IStaticReferences staticReferences, Fund fund)
        {
            AddTradeCommand = new AddTradeCommand(this, transactionService);
            _transactionService = transactionService;
            _fund = fund;
            _staticReferences = staticReferences;
            _lastLockedDate = _staticReferences.GetMostRecentLockedDate(fund.FundId);
            _validationErrors = new ValidationErrors();
            _validationErrors.ErrorsChanged += ChangedErrorsEvents;
            _tradeDate = DateExtentions.InitialDate();
            _settleDate = DateExtentions.InitialDate();
            _custodian = cmbCustodians[0];
            _securitiesList = _staticReferences.GetAllSecurities();
        }

        private readonly List<SecuritiesDIM> _securitiesList;
        public List<SecuritiesDIM> SecuritiesList
        {
            get
            {
                return _securitiesList;
            }
        }

        private SecuritiesDIM _SelectedSecurity;
        public SecuritiesDIM SelectedSecurity
        {
            get
            {
                return _SelectedSecurity;
            }
            set
            {
                _SelectedSecurity = value;
                _validationErrors.ClearErrors(nameof(SelectedSecurity));
                if (_SelectedSecurity== null)
                {
                    _validationErrors.AddError(nameof(SelectedSecurity), $"This Security is not recognised.");
                }
                OnPropertyChanged(nameof(SelectedSecurity));
                OnPropertyChanged(nameof(TradeCurrency));
            }
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

        private string _tradeType;
        public string TradeType
        {
            get
            {
                return _tradeType;
            }
            set
            {
                _tradeType = value;
                OnPropertyChanged(nameof(TradeType));
            }
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
                if (TradeType == "Trade")
                {
                    int multiplier = -1;
                    _tradeAmount = Math.Round((Quantity * Price * multiplier) - Commission, 2);
                }

                return _tradeAmount;
            }
            set
            {
                if (TradeType == "Corporate Action")
                {
                    _tradeAmount = value;
                }
                OnPropertyChanged(nameof(TradeAmount));
            }
        }

        public string TradeCurrency
        {
            get
            {
                if(_SelectedSecurity != null)
                {
                    return _SelectedSecurity.Currency.Symbol;
                }
                return null;
            }
            private set
            {

            }
        }

        private decimal _commission;
        public decimal Commission
        {
            get
            {
                return _commission;
            }
            set
            {
                _commission = value;
                _validationErrors.ClearErrors(nameof(Commission));
                if (_commission < 0)
                {
                    _validationErrors.AddError(nameof(Commission), "The commission cannot be a negative number");
                }
                OnPropertyChanged(nameof(Commission));
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
                    _validationErrors.AddError(nameof(TradeDate), "Your trades can't be booked on weekends");
                }
                if (_tradeDate < _fund.LaunchDate)
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

        public DateTime CreatedDate
        {
            get
            {
                return DateTime.Now;
                //return _staticReferences.GetAllTradeTypes().Select(tt=>tt.TypeName).ToList();
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


        public List<string> cmbTradeType
        {
            get
            {
                return _staticReferences.GetAllTransactionTypes().Where(t => t.TypeClass == "SecurityTrade").Select(t => t.TypeName).ToList();
            }
        }

        public List<string> cmbCustodians
        {
            get
            {
                return _staticReferences.GetAllCustodians().Select(c => c.Name).ToList();
            }
        }

        public ICommand AddTradeCommand { get; set; }

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
