using LiveCharts;
using PortfolioAce.Domain.DataObjects.PositionData;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.FactTables;
using PortfolioAce.EFCore.Services.FactTableServices;
using PortfolioAce.EFCore.Services.PriceServices;
using System.Collections.Generic;
using System.Linq;

namespace PortfolioAce.ViewModels.Modals
{
    public class PositionDetailWindowViewModel : ViewModelWindowBase
    {

        // I need the actual position, take the static references)
        private IFactTableService _factTableService;
        private IPriceService _priceService;

        public PositionDetailWindowViewModel(IPriceService priceService, IFactTableService factTableService,
           ValuedSecurityPosition valuedPosition, Fund fund)
        {
            _factTableService = factTableService;
            _priceService = priceService;
            _valuedPosition = valuedPosition;
            PositionOpenLots = _valuedPosition.Position.OpenLots.ToList();
            Title = $"{_valuedPosition.Position.Security.SecurityName} ({_valuedPosition.Position.Security.Symbol})";
            FundName = fund.FundName;

            List<PositionFACT> positionHistory = _factTableService.GetAllFundStoredPositions(fund.FundId, valuedPosition.Position.Security.SecurityId);
            PositionPriceLineChartYAxis = new ChartValues<decimal>(positionHistory.Select(ph => ph.RealisedPnl + ph.UnrealisedPnl));
            PositionPriceLineChartXAxis = positionHistory.Select(ph => ph.PositionDate.ToString("dd/MM/yyyy")).ToArray();
        }

        public ChartValues<decimal> PositionPriceLineChartYAxis { get; set; }
        public string[] PositionPriceLineChartXAxis { get; set; }


        private ValuedSecurityPosition _valuedPosition;
        public ValuedSecurityPosition TargetPosition
        {
            get
            {
                return _valuedPosition;
            }
        }
        public List<TaxLotsOpen> PositionOpenLots { get; set; }

        public string Title { get; set; }
        public string FundName { get; set; }
    }
}
