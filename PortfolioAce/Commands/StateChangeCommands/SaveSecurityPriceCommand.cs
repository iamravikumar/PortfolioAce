using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.EFCore.Services.PriceServices;
using PortfolioAce.ViewModels;
using System;
using System.Linq;
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
                SecuritiesDIM security = _priceService.GetSecurityInfo(_sysSecurityPricesVM.SelectedSecurity.Symbol, _sysSecurityPricesVM.SelectedSecurity.AssetClass.Name);
                var results = await _priceService.AddDailyPrices(security);
                if (results > 0)
                {
                    MessageBox.Show($"{results} prices have been saved for {_sysSecurityPricesVM.SelectedSecurity.SecurityName}", "Information");
                    await _sysSecurityPricesVM.Load();
                }
                else
                {
                    MessageBox.Show($"No new prices found for {_sysSecurityPricesVM.SelectedSecurity.SecurityName}", "Information");
                }

            }
            catch (ArgumentOutOfRangeException)
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
