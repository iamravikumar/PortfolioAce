using PortfolioAce.Commands;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.EFCore.Services;
using PortfolioAce.EFCore.Services.DimensionServices;
using PortfolioAce.Models;
using PortfolioAce.ViewModels.Modals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PortfolioAce.ViewModels.Modals
{

    public class InvestorActionViewModel : ViewModelWindowBase, INotifyDataErrorInfo
    {
        private ITransferAgencyService investorService;
        private Fund _fund;
        private readonly ValidationErrors _validationErrors;
        private IStaticReferences _staticReferences;

        public InvestorActionViewModel(ITransferAgencyService investorService, IStaticReferences staticReferences, Fund fund)
        {
            // if _isNavFinal. disable the price and quantity box, which means amount is entered manually
            this.investorService = investorService;
            this._fund = fund;
            _staticReferences = staticReferences;
            _tradeDate = DateExtentions.InitialDate();
            _settleDate = DateExtentions.InitialDate();
            _isNavFinal = false;
            _TAType = cmbIssueType[0]; // this defaults the type to subscription..
            _validationErrors = new ValidationErrors();
            _validationErrors.ErrorsChanged += ChangedErrorsEvents;
            // currency should be the funds base currency
            AddInvestorActionCommand = new AddInvestorActionCommand(this, investorService);
        }

        public bool TargetFundWaterMark
        {
            get
            {
                return _fund.HasHighWaterMark;
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
                Units = _units; // this will trigger the set on units.
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
                if (_TAType=="Subscription" && _units < 0)
                {
                    _validationErrors.AddError(nameof(Units), "The Subscription amount cannot be a negative number");
                }
                else if(_TAType == "Redemption" && _units > 0)
                {
                    _validationErrors.AddError(nameof(Units), "The Redemption amount cannot be a positive number");
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
                // units should be absolute. multiplier should be based on sub/ red type.
                _tradeAmount = Math.Round((Units * Price) - Fee, 2);
                

                return _tradeAmount;
            }
            set
            {
                _tradeAmount = value;
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
                if(_tradeDate.DayOfWeek == DayOfWeek.Saturday || _tradeDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    _validationErrors.AddError(nameof(TradeDate), "Your actions can't be booked on weekends");
                }
                if (_tradeDate < _fund.LaunchDate)
                {
                    // validation not showing at the moment because it is bound to TextBox at the moment
                    _validationErrors.AddError(nameof(TradeDate), "Your actions can't be booked before funds launch date.");
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

        public ICommand AddInvestorActionCommand { get; set; }

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
