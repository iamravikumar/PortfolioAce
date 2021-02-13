using LiveCharts;
using PortfolioAce.Commands;
using PortfolioAce.DataCentre.DeserialisedObjects;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.EFCore.Services.DimensionServices;
using PortfolioAce.EFCore.Services.PriceServices;
using PortfolioAce.HelperObjects;
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
        private IStaticReferences _staticReferences;
        public SystemSecurityPricesViewModel(IPriceService priceService, IStaticReferences staticReferences)
        {
            _priceService = priceService;
            _staticReferences = staticReferences;

            SaveSecurityPriceCommand = new SaveSecurityPriceCommand(this, priceService);
            SaveManualSecurityPriceCommand = new SaveManualSecurityPriceCommand(this, priceService);

            // Todo: refactor: sets the initial security..
            if (cmbAssetClasses.Count != 0)
            {
                _assetClass = cmbAssetClasses[0];
                if (cmbSecurities.Count != 0)
                {
                    _symbol = cmbSecurities[0];
                    _securityPrices = _priceService.GetSecurityPrices(_symbol);
                    dgSecurityPrices = new ObservableCollection<PriceContainer>(_securityPrices.Select(sp => new PriceContainer(sp.Date, sp.ClosePrice)));
                    SecurityPriceLineChartYAxis = new ChartValues<decimal>(dgSecurityPrices.Select(sp => sp.ClosePrice));
                    _SecurityPriceLineChartXAxis = dgSecurityPrices.Select(sp => sp.Date.ToString("dd/MM/yyyy")).ToArray();
                }
            }

            if (_symbol == null)
            {
                _securityPrices = new List<SecurityPriceStore>();
                dgSecurityPrices = new ObservableCollection<PriceContainer>(_securityPrices.Select(sp => new PriceContainer(sp.Date, sp.ClosePrice)));
                SecurityPriceLineChartYAxis = new ChartValues<decimal> { 1, 1, 1, 1, 1 };
                SecurityPriceLineChartXAxis = new string[1];
            }
        }

        public ICommand SaveSecurityPriceCommand { get; set; }
        public ICommand SaveManualSecurityPriceCommand { get; set; }


        public List<string> cmbAssetClasses
        {
            get
            {
                return _staticReferences.GetAllAssetClasses().Where(ac => ac.Name != "Cash" && ac.Name != "FXForward").Select(ac=>ac.Name).ToList();

            }
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
                OnPropertyChanged(nameof(cmbSecurities));
            }
        }

        public ObservableCollection<string> cmbSecurities
        {
            get
            {
                List<string> pricedSecuritySymbols = _staticReferences.GetSecuritySymbolByAssetClass(_assetClass);
                return new ObservableCollection<string>(pricedSecuritySymbols);
            }
        }

        private List<SecurityPriceStore> _securityPrices;
        public ObservableCollection<PriceContainer> dgSecurityPrices { get; set; }



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
                Load();
                OnPropertyChanged(nameof(dgSecurityPrices));
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
            _securityPrices = _priceService.GetSecurityPrices(_symbol);
            dgSecurityPrices = new ObservableCollection<PriceContainer>(_securityPrices.Select(sp => new PriceContainer(sp.Date, sp.ClosePrice)));
            SecurityPriceLineChartYAxis.AddRange(new ChartValues<decimal>(dgSecurityPrices.Select(sp=>sp.ClosePrice)));
            _SecurityPriceLineChartXAxis = dgSecurityPrices.Select(sp => sp.Date.ToString("dd/MM/yyyy")).ToArray();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

    }
}
