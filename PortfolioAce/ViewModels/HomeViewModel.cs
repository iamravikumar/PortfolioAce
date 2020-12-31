using LiveCharts;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.FactTables;
using PortfolioAce.EFCore.Services;
using PortfolioAce.EFCore.Services.FactTableServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.ViewModels
{
    public class HomeViewModel: ViewModelBase
    {

        private IFactTableService _factTableService;

        public HomeViewModel(IFactTableService factTableService)
        {
            _factTableService = factTableService;
            _dgAllNavPrices = _factTableService.GetAllNAVPrices();
            if (dgAllNavPrices.Count > 0)
            {
                _selectedPrice = dgLatestNavPrices[0];
                Load(_selectedPrice.FundId);
            }
        }

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
            }
        }

        public ChartValues<decimal> NavPriceLineChartYAxis { get; set; }
        public string[] NavPriceLineChartXAxis { get; set; }




        public async Task Load(int fundId)
        {
            if (NavPriceLineChartYAxis!= null)
            {
               NavPriceLineChartYAxis.Clear();
                NavPriceLineChartYAxis.AddRange(new ChartValues<decimal>(dgAllNavPrices.Where(np => np.FundId == fundId).Select(np => np.NAVPrice)));
            }
            else
            {
                NavPriceLineChartYAxis = new ChartValues<decimal>(dgAllNavPrices.Where(np => np.FundId == fundId).Select(np => np.NAVPrice));
                NavPriceLineChartXAxis = dgAllNavPrices.Where(np => np.FundId == fundId).Select(np => np.FinalisedDate.ToString("dd/MM/yyyy")).ToArray();
            }

            
        }
    }
}
