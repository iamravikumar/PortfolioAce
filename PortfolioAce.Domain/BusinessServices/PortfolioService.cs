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
        public List<CalculatedCashPosition> GetAllCashBalances(Fund fund)
        {
            List<TransactionsBO> allTrades = fund.Transactions
                                                 .OrderBy(t => t.TradeDate)
                                                 .ToList();
            Dictionary<(CurrenciesDIM, CustodiansDIM), List<TransactionsBO>> tradeDict = new Dictionary<(CurrenciesDIM, CustodiansDIM), List<TransactionsBO>>();
            foreach (TransactionsBO trade in allTrades)
            {
                ValueTuple<CurrenciesDIM, CustodiansDIM> groupKey = (trade.Currency, trade.Custodian); // this allows me to group transactions by security AND custodian
                if (!tradeDict.ContainsKey(groupKey))
                {
                    tradeDict[groupKey] = new List<TransactionsBO> { trade };
                }
                else
                {
                    tradeDict[groupKey].Add(trade);
                }
            }

            List<CalculatedCashPosition> allBalances = new List<CalculatedCashPosition>();

            foreach (KeyValuePair<(CurrenciesDIM, CustodiansDIM), List<TransactionsBO>> Kvp in tradeDict)
            {
                CalculatedCashPosition balance = new CalculatedCashPosition(Kvp.Key.Item1, Kvp.Key.Item2);
                balance.AddTransactions(Kvp.Value);
                allBalances.Add(balance);
            }
            return allBalances;
        }

        public List<CalculatedSecurityPosition> GetAllSecurityPositions(Fund fund)
        {
            List<TransactionsBO> allTrades = fund.Transactions
                                                 .Where(t => t.isActive && t.TransactionType.TypeClass.ToString() == "SecurityTrade")
                                                 .OrderBy(t => t.TradeDate)
                                                 .ToList();

            Dictionary<(SecuritiesDIM, CustodiansDIM), List<TransactionsBO>> tradeDict = new Dictionary<(SecuritiesDIM, CustodiansDIM), List<TransactionsBO>>(); 
            foreach (TransactionsBO trade in allTrades)
            {
                ValueTuple<SecuritiesDIM, CustodiansDIM> groupKey = (trade.Security, trade.Custodian); // this allows me to group transactions by security AND custodian
                if (!tradeDict.ContainsKey(groupKey))
                {
                    tradeDict[groupKey] = new List<TransactionsBO> { trade };
                }
                else
                {
                    tradeDict[groupKey].Add(trade);
                }
            }

            List<CalculatedSecurityPosition> allPositions = new List<CalculatedSecurityPosition>();

            foreach(KeyValuePair<(SecuritiesDIM, CustodiansDIM), List<TransactionsBO>> Kvp in tradeDict)
            {
                CalculatedSecurityPosition position = new CalculatedSecurityPosition(Kvp.Key.Item1, Kvp.Key.Item2);
                position.AddTransactions(Kvp.Value);
                allPositions.Add(position);
            }

            return allPositions;
        }
    }
}
