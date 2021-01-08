using PortfolioAce.Domain.DataObjects;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.Domain.BusinessServices
{
    public class PortfolioService : IPortfolioService
    {
        public List<CalculatedCashPosition> GetAllCashBalances(Fund fund, DateTime asOfDate)
        {
            List<TransactionsBO> allTrades = fund.Transactions
                                                 .Where(t=>t.TradeDate<=asOfDate && t.isActive)
                                                 .OrderBy(t => t.TradeDate)
                                                 .ToList();
            Dictionary<(string, string), List<TransactionsBO>> cashTradesByCurrencyAndCustodian = new Dictionary<(string, string), List<TransactionsBO>>();
            foreach (TransactionsBO trade in allTrades)
            {
                ValueTuple<string, string> groupKey = (trade.Currency.Symbol, trade.Custodian.Name); // this allows me to group transactions by security AND custodian
                if (!cashTradesByCurrencyAndCustodian.ContainsKey(groupKey))
                {
                    cashTradesByCurrencyAndCustodian[groupKey] = new List<TransactionsBO> { trade };
                }
                else
                {
                    cashTradesByCurrencyAndCustodian[groupKey].Add(trade);
                }
            }

            List<CalculatedCashPosition> allBalances = new List<CalculatedCashPosition>();

            foreach (KeyValuePair<(string, string), List<TransactionsBO>> Kvp in cashTradesByCurrencyAndCustodian)
            {
                CurrenciesDIM currency = Kvp.Value[0].Currency;
                CustodiansDIM custodian =Kvp.Value[0].Custodian;
                CalculatedCashPosition balance = new CalculatedCashPosition(currency, custodian);
                balance.AddTransactions(Kvp.Value);
                allBalances.Add(balance);
            }
            return allBalances;
        }

        public List<CashPositionValuation> GetAllValuedCashBalances(Fund fund, DateTime asOfDate, Dictionary<(string, DateTime), decimal> priceTable)
        {
            List<CalculatedCashPosition> allCashPositions = GetAllCashBalances(fund, asOfDate);
            List<CashPositionValuation> valuedCashPositions = new List<CashPositionValuation>();
            foreach (CalculatedCashPosition cashPosition in allCashPositions)
            {
                CashPositionValuation valuedCashPosition = new CashPositionValuation(cashPosition, priceTable, asOfDate, fund.BaseCurrency);
                valuedCashPositions.Add(valuedCashPosition);
            }
            return valuedCashPositions;
        }

        public List<ClientHolding> GetAllClientHoldings(Fund fund, DateTime asOfDate)
        {
            List<TransferAgencyBO> allActions = fund.TransferAgent.Where(t => t.IsNavFinal && t.TransactionDate <= asOfDate)
                                                    .OrderBy(t => t.TransactionDate)
                                                    .ToList();
            Dictionary<FundInvestorBO, List<TransferAgencyBO>> transferDict = new Dictionary<FundInvestorBO, List<TransferAgencyBO>>();
            foreach(TransferAgencyBO transaction in allActions)
            {
                FundInvestorBO dictKey = transaction.FundInvestor;
                if (!transferDict.ContainsKey(dictKey))
                {
                    transferDict[dictKey] = new List<TransferAgencyBO> { transaction };
                }
                else
                {
                    transferDict[dictKey].Add(transaction);
                }
            }

            List<ClientHolding> allHoldings = new List<ClientHolding>();

            foreach (KeyValuePair<FundInvestorBO, List<TransferAgencyBO>> Kvp in transferDict)
            {
                ClientHolding holding = new ClientHolding(Kvp.Key);
                holding.AddClientTransactions(Kvp.Value);
                allHoldings.Add(holding);
            }

            return allHoldings;
        }

        public List<CalculatedSecurityPosition> GetAllSecurityPositions(Fund fund, DateTime asOfDate)
        {
            List<TransactionsBO> allTrades = fund.Transactions
                                                 .Where(t => t.isActive && t.TransactionType.TypeClass == "SecurityTrade" && t.TradeDate<=asOfDate)
                                                 .OrderBy(t => t.TradeDate)
                                                 .ToList();

            Dictionary<(string, string), List<TransactionsBO>> tradesBySecurityAndCustodian = new Dictionary<(string, string), List<TransactionsBO>>(); 
            foreach (TransactionsBO trade in allTrades)
            {
                ValueTuple<string, string> groupKey = (trade.Security.Symbol, trade.Custodian.Name); // this allows me to group transactions by security AND custodian
                if (!tradesBySecurityAndCustodian.ContainsKey(groupKey))
                {
                    tradesBySecurityAndCustodian[groupKey] = new List<TransactionsBO> { trade };
                }
                else
                {
                    tradesBySecurityAndCustodian[groupKey].Add(trade);
                }
            }

            List<CalculatedSecurityPosition> allPositions = new List<CalculatedSecurityPosition>();

            foreach(KeyValuePair<(string, string), List<TransactionsBO>> Kvp in tradesBySecurityAndCustodian)
            {
                SecuritiesDIM security = Kvp.Value[0].Security;
                CustodiansDIM custodian = Kvp.Value[0].Custodian;
                CalculatedSecurityPosition position = new CalculatedSecurityPosition(security, custodian);
                position.AddTransactions(Kvp.Value);
                allPositions.Add(position);
            }

            return allPositions;
        }

        public List<SecurityPositionValuation> GetAllValuedSecurityPositions(Fund fund, DateTime asOfDate, Dictionary<(string, DateTime), decimal> priceTable)
        {
            List<CalculatedSecurityPosition> allPositions = GetAllSecurityPositions(fund, asOfDate);
            List<SecurityPositionValuation> valuedPositions = new List<SecurityPositionValuation>();

            foreach (CalculatedSecurityPosition position in allPositions)
            {
                SecurityPositionValuation valuedPosition = new SecurityPositionValuation(position, priceTable, asOfDate, fund.BaseCurrency);
                valuedPositions.Add(valuedPosition);
            }
            return valuedPositions;
        }
    }
}
