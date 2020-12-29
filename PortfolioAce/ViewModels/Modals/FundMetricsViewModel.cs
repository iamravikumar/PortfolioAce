using LiveCharts;
using LiveCharts.Wpf;
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

        public ChartValues<decimal> NavPriceLineChartYAxis { get; set; }
        public string[] NavPriceLineChartXAxis { get; set; }

        public SeriesCollection PieChartData { get; set; }




        public async Task Load()
        {
            IEnumerable<NAVPriceStoreFACT> navPrices = _fund.NavPrices.Where(np=>np.FinalisedDate<=_asOfDate).OrderBy(np => np.FinalisedDate);
            NavPriceLineChartYAxis = new ChartValues<decimal>(navPrices.Select(np => np.NAVPrice));
            NavPriceLineChartXAxis = navPrices.Select(np => np.FinalisedDate.ToString("dd/MM/yyyy")).ToArray();

            // TODO Make A DataObject that can handle this. Building the PieChart Data
            List<PositionFACT> activePositions = _factTableService.GetAllStoredPositions(_asOfDate, _fund.FundId, onlyActive: true);

            

            decimal totalMV = activePositions.Sum(ap => ap.MarketValue);
            Dictionary<string, decimal> MarketValByAssetClass = activePositions
                                                                   .GroupBy(ap=>ap.AssetClass.Name.ToString())
                                                                   .ToDictionary(g=>g.Key, g=>Math.Round(g.Sum(v=>v.MarketValue)/totalMV,2));
            PieChartData = new SeriesCollection();
            foreach(KeyValuePair<string, decimal> kvp in MarketValByAssetClass)
            {
                PieChartData.Add(new PieSeries { Title = kvp.Key, Values = new ChartValues<decimal> { kvp.Value }, DataLabels = true });
            };

        }
    }
}
