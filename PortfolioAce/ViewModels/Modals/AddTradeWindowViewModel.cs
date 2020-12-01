using PortfolioAce.Commands;
using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore.Services;
using PortfolioAce.EFCore.Services.DimensionServices;
using PortfolioAce.Models;
using PortfolioAce.Navigation;
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
    public class AddTradeWindowViewModel: ViewModelWindowBase, INotifyDataErrorInfo
    {
        private Fund _fund;
        private ITransactionService _transactionService;
        private IStaticReferences _staticReferences;

        private readonly ValidationErrors _validationErrors;
        public AddTradeWindowViewModel(ITransactionService transactionService, IStaticReferences staticReferences, Fund fund)
        {
            AddTradeCommand = new AddTradeCommand(this, transactionService);
            _transactionService = transactionService;
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

        private string _symbol;
        public string Symbol
        {
            get
            {
                return _symbol;
            }
            set
            {
                _symbol = value;
                _validationErrors.ClearErrors(nameof(Symbol));
                if (!_transactionService.SecurityExists(_symbol))
                {
                    _validationErrors.AddError(nameof(Symbol), $"The Security '{_symbol}' does not exist");
                }
                else
                {
                    string assetClass = _transactionService.GetSecurityInfo(Symbol).AssetClass.Name.ToString();
                    if(assetClass == "Cash")
                    {
                        _validationErrors.AddError(nameof(Symbol), $"Cash purchases/sales not yet supported");
                    }
                }
                // I can check the Database for the value,
                // and if it exists prefill the currency field
                // if not raise exception
                OnPropertyChanged(nameof(Symbol));
                OnPropertyChanged(nameof(TradeCurrency));
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
                _price= value;
                _validationErrors.ClearErrors(nameof(Price));
                if(_price < 0)
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
                if(TradeType == "Corporate Action")
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
                //this should be based on the security symbol
                if (Symbol == null)
                {
                    return null;
                }
                else
                {
                    var res = _transactionService.GetSecurityInfo(Symbol);
                    return (res != null) ? res.Currency.Symbol.ToString() : null;
                }
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
                return _staticReferences.GetAllTransactionTypes().Where(t => t.TypeClass.ToString() == "SecurityTrade").Select(t => t.TypeName.ToString()).ToList();
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
