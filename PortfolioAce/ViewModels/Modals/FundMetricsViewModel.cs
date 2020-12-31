using LiveCharts;
using LiveCharts.Wpf;
using PortfolioAce.Domain.DataObjects;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.FactTables;
using PortfolioAce.EFCore.Services.DimensionServices;
using PortfolioAce.EFCore.Services.FactTableServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.ViewModels.Modals
{
    public class FundMetricsViewModel: ViewModelWindowBase
    {
        private IFactTableService _factTableService;
        private IStaticReferences _staticReferences;
        private Fund _fund;
        private DateTime _asOfDate;
        public FundMetricsViewModel(IFactTableService factTableService, IStaticReferences staticReferences, Fund fund, DateTime asOfDate)
        {
            _factTableService = factTableService;
            _staticReferences = staticReferences;
            _fund = fund;
            _asOfDate = asOfDate;
            Load();
        }

        public string FundName
        {
            get
            {
                return _fund.FundName;
            }
        }

        public string InceptionToDatePerformance
        {
            get
            {
                List<NAVPriceStoreFACT> orderedPrices = _fund.NavPrices.Where(np=>np.FinalisedDate<=asOfDate).OrderBy(np => np.FinalisedDate).ToList();
                decimal startPrice = orderedPrices[0].NAVPrice;
                decimal endPrice = orderedPrices[orderedPrices.Count - 1].NAVPrice;
                decimal performance = (endPrice / startPrice) - 1;
                return String.Format("{0:P2}", performance);
            }
        }

        public string MonthToDatePerformance
        {
            get
            {
                List<NAVPriceStoreFACT> orderedPrices = _fund.NavPrices.Where(np=>np.FinalisedDate.Month==asOfDate.Month && np.FinalisedDate<=asOfDate).OrderBy(np => np.FinalisedDate).ToList();
                decimal startPrice = orderedPrices[0].NAVPrice;
                decimal endPrice = orderedPrices[orderedPrices.Count - 1].NAVPrice;
                decimal performance = (endPrice / startPrice) - 1;
                return String.Format("{0:P2}", performance);
            }
        }

        public DateTime asOfDate
        {
            get
            {
                return _asOfDate;
            }
        }

        public int PositionCount { get; set; }


        public ChartValues<decimal> NavPriceLineChartYAxis { get; set; }
        public string[] NavPriceLineChartXAxis { get; set; }

        public SeriesCollection PieChartData { get; set; }


        public SeriesCollection RowChartData { get; set; }

        public string[] RowChartDataLabel { get; set; }
        public Func<double, string> Formatter { get; set; }

        public async Task Load()
        {
            // Line Chart
            IEnumerable<NAVPriceStoreFACT> navPrices = _fund.NavPrices.Where(np=>np.FinalisedDate<=_asOfDate).OrderBy(np => np.FinalisedDate);
            NavPriceLineChartYAxis = new ChartValues<decimal>(navPrices.Select(np => np.NAVPrice));
            NavPriceLineChartXAxis = navPrices.Select(np => np.FinalisedDate.ToString("dd/MM/yyyy")).ToArray();

            
            List<PositionFACT> activePositions = _factTableService.GetAllStoredPositions(_asOfDate, _fund.FundId, onlyActive: true);
            PositionCount = activePositions.Count;
            // Pie Chart
            decimal totalMV = activePositions.Sum(ap => ap.MarketValue);
            Dictionary<string, decimal> MarketValByAssetClass = activePositions
                                                                   .GroupBy(ap => ap.AssetClass.Name.ToString())
                                                                   .ToDictionary(g => g.Key, g => Math.Round(g.Sum(v => v.MarketValue) / totalMV, 2));
            PieChartData = new SeriesCollection();
            foreach (KeyValuePair<string, decimal> kvp in MarketValByAssetClass)
            {
                PieChartData.Add(new PieSeries { Title = kvp.Key, Values = new ChartValues<decimal> { kvp.Value }, DataLabels = true });
            };


            // Row Chart Table
            List<PositionFactPerformance> positionPerformances = new List<PositionFactPerformance>();
            foreach(PositionFACT position in activePositions)
            {
                PositionFactPerformance performance = new PositionFactPerformance(position);
                positionPerformances.Add(performance);
            }

            IEnumerable<PositionFactPerformance> positionPerformancesTopFivePercent = positionPerformances.OrderByDescending(pp => pp.GainPercent).Take(5);

            ChartValues<decimal> rowChartValues = new ChartValues<decimal>(positionPerformancesTopFivePercent.Select(pp => pp.GainPercent));
            RowChartDataLabel = positionPerformancesTopFivePercent.Select(pp => pp.Position.Security.SecurityName).ToArray();
            RowChartData = new SeriesCollection { new RowSeries { Title = "Market Value", Values = rowChartValues, DataLabels=true} };
            Formatter = value => value.ToString("P2");
        }
    }
}
