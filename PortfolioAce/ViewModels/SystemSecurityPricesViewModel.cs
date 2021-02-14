using LiveCharts;
using PortfolioAce.Commands;
using PortfolioAce.DataCentre.DeserialisedObjects;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.EFCore.Services.DimensionServices;
using PortfolioAce.EFCore.Services.PriceServices;
using PortfolioAce.HelperObjects;
using PortfolioAce.Navigation;
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
            AssetSelectionChangedCommand = new ActionCommand(ChangeAssetClassCommand);
            SecuritySelectionChangedCommand = new ActionCommand(ChangeSecurityCommand);
            SecurityPriceLineChartYAxis = new ChartValues<decimal>();
            SecurityPriceLineChartXAxis = new string[1];

            _cmbAssetClasses = _staticReferences.GetAllAssetClasses().Where(ac => ac.Name != "Cash" && ac.Name != "FXForward").Select(ac => ac.Name).ToList();

            // Todo: refactor: sets the initial security..

            if (_cmbAssetClasses.Count != 0)
            {
                AssetClass = _cmbAssetClasses[0];
                _cmbSecurities = _staticReferences.GetSecuritySymbolByAssetClass(_assetClass);
                if(_cmbSecurities.Count != 0)
                {
                    Symbol = _cmbSecurities[0];
                    Load();
                }
            }
        }

        public ICommand SaveSecurityPriceCommand { get; set; }
        public ICommand SaveManualSecurityPriceCommand { get; set; }
        public ICommand AssetSelectionChangedCommand { get; set; }
        public ICommand SecuritySelectionChangedCommand { get; set; }

        private List<string> _cmbAssetClasses;
        public List<string> cmbAssetClasses
        {
            get
            {
                return _cmbAssetClasses;
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
                OnPropertyChanged(nameof(AssetClass));
            }
        }

        private List<string> _cmbSecurities;
        public ObservableCollection<string> cmbSecurities
        {
            get
            {
                return new ObservableCollection<string>(_cmbSecurities);
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

        private SecuritiesDIM _securityInfo;
        public SecuritiesDIM SecurityInfo
        {
            get
            {
                return _securityInfo;
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

        private List<SecurityPriceStore> _securityPrices;
        public ObservableCollection<PriceContainer> dgSecurityPrices { get; set; }


        public async Task Load()
        {
            SecurityPriceLineChartYAxis.Clear();
            _securityPrices = _priceService.GetSecurityPrices(_symbol);
            _securityInfo = _priceService.GetSecurityInfo(_symbol);
            dgSecurityPrices = new ObservableCollection<PriceContainer>(_securityPrices.Select(sp => new PriceContainer(sp.Date, sp.ClosePrice)));
            SecurityPriceLineChartYAxis.AddRange(new ChartValues<decimal>(dgSecurityPrices.Select(sp=>sp.ClosePrice)));
            _SecurityPriceLineChartXAxis = dgSecurityPrices.Select(sp => sp.Date.ToString("dd/MM/yyyy")).ToArray();
        }

        public void ChangeSecurityCommand()
        {
            Load();
            OnPropertyChanged("");
        }
        

        public void ChangeAssetClassCommand()
        {
            _cmbSecurities = _staticReferences.GetSecuritySymbolByAssetClass(_assetClass);
            if (_cmbSecurities.Count > 0)
            {
                Symbol = _cmbSecurities[0];
                OnPropertyChanged(nameof(Symbol));
            }
            OnPropertyChanged(nameof(cmbSecurities));
        }

        public override void Dispose()
        {
            base.Dispose();
        }

    }
}
