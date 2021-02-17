using LiveCharts;
using PortfolioAce.Commands;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.EFCore.Services.DimensionServices;
using PortfolioAce.EFCore.Services.PriceServices;
using PortfolioAce.HelperObjects;
using PortfolioAce.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.ViewModels
{
    public class SystemSecurityPricesViewModel : ViewModelBase
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
            SecurityPriceLineChartYAxis = new ChartValues<decimal>();
            SecurityPriceLineChartXAxis = new string[1];

            _cmbAssetClasses = _staticReferences.GetAllAssetClasses().Where(ac => ac.Name != "Cash" && ac.Name != "FXForward").Select(ac => ac.Name).ToList();

            _allSecuritiesList = _staticReferences.GetAllSecurities(includeRates: true);
            if (_cmbAssetClasses.Count != 0)
            {
                AssetClass = _cmbAssetClasses[0];
                _securitiesList = _allSecuritiesList.Where(s=>s.AssetClass.Name==AssetClass).ToList();
                if (_securitiesList.Count != 0)
                {
                    _SelectedSecurity = _securitiesList[0];
                    Load();
                }
            }
        }

        public ICommand SaveSecurityPriceCommand { get; set; }
        public ICommand SaveManualSecurityPriceCommand { get; set; }
        public ICommand AssetSelectionChangedCommand { get; set; }



        private readonly List<SecuritiesDIM> _allSecuritiesList;

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

        private List<SecuritiesDIM> _securitiesList;
        public List<SecuritiesDIM> SecuritiesList
        {
            get
            {
                if (_assetClass == null)
                {
                   return _securitiesList;
                }
                else
                {
                    return _allSecuritiesList.Where(s => s.AssetClass.Name == _assetClass).ToList();
                }
            }
        }


        private SecuritiesDIM _SelectedSecurity;
        public SecuritiesDIM SelectedSecurity
        {
            get
            {
                return _SelectedSecurity;
            }
            set
            {
                _SelectedSecurity = value;
                if(_SelectedSecurity != null)
                {
                    Load();
                    OnPropertyChanged("");
                }
                else
                {
                    OnPropertyChanged(nameof(SelectedSecurity));
                }
            }
        }



        public Visibility ShowAPIButton
        {
            get
            {
                return (_SelectedSecurity != null) ? Visibility.Visible : Visibility.Collapsed;
            }
            private set
            {
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
            _securityPrices = _priceService.GetSecurityPrices(_SelectedSecurity.Symbol);
            dgSecurityPrices = new ObservableCollection<PriceContainer>(_securityPrices.Select(sp => new PriceContainer(sp.Date, sp.ClosePrice)));
            SecurityPriceLineChartYAxis.AddRange(new ChartValues<decimal>(dgSecurityPrices.Select(sp => sp.ClosePrice)));
            _SecurityPriceLineChartXAxis = dgSecurityPrices.Select(sp => sp.Date.ToString("dd/MM/yyyy")).ToArray();
        }



        public void ChangeAssetClassCommand()
        {
            _securitiesList = _allSecuritiesList.Where(s => s.AssetClass.Name == AssetClass).ToList();
            if (_securitiesList.Count > 0)
            {
                _SelectedSecurity = _securitiesList[0];
                OnPropertyChanged(nameof(SelectedSecurity));
                Load();
            }
            OnPropertyChanged(nameof(SecuritiesList));
        }

        public override void Dispose()
        {
            base.Dispose();
        }

    }
}
