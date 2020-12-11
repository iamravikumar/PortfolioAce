using PortfolioAce.Domain.DataObjects;
using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.ViewModels.Modals
{
    public class NavSummaryViewModel:ViewModelWindowBase
    {
        private ITransferAgencyService _investorService;
        private NavValuations _navValuation;

        public NavSummaryViewModel(NavValuations navValuation, ITransferAgencyService investorService)
        {
            _navValuation = navValuation;
            _investorService = investorService;

            /*
             * Here I will have the:
             * Fund Name
             * Net asset Value
             * expected nav price
             * Two datagrids all of the cash balances and positions on this date and whether or not they are valued at market price (using IsValuedBase)
             * projected management fees
             * shares outstanding
             * a confirmation button to lock the NAV, ONce created i will launched the locking process....
             */
        }

        public string FundName
        {
            get
            {
                return _navValuation.fund.FundName;
            }
        }

        public DateTime AsOfDate
        {
            get
            {
                return _navValuation.AsOfDate;
            }
        }

        public decimal NetAssetValue
        {
            get
            {
                return _navValuation.NetAssetValue;
            }
        }

        public decimal NetAssetValuePS
        {
            get
            {
                return _navValuation.NetAssetValuePerShare;
            }
        }

        public decimal SharesOutstanding
        {
            get
            {
                return _navValuation.SharesOutstanding;
            }
        }

        public decimal ManagementFeeAmount
        {
            get
            {
                return _navValuation.ManagementFeeAmount;
            }
        }
    }
}
