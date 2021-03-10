using PortfolioAce.Commands;
using PortfolioAce.Commands.CRUDCommands;
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

    public class InvestorActionViewModel : ViewModelWindowBase, INotifyDataErrorInfo
    {
        private ITransferAgencyService investorService;
        private Fund _fund;
        private readonly ValidationErrors _validationErrors;
        private IStaticReferences _staticReferences;
        private DateTime _lastLockedDate;

        public InvestorActionViewModel(ITransferAgencyService investorService, IStaticReferences staticReferences, Fund fund)
        {
            // if _isNavFinal. disable the price and quantity box, which means amount is entered manually
            this.investorService = investorService;
            this._fund = fund;
            _staticReferences = staticReferences;
            _tradeDate = DateExtentions.InitialDate();
            _settleDate = DateExtentions.InitialDate();
            _lastLockedDate = _staticReferences.GetMostRecentLockedDate(fund.FundId);

            _isNavFinal = false;
            _TAType = cmbIssueType[0]; // this defaults the type to subscription..
            _validationErrors = new ValidationErrors();
            _validationErrors.ErrorsChanged += ChangedErrorsEvents;
            // currency should be the funds base currency
            AddSubscriptionCommand = new AddSubscriptionCommand(this, investorService);
            AddRedemptionCommand = new AddRedemptionCommand(this, investorService);
        }

        public bool TargetFundWaterMark
        {
            get
            {
                return _fund.HasHighWaterMark;
            }
        }

        public string TargetFundBaseCurrency
        {
            get
            {
                return _fund.BaseCurrency;
            }
        }

        public decimal TargetFundMinimumInvestment
        {
            get
            {
                return _fund.MinimumInvestment;
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

        private string _TAType;
        public string TAType
        {
            get
            {
                return _TAType;
            }
            set
            {
                _TAType = value;
                OnPropertyChanged(nameof(TAType));
                OnPropertyChanged(nameof(isSubscription));
            }
        }

        private bool _isNavFinal;
        public bool isNavFinal
        {
            get
            {
                return _isNavFinal;
            }
            set
            {
                _isNavFinal = value;
                OnPropertyChanged(nameof(isNavFinal));
            }
        }


        public List<InvestorsDIM> cmbInvestors
        {
            get
            {
                return investorService.GetAllInvestors();
            }
        }

        private InvestorsDIM _selectedInvestor;
        public InvestorsDIM SelectedInvestor
        {
            get
            {
                return _selectedInvestor;
            }
            set
            {
                _selectedInvestor = value;
                OnPropertyChanged(nameof(SelectedInvestor));
            }
        }

        private decimal _units;
        public decimal Units
        {
            get
            {
                return _units;
            }
            set
            {
                _units = value;
                _validationErrors.ClearErrors(nameof(Units));
                if (_units >= 0)
                {
                    _validationErrors.AddError(nameof(Units), "The Redemption Amount must be a positive number");
                }
                OnPropertyChanged(nameof(Units));
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
                _validationErrors.ClearErrors(nameof(Units));
                if (_tradeAmount<0)
                {
                    _validationErrors.AddError(nameof(Units), "The subscription amount cannot be a negative number");
                }
                OnPropertyChanged(nameof(TradeAmount));
            }
        }


        public string Currency
        {
            get
            {
                return _fund.BaseCurrency;
            }
            private set
            {

            }
        }

        private decimal _fee;
        public decimal Fee
        {
            get
            {
                return _fee;
            }
            set
            {
                _fee = value;
                OnPropertyChanged(nameof(Fee));
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
                    _validationErrors.AddError(nameof(TradeDate), "Your actions can't be booked before funds launch date.");
                }
                if (_tradeDate <= _lastLockedDate)
                {
                    _validationErrors.AddError(nameof(TradeDate), "You cannot book actions on a locked period");
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
                    _validationErrors.AddError(nameof(SettleDate), "The SettleDate cannot take place before the Action date");
                }
                OnPropertyChanged(nameof(SettleDate));
            }
        }

        public List<string> cmbIssueType
        {
            get
            {
                return _staticReferences.GetAllIssueTypes().Select(i => i.TypeName).ToList();
            }
        }

        public bool isSubscription
        {
            get
            {
                return (_TAType == "Subscription");
            }
        }

        public ICommand AddSubscriptionCommand { get; }
        public ICommand AddRedemptionCommand { get; }

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
