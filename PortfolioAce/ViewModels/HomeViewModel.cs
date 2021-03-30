using LiveCharts;
using LiveCharts.Wpf;
using PortfolioAce.Domain.Models.FactTables;
using PortfolioAce.EFCore.Services.FactTableServices;
using PortfolioAce.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PortfolioAce.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {

        private IFactTableService _factTableService;

        public HomeViewModel(IFactTableService factTableService)
        {
            _factTableService = factTableService;
            _dgAllNavPrices = _factTableService.GetAllNAVPrices();
            _currentCard = 0;
            if (dgAllNavPrices.Count > 0)
            {
                _selectedPrice = dgLatestNavPrices[0];
                Load(_selectedPrice.FundId);
            }

            _assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();


            NextCardCommand = new ActionCommand(NextCard);
            PreviousCardCommand = new ActionCommand(PreviousCard);
        }

        public ICommand NextCardCommand { get; }
        public ICommand PreviousCardCommand { get; }

        private List<NAVPriceStoreFACT> _dgAllNavPrices;
        public List<NAVPriceStoreFACT> dgAllNavPrices
        {
            get
            {
                return _dgAllNavPrices;
            }
            set
            {
                _dgAllNavPrices = _factTableService.GetAllNAVPrices();
                OnPropertyChanged(nameof(dgAllNavPrices));
            }
        }

        public List<NAVPriceStoreFACT> dgLatestNavPrices
        {
            get
            {
                return dgAllNavPrices.GroupBy(x => x.FundId).Select(y => y.Last()).ToList();
            }
        }

        private int _currentCard;
        public int CurrentCard
        {
            // there are 3 cards this will iterate over them.
            get
            {
                return _currentCard;
            }
            set
            {
                _currentCard = value;
                OnPropertyChanged(nameof(CurrentCard));
            }
        }

        public HashSet<string> AllFundSymbols
        {
            get
            {
                return dgAllNavPrices.Select(np => np.Fund.Symbol).ToHashSet();
            }
        }

        private NAVPriceStoreFACT _selectedPrice;
        public NAVPriceStoreFACT selectedPrice
        {
            get
            {
                return _selectedPrice;
            }
            set
            {
                _selectedPrice = value;
                Load(_selectedPrice.FundId);
                OnPropertyChanged(nameof(selectedPrice));
                OnPropertyChanged(nameof(NavPriceLineChartXAxis));
            }
        }

        public ChartValues<decimal> NavPriceLineChartYAxis { get; set; }

        private string[] _NavPriceLineChartXAxis;
        public string[] NavPriceLineChartXAxis
        {
            get
            {
                return _NavPriceLineChartXAxis;
            }
            set
            {
                _NavPriceLineChartXAxis = value;
                OnPropertyChanged(nameof(NavPriceLineChartXAxis));
            }
        }

        private string _assemblyVersion;
        public string AssemblyVersion
        {
            get
            {
                return _assemblyVersion;
            }
        }


        public SeriesCollection RowChartData { get; set; }

        public string[] RowChartDataLabel { get; set; }
        public Func<double, string> Formatter { get; set; }

        public async Task Load(int fundId)
        {
            if (NavPriceLineChartYAxis != null)
            {
                NavPriceLineChartYAxis.Clear();
                NavPriceLineChartYAxis.AddRange(new ChartValues<decimal>(dgAllNavPrices.Where(np => np.FundId == fundId).Select(np => np.NAVPrice)));
            }
            else
            {
                NavPriceLineChartYAxis = new ChartValues<decimal>(dgAllNavPrices.Where(np => np.FundId == fundId).Select(np => np.NAVPrice));
            }
            _NavPriceLineChartXAxis = dgAllNavPrices.Where(np => np.FundId == fundId).Select(np => np.FinalisedDate.ToString("dd/MM/yyyy")).ToArray();

            ChartValues<decimal> rowChartValues = new ChartValues<decimal>();
            RowChartDataLabel = new string[AllFundSymbols.Count];

            // RowChartData
            int counter = 0;
            foreach (string fundSymbol in AllFundSymbols)
            {
                List<NAVPriceStoreFACT> allFundPrices = dgAllNavPrices.Where(np => np.Fund.Symbol == fundSymbol).OrderBy(np => np.FinalisedDate).ToList();
                decimal startPrice = allFundPrices[0].NAVPrice;
                decimal endPrice = allFundPrices[allFundPrices.Count - 1].NAVPrice;
                decimal performance = (endPrice / startPrice) - 1;
                rowChartValues.Add(performance);
                RowChartDataLabel[counter] = fundSymbol;
                counter += 1;
            }
            RowChartData = new SeriesCollection { new RowSeries { Title = "ITD Performance", Values = rowChartValues, DataLabels = true } };
            Formatter = value => value.ToString("P2");
        }

        public void NextCard()
        {
            CurrentCard += 1;
            if (CurrentCard > 2)
            {
                CurrentCard = 0;
            }
        }
        public void PreviousCard()
        {
            CurrentCard -= 1;
            if (CurrentCard < 0)
            {
                CurrentCard = 2;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
