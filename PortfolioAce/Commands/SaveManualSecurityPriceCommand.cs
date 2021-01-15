using PortfolioAce.EFCore.Services.PriceServices;
using PortfolioAce.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class SaveManualSecurityPriceCommand:ICommand
    {
        public event EventHandler CanExecuteChanged;
        private SystemSecurityPricesViewModel _sysSecurityPricesVM;
        private IPriceService _priceService;

        public SaveManualSecurityPriceCommand(
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

            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }
    }
}
