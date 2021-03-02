using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore.Services.PriceServices;
using PortfolioAce.HelperObjects;
using PortfolioAce.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class SaveManualSecurityPriceCommand : ICommand
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
                Dictionary<DateTime, SecurityPriceStore> dbPrices = _priceService.GetSecurityPrices(_sysSecurityPricesVM.SelectedSecurity.Symbol)
                                                                            .ToDictionary(g => g.Date);
                int secId = _sysSecurityPricesVM.SelectedSecurity.SecurityId;
                List<SecurityPriceStore> pricesToUpdate = new List<SecurityPriceStore>();
                List<SecurityPriceStore> newPrices = new List<SecurityPriceStore>();
                foreach (PriceContainer priceContainer in _sysSecurityPricesVM.dgSecurityPrices)
                {
                    if (dbPrices.ContainsKey(priceContainer.Date))
                    {
                        if (dbPrices[priceContainer.Date].ClosePrice != priceContainer.ClosePrice)
                        {
                            dbPrices[priceContainer.Date].ClosePrice = priceContainer.ClosePrice;
                            dbPrices[priceContainer.Date].PriceSource = "Manual";
                            pricesToUpdate.Add(dbPrices[priceContainer.Date]);
                        }
                    }
                    else
                    {
                        SecurityPriceStore newManualPrice = new SecurityPriceStore
                        {
                            Date = priceContainer.Date,
                            ClosePrice = priceContainer.ClosePrice,
                            PriceSource = "Manual",
                            SecurityId = secId
                        };
                        newPrices.Add(newManualPrice);
                    }
                }
                _priceService.AddManualPrices(newPrices);
                _priceService.UpdateManualPrices(pricesToUpdate);
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }
    }
}
