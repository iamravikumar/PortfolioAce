using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore.Services;
using PortfolioAce.EFCore.Services.DimensionServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.ViewModels.Modals
{
    public class FundPropertiesViewModel:ViewModelWindowBase
    {
        private ITransferAgencyService _investorService;
        private IStaticReferences _staticReferences;
        private Fund _fund;
        public FundPropertiesViewModel(ITransferAgencyService investorService, IStaticReferences staticReferences, Fund fund)
        {
            _investorService = investorService;
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
    }
}
