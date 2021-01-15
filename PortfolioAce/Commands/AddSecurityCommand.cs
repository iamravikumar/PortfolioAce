using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.EFCore.Services;
using PortfolioAce.EFCore.Services.DimensionServices;
using PortfolioAce.ViewModels.Modals;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class AddSecurityCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private SecurityManagerWindowViewModel _SecurityManagerVM;
        private IAdminService _adminService;
        private IStaticReferences _staticReferences;

        public AddSecurityCommand(SecurityManagerWindowViewModel securityManagerVM, 
            IAdminService adminService, IStaticReferences staticReferences)
        {
            _SecurityManagerVM = securityManagerVM;
            _adminService = adminService;
            _staticReferences = staticReferences;
        }

        public bool CanExecute(object parameter)
        {
            return true; // for now
        }

        public void Execute(object parameter)
        {
            // currency and asset class objects
            CurrenciesDIM currency = _staticReferences.GetCurrency(_SecurityManagerVM.Currency);
            AssetClassDIM assetClass = _staticReferences.GetAssetClass(_SecurityManagerVM.AssetClass);
            try
            {
                SecuritiesDIM newSecurity = new SecuritiesDIM
                {
                    AssetClassId = assetClass.AssetClassId,
                    Symbol = _SecurityManagerVM.SecuritySymbol,
                    CurrencyId = currency.CurrencyId,
                    SecurityName = _SecurityManagerVM.SecurityName,
                    AlphaVantageSymbol=_SecurityManagerVM.AVSymbol,
                    FMPSymbol=_SecurityManagerVM.FMPSymbol,
                    ISIN = _SecurityManagerVM.ISIN
                };
                _adminService.AddSecurityInfo(newSecurity);
                return;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
