using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PortfolioAce.Domain.DataObjects.DTOs;
using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly PortfolioAceDbContextFactory _contextFactory;

        public TransactionService(PortfolioAceDbContextFactory contextFactory)
        {
            this._contextFactory = contextFactory;
        }

        public async Task<TransactionsBO> CreateFXTransaction(ForexDTO fxTransaction)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                /* Trade Type is hardcoded to FXTrade
                 * 
                 * Steps:
                 * Create a security if not exist for the FX Forward i.e. symbol USDGBP130121, name USD/GBP 13/01/21, ccy=USD
                 * Create the Transactions BO object. quantity is buy amount, trade amount=0, 
                 * Create the collapse trade type=fxtradecollapse
                 * Create 2 Cash Transactions for the buy and sell legs (type=fxBuy and fxSell)... 
                 * I need a nullable column for Linked trades. 
                 * I need a table called LinkedTransactions to allow me to connect these four items together..
                 * [^basically i create this object and put the ID on all four items.]
                 * So when it comes to editing i can just grab them all.
                 * REMEMBER FXTrade is just a REFERENCE. the actual cash movement is on the cash trades.
                 * HIDE FX AND CASH FROM SECURITY MANAGER!!!!
                 * */
                SecuritiesDIM fxSecurity = context.Securities.Where(s => s.Symbol == fxTransaction.Symbol).FirstOrDefault();
                AssetClassDIM assetClass = context.AssetClasses.Where(a => a.Name == "FX").FirstOrDefault();
                CustodiansDIM custodian = context.Custodians.Where(c => c.Name == fxTransaction.Custodian).FirstOrDefault();
                List<TransactionTypeDIM> transactionTypes = context.TransactionTypes.ToList();
                List<CurrenciesDIM> currencies = context.Currencies.ToList();

                CurrenciesDIM buyCurrency = context.Currencies.Where(c=>c.Symbol==fxTransaction.BuyCurrency).First();
                CurrenciesDIM sellCurrency = context.Currencies.Where(c => c.Symbol == fxTransaction.SellCurrency).First();
                if (fxSecurity == null)
                {
                    fxSecurity = new SecuritiesDIM
                    {
                        AssetClassId = assetClass.AssetClassId,
                        CurrencyId = buyCurrency.CurrencyId,
                        SecurityName = fxTransaction.Name,
                        Symbol = fxTransaction.Symbol
                    };
                    context.Securities.Add(fxSecurity);
                }
                // Created LinkedTransactions Here

                TransactionsBO refTransaction = new TransactionsBO
                {
                    Security = fxSecurity,
                    FundId = fxTransaction.FundId,
                    isActive = true,
                    isLocked = false,
                    TradeAmount = 0,
                    Price = fxTransaction.Price,
                    Quantity = fxTransaction.BuyAmount,
                    CurrencyId = buyCurrency.CurrencyId,
                    TransactionTypeId = transactionTypes.Where(tt => tt.TypeName == "FXTrade").First().TransactionTypeId,
                    Comment = "",
                    CustodianId=custodian.CustodianId,
                    Fees = 0,
                    isCashTransaction = false,
                    TradeDate = fxTransaction.TradeDate,
                    SettleDate = fxTransaction.SettleDate,
                    CreatedDate = DateTime.Now,
                    LastModified = DateTime.Now
                };
                TransactionsBO refTransactionCollapse = new TransactionsBO
                {
                    Security = fxSecurity,
                    FundId = fxTransaction.FundId,
                    isActive = true,
                    isLocked = false,
                    TradeAmount = 0,
                    Price = fxTransaction.Price,
                    Quantity = fxTransaction.BuyAmount*-1,
                    CurrencyId = buyCurrency.CurrencyId,
                    TransactionTypeId = transactionTypes.Where(tt => tt.TypeName == "FXTradeCollapse").First().TransactionTypeId,
                    Comment = "",
                    CustodianId = custodian.CustodianId,
                    Fees = 0,
                    isCashTransaction = false,
                    TradeDate = fxTransaction.SettleDate,
                    SettleDate = fxTransaction.SettleDate,
                    CreatedDate = DateTime.Now,
                    LastModified = DateTime.Now
                };
                EntityEntry<TransactionsBO> res = await context.Transactions.AddAsync(refTransaction);
                context.Transactions.Add(refTransactionCollapse);

                SecuritiesDIM fxBuySecurity = context.Securities.Where(s => s.Symbol == $"{fxTransaction.BuyCurrency}c").FirstOrDefault();
                SecuritiesDIM fxSellSecurity = context.Securities.Where(s => s.Symbol == $"{fxTransaction.SellCurrency}c").FirstOrDefault();

                TransactionsBO fxBuyLegCash = new TransactionsBO
                {
                    SecurityId = fxBuySecurity.SecurityId,
                    FundId = fxTransaction.FundId,
                    isActive = true,
                    isLocked = false,
                    TradeAmount = fxTransaction.BuyAmount,
                    Price = 1,
                    Quantity = fxTransaction.BuyAmount,
                    CurrencyId = buyCurrency.CurrencyId,
                    TransactionTypeId = transactionTypes.Where(tt => tt.TypeName == "FXBuy").First().TransactionTypeId,
                    Comment = fxTransaction.Description,
                    CustodianId = custodian.CustodianId,
                    Fees = 0,
                    isCashTransaction = false,
                    TradeDate = fxTransaction.SettleDate,
                    SettleDate = fxTransaction.SettleDate,
                    CreatedDate = DateTime.Now,
                    LastModified = DateTime.Now
                };
                TransactionsBO fxSellLegCash = new TransactionsBO
                {
                    SecurityId= fxSellSecurity.SecurityId,
                    FundId = fxTransaction.FundId,
                    isActive = true,
                    isLocked = false,
                    TradeAmount = fxTransaction.SellAmount,
                    Price = 1,
                    Quantity = fxTransaction.SellAmount,
                    CurrencyId = sellCurrency.CurrencyId,
                    TransactionTypeId = transactionTypes.Where(tt => tt.TypeName == "FXSell").First().TransactionTypeId,
                    Comment = fxTransaction.Description,
                    CustodianId = custodian.CustodianId,
                    Fees = 0,
                    isCashTransaction = false,
                    TradeDate = fxTransaction.SettleDate,
                    SettleDate = fxTransaction.SettleDate,
                    CreatedDate = DateTime.Now,
                    LastModified = DateTime.Now
                };
                context.Transactions.Add(fxBuyLegCash);
                context.Transactions.Add(fxSellLegCash);

                context.SaveChangesAsync();
                return refTransaction;
            }
        }

        public async Task<TransactionsBO> CreateTransaction(TransactionsBO transaction)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<TransactionsBO> res = await context.Transactions.AddAsync(transaction);
                await context.SaveChangesAsync();
                return res.Entity;
            }
        }

        public void DeleteTransaction(TransactionsBO transaction)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                TransactionsBO originalTransaction = context.Transactions.First(t => t.TransactionId == transaction.TransactionId);
                transaction.isActive = false;
                context.Entry(originalTransaction).CurrentValues.SetValues(transaction);
                context.SaveChangesAsync();
            }
        }

        public CustodiansDIM GetCustodian(string name)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Custodians.Where(c => (c.Name) == name).FirstOrDefault();
            }
        }

        public SecuritiesDIM GetSecurityInfo(string symbol)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Securities.Where(s => s.Symbol == symbol).Include(s=>s.Currency).Include(s=>s.AssetClass).FirstOrDefault();
            }
        }

        public TransactionTypeDIM GetTradeType(string name)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.TransactionTypes.Where(t => (t.TypeName) == name).FirstOrDefault();
            }
        }

        public void RestoreTransaction(TransactionsBO transaction)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                TransactionsBO originalTransaction = context.Transactions.First(t => t.TransactionId == transaction.TransactionId);
                transaction.isActive = true;
                context.Entry(originalTransaction).CurrentValues.SetValues(transaction);
                context.SaveChangesAsync();
            }
        }

        public bool SecurityExists(string symbol)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return (context.Securities.Where(s => s.Symbol == symbol).FirstOrDefault() != null);
            }
        }

        public void UpdateTransaction(TransactionsBO transaction)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                TransactionsBO originalTransaction = context.Transactions.First(t=>t.TransactionId==transaction.TransactionId);
                context.Entry(originalTransaction).CurrentValues.SetValues(transaction);
                context.SaveChanges();
            }
        }
    }
}
