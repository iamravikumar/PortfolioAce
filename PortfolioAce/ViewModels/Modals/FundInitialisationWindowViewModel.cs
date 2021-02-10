using PortfolioAce.Commands;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.EFCore.Services;
using PortfolioAce.EFCore.Services.DimensionServices;
using PortfolioAce.HelperObjects;
using PortfolioAce.Models;
using PortfolioAce.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.ViewModels.Modals
{
    public class FundInitialisationWindowViewModel : ViewModelWindowBase, INotifyDataErrorInfo
    {

        private ITransferAgencyService investorService;
        private Fund _fund;
        private readonly ValidationErrors _validationErrors;
        private IStaticReferences _staticReferences;

        public FundInitialisationWindowViewModel(ITransferAgencyService investorService, IStaticReferences staticReferences, Fund fund)
        {
            this.investorService = investorService;
            this._fund = fund;
            _staticReferences = staticReferences;
            _validationErrors = new ValidationErrors();
            _validationErrors.ErrorsChanged += ChangedErrorsEvents;
            dgSeedingInvestors = new ObservableCollection<SeedingInvestor>();
            InitialiseFundCommand = new InitialiseFundCommand(this, investorService, staticReferences);
            _NavPrice = 1;
            _custodian = cmbCustodians[0];
        }
        public ICommand InitialiseFundCommand { get; set; }

        public ObservableCollection<SeedingInvestor> dgSeedingInvestors { get; set; }

        public Fund TargetFund
        {
            get
            {
                return _fund;
            }
            private set
            {
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

        public List<string> cmbCustodians
        {
            get
            {
                return _staticReferences.GetAllCustodians().Select(c => c.Name).ToList();
            }
        }

        public List<InvestorsDIM> cmbInvestors
        {
            get
            {
                return investorService.GetAllInvestors();
            }
        }

        private decimal _NavPrice;
        public decimal NavPrice
        {
            get
            {
                return _NavPrice;
            }
            set
            {
                _NavPrice = value;
                _validationErrors.ClearErrors(nameof(NavPrice));
                if (_NavPrice < 1)
                {
                    _validationErrors.AddError(nameof(NavPrice), "The price cannot be less than 1");
                }

                OnPropertyChanged(nameof(NavPrice));
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
