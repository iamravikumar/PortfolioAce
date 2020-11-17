using PortfolioAce.Commands;
using PortfolioAce.EFCore.Services;
using PortfolioAce.Models;
using PortfolioAce.ViewModels.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace PortfolioAce.ViewModels.Modals
{

    public class AddFundWindowViewModel:ViewModelWindowBase, INotifyDataErrorInfo
    {
        private IFundService _fundService;
        private readonly ValidationErrors _validationErrors;

        public AddFundWindowViewModel(IFundService fundService)
        {
            AddFundCommand = new AddFundCommand(this, fundService);
            _fundService = fundService;
            _fundLaunch = DateTime.Today;
            _validationErrors = new ValidationErrors();
            _validationErrors.ErrorsChanged += ChangedErrorsEvents;
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
                if(_fundPerfFee<0 || _fundPerfFee > 1)
                {
                    _validationErrors.AddError(nameof(FundPerfFee), "The performance fee must be between 0% to 100%");
                }
                OnPropertyChanged(nameof(FundPerfFee));
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
                OnPropertyChanged(nameof(FundLaunch));
            }
        }

        public ICommand AddFundCommand { get; set; }

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
