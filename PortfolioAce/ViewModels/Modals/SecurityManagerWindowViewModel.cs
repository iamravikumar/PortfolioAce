using PortfolioAce.Commands;
using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PortfolioAce.ViewModels.Modals
{
    public class SecurityManagerWindowViewModel : ViewModelBase
    {
        private IAdminService _adminService;

        public SecurityManagerWindowViewModel(IAdminService adminService)
        {
            _adminService = adminService;
            AddSecurityCommand = new AddSecurityCommand(this, adminService);
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

        private List<Security> _dgSecurities;
        public List<Security> dgSecurities
        {
            get
            {
                return _adminService.GetAllSecurities();
            }
            set
            {
                _dgSecurities = _adminService.GetAllSecurities();
                OnPropertyChanged(nameof(dgSecurities));
            }
        }

        public ICommand AddSecurityCommand { get; set; }
    }
}
