using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.EFCore.Services.PriceServices;
using PortfolioAce.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class SaveSecurityPriceCommand:ICommand
    {
        public event EventHandler CanExecuteChanged;
        private SystemSecurityPricesViewModel _sysSecurityPricesVM;
        private IPriceService _priceService;

        public SaveSecurityPriceCommand(
            SystemSecurityPricesViewModel sysSecurityPricesVM,
            IPriceService priceService)
        {
            _sysSecurityPricesVM = sysSecurityPricesVM;
            _priceService = priceService;
        }

        public bool CanExecute(object parameter)
        {
            return true; // true for now
        }

        public async void Execute(object parameter)
        {
            try
            {
                SecuritiesDIM security = _priceService.GetSecurityInfo(_sysSecurityPricesVM.Symbol);
                await _priceService.AddDailyPrices(security);
            }
            catch(ArgumentOutOfRangeException argError)
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
