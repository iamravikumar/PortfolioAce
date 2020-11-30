using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.Domain.Models.FactTables;
using PortfolioAce.EFCore.HelperMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services
{
    public class TransferAgencyService : ITransferAgencyService
    {

        private readonly PortfolioAceDbContextFactory _contextFactory;

        public TransferAgencyService(PortfolioAceDbContextFactory contextFactory)
        {
            this._contextFactory = contextFactory;
        }

        public async Task<TransferAgencyBO> CreateInvestorAction(TransferAgencyBO investorAction)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<TransferAgencyBO> res = await context.TransferAgent.AddAsync(investorAction);
                await context.SaveChangesAsync();
                CashBookBO  transaction = TransactionMapper(res.Entity, new CashBookBO());
                await context.CashBooks.AddAsync(transaction);
                await context.SaveChangesAsync();

                return res.Entity;
            }
        }

        public async Task<TransferAgencyBO> DeleteInvestorAction(int id)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                TransferAgencyBO investorAction = await context.TransferAgent.FindAsync(id);
                if (investorAction == null)
                {
                    return investorAction;
                }
                context.TransferAgent.Remove(investorAction);
                //TODO: raise a warning if there is no transaction to remove. Big issue if this is the case.
                CashBookBO transaction = await context.CashBooks.Where(ta => ta.TransferAgencyId == id).FirstAsync();
                context.CashBooks.Remove(transaction);
                await context.SaveChangesAsync();

                return investorAction;
            }
        }

        public List<TransferAgencyBO> GetAllFundInvestorActions(int fundId)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.TransferAgent.Where(ta => ta.FundId == fundId).OrderBy(ta => ta.TransactionDate).ToList();
            }
        }

        public async Task<TransferAgencyBO> GetInvestorActionById(int id)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.TransferAgent.FindAsync(id);
            }
        }

        public void InitialiseFundAction(Fund fund, List<TransferAgencyBO> investors, NAVPriceStoreFACT initialNav)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                // need to update fund to initialised, add TA to database, add cashbook to DB,
                // Need to update the funds first NAV
                context.Funds.Update(fund);
                context.NavPriceData.Add(initialNav);
                foreach (TransferAgencyBO investor in investors)
                {
                    var res = context.TransferAgent.Add(investor);
                    context.SaveChanges();
                    CashBookBO transaction = TransactionMapper(res.Entity, new CashBookBO());
                    context.CashBooks.Add(transaction);

                };
                // set the monthly or daily account periods for up to one year ahead...
                DateTime startDate = fund.LaunchDate;
                List<DateTime> allPeriods;
                if (fund.NAVFrequency == "Daily")
                {
                    // get daily dates from fund launch to a year ahead
                    allPeriods = DateSettings.AnnualWorkingDays(startDate);
                }
                else
                {
                    // get month end dates from fund launch to a year ahead
                    allPeriods = DateSettings.AnnualMonthEnds(startDate);
                }
                // add all the dates to the new periods
                List<AccountingPeriodsDIM> initialPeriods = new List<AccountingPeriodsDIM>();
                foreach(DateTime period in allPeriods)
                {
                    AccountingPeriodsDIM newPeriod;
                    if (period == fund.LaunchDate)
                    {
                        newPeriod = new AccountingPeriodsDIM { AccountingDate = period.Date, isLocked = true, FundId=fund.FundId };
                    }
                    else
                    {
                        newPeriod = new AccountingPeriodsDIM { AccountingDate = period.Date, isLocked = false, FundId = fund.FundId };
                    }
                    initialPeriods.Add(newPeriod);
                }
                context.Periods.AddRange(initialPeriods);

                context.SaveChanges();
            }
        }

        public async Task<TransferAgencyBO> UpdateInvestorAction(TransferAgencyBO investorAction)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                context.TransferAgent.Update(investorAction);
                CashBookBO transaction = await context.CashBooks.Where(ta => ta.TransferAgencyId == investorAction.TransferAgencyId).FirstAsync();
                CashBookBO newTransaction = TransactionMapper(investorAction, transaction);
                context.CashBooks.Update(newTransaction);
                await context.SaveChangesAsync();

                return investorAction;
            }
        }

        private CashBookBO TransactionMapper(TransferAgencyBO investorAction, CashBookBO transaction)
        {
            transaction.TransferAgencyId = investorAction.TransferAgencyId;
            transaction.TransactionType = investorAction.IssueType;// subscription or redemption
            transaction.TransactionAmount = investorAction.TradeAmount;
            transaction.TransactionDate = investorAction.TransactionSettleDate;
            transaction.Currency = investorAction.Currency;
            transaction.FundId = investorAction.FundId;

            transaction.Comment = (investorAction.IssueType =="Subscription")?"FUND SUBSCRIPTION":"FUND REDEMPTION";
            return transaction;
        }
    }
}
