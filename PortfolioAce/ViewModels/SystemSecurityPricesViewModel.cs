using PortfolioAce.Commands;
using PortfolioAce.DataCentre.DeserialisedObjects;
using PortfolioAce.EFCore.Services.PriceServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PortfolioAce.ViewModels
{
    public class SystemSecurityPricesViewModel:ViewModelBase
    {
        private IPriceService _priceService;

        public SystemSecurityPricesViewModel(IPriceService priceService)
        {
            _priceService = priceService;
            SaveSecurityPriceCommand = new SaveSecurityPriceCommand(this, priceService);
        }

        public ICommand SaveSecurityPriceCommand { get; set; }

        private string _symbol;
        public string Symbol
        {
            get
            {
                return _symbol;
            }
            set
            {
                _symbol = value;
                OnPropertyChanged(nameof(Symbol));
            }
        }

        /*
         * I have to make the task async and await the service itself.
        public async Task<List<AVSecurityPriceData>> P()
        {
            var x = await _priceService.GetPrices("IBM");
            foreach(var i in x)
            {
                Debug.WriteLine(i.Close);
            }
            return x;
        }
        */

    }
}
