using PortfolioAce.Commands;
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

    public class AddFundWindowViewModel : ViewModelWindowBase, INotifyDataErrorInfo
    {
        private IFundService _fundService;
        private readonly ValidationErrors _validationErrors;
        private IStaticReferences _staticReferences;

        public AddFundWindowViewModel(IFundService fundService, IStaticReferences staticReferences)
        {
            AddFundCommand = new AddFundCommand(this, fundService);
            _fundService = fundService;
            _fundLaunch = DateExtentions.InitialDate();
            _validationErrors = new ValidationErrors();
            _validationErrors.ErrorsChanged += ChangedErrorsEvents;
            _staticReferences = staticReferences;
            _MinimumInvestment = decimal.One;
            _selectedHurdleType = "None";
            _HighWaterMark = false;
            // to set decimal points i might need to use a converter
        }

        private string _fundName;
        public string FundName
        {
            get
            {
                return _fundName;
            }
            set
            {
                _fundName = value;
                OnPropertyChanged(nameof(FundName));
            }
        }

        private string _fundSymbol;
        public string FundSymbol
        {
            get
            {
                return _fundSymbol;
            }
            set
            {
                _fundSymbol = value;
                _validationErrors.ClearErrors(nameof(FundSymbol));
                if (_fundService.FundExists(_fundSymbol))
                {
                    _validationErrors.AddError(nameof(FundSymbol), $"The Symbol '{_fundSymbol}' already exists");
                }
                if (_fundSymbol.Length > 8)
                {
                    _validationErrors.AddError(nameof(FundSymbol), $"The Symbol cannot contain more 8 characters");
                }
                OnPropertyChanged(nameof(FundSymbol));
            }
        }

        private string _fundCurrency;
        public string FundCurrency
        {
            get
            {
                return _fundCurrency;
            }
            set
            {
                _fundCurrency = value;
                OnPropertyChanged(nameof(FundCurrency));
            }
        }

        public bool _HighWaterMark;
        public bool HighWaterMark
        {
            get
            {
                return _HighWaterMark;
            }
            set
            {
                _HighWaterMark = value;
                OnPropertyChanged(nameof(HighWaterMark));
            }
        }

        private decimal _fundManFee;
        public decimal FundManFee
        {
            get
            {
                return _fundManFee;
            }
            set
            {
                _fundManFee = value;
                _validationErrors.ClearErrors(nameof(FundManFee));
                if (_fundManFee < 0 || _fundManFee > 1)
                {
                    _validationErrors.AddError(nameof(FundManFee), "The management fee must be between 0% to 100%");
                }
                OnPropertyChanged(nameof(FundManFee));
                OnPropertyChanged(nameof(percentageFundManFee));
            }
        }

        private decimal _fundPerfFee;
        public decimal FundPerfFee
        {
            get
            {
                return _fundPerfFee;
            }
            set
            {
                _fundPerfFee = value;
                _validationErrors.ClearErrors(nameof(FundPerfFee));
                if (_fundPerfFee < 0 || _fundPerfFee > 1)
                {
                    _validationErrors.AddError(nameof(FundPerfFee), "The performance fee must be between 0% to 100%");
                }
                OnPropertyChanged(nameof(FundPerfFee));
                OnPropertyChanged(nameof(percentageFundPerfFee));
            }
        }

        private decimal _MinimumInvestment;
        public decimal MinimumInvestment
        {
            get
            {
                return _MinimumInvestment;
            }
            set
            {
                _MinimumInvestment = value;
                _validationErrors.ClearErrors(nameof(MinimumInvestment));
                if (_MinimumInvestment <= 0)
                {
                    _validationErrors.AddError(nameof(MinimumInvestment), "The Minimum investment must be greater than zero");
                }
                OnPropertyChanged(nameof(MinimumInvestment));
            }
        }

        private string _fundNavFreq;
        public string FundNavFreq
        {
            get
            {
                return _fundNavFreq;
            }
            set
            {
                _fundNavFreq = value;
                OnPropertyChanged(nameof(FundNavFreq));
            }
        }

        private string _selectedHurdleType;
        public string selectedHurdleType
        {
            get
            {
                return _selectedHurdleType;
            }
            set
            {
                _selectedHurdleType = value;
                if (_selectedHurdleType == "None")
                {
                    _hurdleRate = decimal.Zero;
                }
                OnPropertyChanged(nameof(selectedHurdleType));
                OnPropertyChanged(nameof(HurdleRate));
                OnPropertyChanged(nameof(EnableHurdleRate));
                OnPropertyChanged(nameof(percentageFundHurdleRate));
            }
        }

        public bool EnableHurdleRate
        {
            get
            {
                return (_selectedHurdleType != "None");
            }
        }

        private decimal _hurdleRate;
        public decimal HurdleRate
        {
            get
            {
                return _hurdleRate;
            }
            set
            {
                _hurdleRate = value;
                _validationErrors.ClearErrors(nameof(HurdleRate));
                if (_hurdleRate < 0 || _hurdleRate > 1)
                {
                    _validationErrors.AddError(nameof(HurdleRate), "The Hurdle Rate must be between 0% to 100%");
                }
                OnPropertyChanged(nameof(HurdleRate));
                OnPropertyChanged(nameof(percentageFundHurdleRate));
            }
        }

        private DateTime _fundLaunch;
        public DateTime FundLaunch
        {
            get
            {
                return _fundLaunch;
            }
            set
            {
                _fundLaunch = value;
                _validationErrors.ClearErrors(nameof(FundLaunch));
                if (_fundLaunch.DayOfWeek == DayOfWeek.Saturday || _fundLaunch.DayOfWeek == DayOfWeek.Sunday)
                {
                    _validationErrors.AddError(nameof(FundLaunch), "Your fund can't launch on a weekend");
                }
                OnPropertyChanged(nameof(FundLaunch));
            }
        }

        public string percentageFundManFee
        {
            get
            {
                return String.Format("{0:P2}", _fundManFee);
            }
        }

        public string percentageFundPerfFee
        {
            get
            {
                return String.Format("{0:P2}", _fundPerfFee);
            }
        }

        public string percentageFundHurdleRate
        {
            get
            {
                return String.Format("{0:P2}", _hurdleRate);
            }
        }


        public List<string> cmbNavFreq
        {
            get
            {
                return _staticReferences.GetAllNavFrequencies().Select(nf => nf.Frequency).ToList();
            }
        }

        public List<string> cmbCurrency
        {
            get
            {
                return _staticReferences.GetAllCurrencies().Select(c => c.Symbol).ToList();
            }
        }

        public ICommand AddFundCommand { get; }

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
    public enum HurdleType
    {
        Hard,
        Soft,
        None
    }
}
