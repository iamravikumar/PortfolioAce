using PortfolioAce.Commands;
using PortfolioAce.Domain.DataObjects;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.EFCore.Services;
using PortfolioAce.EFCore.Services.DimensionServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.ViewModels.Modals
{
    public class NavSummaryViewModel:ViewModelWindowBase
    {
        private ITransferAgencyService _investorService;
        private IStaticReferences _staticReferences;
        private NavValuations _navValuation;

        public NavSummaryViewModel(NavValuations navValuation, ITransferAgencyService investorService, IStaticReferences staticReferences)
        {
            _navValuation = navValuation;
            _investorService = investorService;
            _staticReferences = staticReferences;
            LockNavCommand = new LockNavCommand(this, _navValuation, _investorService);
        }
        public ICommand LockNavCommand { get; set; }

        public string FundName
        {
            get
            {
                return _navValuation.fund.FundName;
            }
        }

        public string BaseCurrency
        {
            get
            {
                return _navValuation.fund.BaseCurrency;
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

        public int UnvaluedPositions
        {
            get
            {
                return _navValuation.UnvaluedPositions;
            }
        }

        public decimal ManagementFeeAmount
        {
            get
            {
                return _navValuation.ManagementFeeAmount;
            }
        }

        public List<SecurityPositionValuation> dgSecurityPositions
        {
            get
            {
                return _navValuation.SecurityPositions;
            }
        }

        public List<CashPositionValuation> dgCashPositions
        {
            get
            {
                return _navValuation.CashPositions;
            }
        }



        public Visibility ValuedMessage
        {
            // determines the message shown if ALL positions are valued..
            get
            {
                return (UnvaluedPositions==0) ? Visibility.Visible : Visibility.Collapsed;
            }
            private set
            {

            }
        }

        public bool EnableLockNav
        {
            get
            {
                // and the accounting period is not locked AND the prior accounting period is not locked....
                AccountingPeriodsDIM period = _staticReferences.GetPeriod(AsOfDate,_navValuation.fund.FundId);
                if (period == null)
                {
                    return false;
                }
                else
                {
                    return (UnvaluedPositions == 0 && !period.isLocked);
                }
            }
        }
    }
}
