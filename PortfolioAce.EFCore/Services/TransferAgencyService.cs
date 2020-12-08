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

        public void InitialiseFundAction(Fund fund, List<TransferAgencyBO> investors, List<TransactionsBO> transactions, NAVPriceStoreFACT initialNav)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                // Saves the first Nav
                context.NavPriceData.Add(initialNav);


                // Saves the funds state to initialised
                context.Funds.Update(fund);

                
                // saves the investors to the database
                context.TransferAgent.AddRange(investors);

                context.Transactions.AddRange(transactions);


                context.SaveChanges();
            }
        }

        public async Task<TransferAgencyBO> UpdateInvestorAction(TransferAgencyBO investorAction)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                context.TransferAgent.Update(investorAction);
                await context.SaveChangesAsync();

                return investorAction;
            }
        }
    }
}
