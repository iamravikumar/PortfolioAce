using PortfolioAce.Commands;
using PortfolioAce.Domain.DataObjects;
using PortfolioAce.Domain.DataObjects.PositionData;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.EFCore.Services;
using PortfolioAce.EFCore.Services.DimensionServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.ViewModels.Modals
{
    public class NavSummaryViewModel : ViewModelWindowBase
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
            UnlockNavCommand = new UnlockNavCommand(this, _investorService);
        }
        public ICommand LockNavCommand { get; }
        public ICommand UnlockNavCommand { get; }

        public int FundId
        {
            get
            {
                return _navValuation.fund.FundId;
            }
        }

        public string FundName
        {
            get
            {
                return _navValuation.fund.FundName;
            }
        }

        public DateTime FundLaunchDate
        {
            get
            {
                return _navValuation.fund.LaunchDate;
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

        public decimal PerformanceFeeAmount
        {
            get
            {
                return _navValuation.PerformanceFeeAmount;
            }
        }

        public List<ValuedSecurityPosition> dgSecurityPositions
        {
            get
            {
                return _navValuation.SecurityPositions;
            }
        }

        public List<ValuedCashPosition> dgCashPositions
        {
            get
            {
                return _navValuation.CashPositions;
            }
        }

        public List<ClientHoldingValuation> dgClientHoldings
        {
            get
            {
                return _navValuation.ClientHoldings;
            }
        }

        public Visibility ValuedMessage
        {
            // determines the message shown if ALL positions are valued..
            get
            {
                return (UnvaluedPositions == 0) ? Visibility.Visible : Visibility.Collapsed;
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
                AccountingPeriodsDIM period = _staticReferences.GetPeriod(AsOfDate, _navValuation.fund.FundId);
                if (period == null)
                {
                    return false;
                }
                else
                {
                    // i need to check if the previous period is locked or not..
                    bool isWeekend = (AsOfDate.DayOfWeek == DayOfWeek.Saturday || AsOfDate.DayOfWeek == DayOfWeek.Sunday); // you can't lock nav on weekend
                    bool isPreviousPeriodLocked = _staticReferences.PreviousPeriodLocked(AsOfDate, _navValuation.fund.FundId); // you can't lock nav if the previous period is locked.
                    return (UnvaluedPositions == 0 && !period.isLocked && !isWeekend && isPreviousPeriodLocked);
                }
            }
        }

        public bool EnableUnlockNav
        {
            get
            {
                // and the accounting period is not locked AND the prior accounting period is not locked....
                List<AccountingPeriodsDIM> periods = _staticReferences.GetAllFundPeriods(_navValuation.fund.FundId);
                AccountingPeriodsDIM period = periods.Where(p => p.AccountingDate == AsOfDate).FirstOrDefault();
                AccountingPeriodsDIM futurePeriods = periods.Where(p => p.AccountingDate > AsOfDate && p.isLocked).FirstOrDefault();
                if (period != null && futurePeriods == null)
                {
                    return period.isLocked;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
