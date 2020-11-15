using PortfolioAce.Commands;
using PortfolioAce.EFCore.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PortfolioAce.ViewModels.Windows
{

    public class AddFundWindowViewModel:ViewModelWindowBase
    {
        public AddFundWindowViewModel(IFundService fundService)
        {
            AddFundCommand = new AddFundCommand(this, fundService);
            _fundLaunch = DateTime.Today;
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

    }
}
