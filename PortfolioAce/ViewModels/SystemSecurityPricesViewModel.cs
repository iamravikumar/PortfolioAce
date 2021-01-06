using LiveCharts;
using PortfolioAce.Commands;
using PortfolioAce.DataCentre.DeserialisedObjects;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.EFCore.Services.PriceServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            SecurityPriceLineChartYAxis = new ChartValues<decimal> { 1,1,1,1,1};
            SecurityPriceLineChartXAxis = new string[1];
        }

        public ICommand SaveSecurityPriceCommand { get; set; }


        public ObservableCollection<string> cmbSecurities
        {
            get
            {
                HashSet<string> pricedSecuritySymbols = _priceService.GetAllSecuritySymbols();
                return new ObservableCollection<string>(pricedSecuritySymbols);
            }
        }

        public ObservableCollection<SecurityPriceStore> dgSecurityPrices
        {
            get
            {
                List<SecurityPriceStore> securityPrices = _priceService.GetSecurityPrices(_symbol);
                return new ObservableCollection<SecurityPriceStore>(securityPrices);
            }
        }

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
                OnPropertyChanged(nameof(SecurityInfo));
                OnPropertyChanged(nameof(dgSecurityPrices));
                Load();
                OnPropertyChanged(nameof(SecurityPriceLineChartXAxis));
                OnPropertyChanged(nameof(ShowAPIButton));
            }
        }

        public Visibility ShowAPIButton
        {
            get
            {
                return (_symbol != null) ? Visibility.Visible : Visibility.Collapsed;
            }
            private set
            {
            }
        }

        public SecuritiesDIM SecurityInfo
        {
            get
            {
                return _priceService.GetSecurityInfo(_symbol);
            }
        }

        private string[] _SecurityPriceLineChartXAxis;
        public string[] SecurityPriceLineChartXAxis
        {
            get
            {
                return _SecurityPriceLineChartXAxis;
            }
            set
            {
                _SecurityPriceLineChartXAxis = value;
                OnPropertyChanged(nameof(SecurityPriceLineChartXAxis));
            }
        }

        public ChartValues<decimal> SecurityPriceLineChartYAxis { get; set; }


        public async Task Load()
        {
            SecurityPriceLineChartYAxis.Clear();
            SecurityPriceLineChartYAxis.AddRange(new ChartValues<decimal>(dgSecurityPrices.Select(sp=>sp.ClosePrice)));
            _SecurityPriceLineChartXAxis = dgSecurityPrices.Select(sp => sp.Date.ToString("dd/MM/yyyy")).ToArray();
        }


    }
}
