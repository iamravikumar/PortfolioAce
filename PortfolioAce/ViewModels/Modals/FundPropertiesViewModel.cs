using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.Domain.Models.FactTables;
using PortfolioAce.EFCore.Services;
using PortfolioAce.EFCore.Services.DimensionServices;
using PortfolioAce.EFCore.Services.FactTableServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PortfolioAce.ViewModels.Modals
{
    public class FundPropertiesViewModel:ViewModelWindowBase
    {
        private IFactTableService _factTableService;
        private IStaticReferences _staticReferences;
        private Fund _fund;
        public FundPropertiesViewModel(IFactTableService factTableService, IStaticReferences staticReferences, Fund fund)
        {
            _factTableService = factTableService;
            _staticReferences = staticReferences;
            _fund = fund;
        }

        public string Title
        {
            get
            {
                return $"{_fund.FundName} ({_fund.Symbol})";
            }
        }

        public string FundSymbol
        {
            get
            {
                return _fund.Symbol;
            }
        }

        public string BaseCurrency
        {
            get
            {
                return _fund.BaseCurrency;
            }
        }

        public string LaunchDate
        {
            get
            {
                return _fund.LaunchDate.ToString("dd/MM/yyyy");
            }
        }

        public decimal MinimumInvestment
        {
            get
            {
                return _fund.MinimumInvestment;
            }
        }

        public string ManagementFeeRate
        {
            get
            {
                return String.Format("{0:P2}", _fund.ManagementFee);
            }
        }

        public string PerformanceFeeRate
        {
            get
            {
                return String.Format("{0:P2}", _fund.PerformanceFee);
            }
        }

        public string HurdleInfo
        {
            get
            {
                return String.Format("{0:P2} {1}", _fund.HurdleRate, _fund.HurdleType);
            }
        }

        public string HighWaterMark
        {
            get
            {
                return _fund.HasHighWaterMark.ToString();
            }
        }

        public string NavFrequency
        {
            get
            {
                return _fund.NAVFrequency;
            }
        }

        public ObservableCollection<NAVPriceStoreFACT> dgNavPrices
        {
            get
            {
                List<NAVPriceStoreFACT> navPrices = _factTableService.GetAllFundNAVPrices(_fund.FundId);
                return new ObservableCollection<NAVPriceStoreFACT>(navPrices);
            }
        }

        public ObservableCollection<AccountingPeriodsDIM> dgNavPeriods
        {
            get
            {
                List<AccountingPeriodsDIM> periods = _staticReferences.GetAllFundPeriods(_fund.FundId);
                return new ObservableCollection<AccountingPeriodsDIM>(periods);
            }
        }


    }
}
