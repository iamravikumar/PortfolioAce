using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.EFCore.Services;
using PortfolioAce.EFCore.Services.DimensionServices;
using PortfolioAce.ViewModels.Modals;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class AddSecurityCommand : AsyncCommandBase
    {

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

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object parameter)
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
                    AlphaVantageSymbol = _SecurityManagerVM.AVSymbol,
                    FMPSymbol = _SecurityManagerVM.FMPSymbol,
                    ISIN = _SecurityManagerVM.ISIN
                };
                await _adminService.AddSecurityInfo(newSecurity);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
