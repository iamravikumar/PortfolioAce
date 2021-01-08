using PortfolioAce.Commands;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.EFCore.Services;
using PortfolioAce.EFCore.Services.DimensionServices;
using PortfolioAce.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PortfolioAce.ViewModels.Modals
{
    public class SecurityManagerWindowViewModel : ViewModelWindowBase, INotifyDataErrorInfo
    {
        private IAdminService _adminService;
        private readonly ValidationErrors _validationErrors;
        private IStaticReferences _staticReferences;

        public SecurityManagerWindowViewModel(IAdminService adminService, IStaticReferences staticReferences)
        {
            _adminService = adminService;
            _staticReferences = staticReferences;
            AddSecurityCommand = new AddSecurityCommand(this, adminService, staticReferences);
            _validationErrors = new ValidationErrors();
            _validationErrors.ErrorsChanged += ChangedErrorsEvents;
        }

        private string _assetClass;
        public string AssetClass
        {
            get
            {
                return _assetClass;
            }
            set
            {
                _assetClass = value;
                OnPropertyChanged(nameof(AssetClass));
            }
        }

        private string _securityName;
        public string SecurityName
        {
            get
            {
                return _securityName;
            }
            set
            {
                _securityName = value;
                OnPropertyChanged(nameof(SecurityName));
            }
        }

        private string _securitySymbol;
        public string SecuritySymbol
        {
            get
            {
                return _securitySymbol;
            }
            set
            {
                _securitySymbol = value;
                _validationErrors.ClearErrors(nameof(SecuritySymbol));
                if (_adminService.SecurityExists(_securitySymbol))
                {
                    _validationErrors.AddError(nameof(SecuritySymbol), $"The Security '{_securitySymbol}' already exist");
                }
                OnPropertyChanged(nameof(SecuritySymbol));
            }
        }

        private string _currency;
        public string Currency
        {
            get
            {
                return _currency;
            }
            set
            {
                _currency = value;
                OnPropertyChanged(nameof(Currency));
            }
        }

        private string _ISIN;
        public string ISIN
        {
            get
            {
                return _ISIN;
            }
            set
            {
                _ISIN = value;
                OnPropertyChanged(nameof(ISIN));
            }
        }


        public ObservableCollection<SecuritiesDIM> dgSecurities
        {
            get
            {
                List<SecuritiesDIM> securities = _adminService.GetAllSecurities();
                return new ObservableCollection<SecuritiesDIM>(securities);
            }
        }

        public List<string> cmbAssetClass
        {
            get
            {
                return _staticReferences.GetAllAssetClasses().Select(ac => ac.Name).ToList();
            }
        }

        public List<string> cmbCurrency
        {
            get
            {
                return _staticReferences.GetAllCurrencies().Select(c => c.Symbol).ToList();
            }
        }

        public ICommand AddSecurityCommand { get; set; }
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
