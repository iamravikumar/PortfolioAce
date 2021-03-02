using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.EFCore.Services.PriceServices;
using PortfolioAce.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class SaveSecurityPriceCommand : AsyncCommandBase
    {
        private SystemSecurityPricesViewModel _sysSecurityPricesVM;
        private IPriceService _priceService;

        public SaveSecurityPriceCommand(
            SystemSecurityPricesViewModel sysSecurityPricesVM,
            IPriceService priceService)
        {
            _sysSecurityPricesVM = sysSecurityPricesVM;
            _priceService = priceService;
        }

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                SecuritiesDIM security = _priceService.GetSecurityInfo(_sysSecurityPricesVM.SelectedSecurity.Symbol);
                await _priceService.AddDailyPrices(security);
            }
            catch (ArgumentOutOfRangeException argError)
            {
                MessageBox.Show("Your API Key might not be valid. Please investigate", "Error");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
