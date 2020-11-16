using PortfolioAce.Domain.DataObjects;
using PortfolioAce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.Domain.BusinessServices
{
    public class PortfolioService : IPortfolioService
    {
        public CashHoldings GetAllCashBalances(Fund fund)
        {
            var x = fund.CashBooks.ToList();
            var y = x.GroupBy(ccy => ccy.Currency, (key, values)
                 => new CashAccountBalance(key, values.Sum(ccy => ccy.TransactionAmount))).ToList();
            CashHoldings cashHoldings = new CashHoldings();
            foreach(CashAccountBalance account in y)
            {
                cashHoldings.Add(account);
            }
            return cashHoldings;
        }

        public List<Position> GetAllPositions(Fund fund)
        {
            
            List<Trade> allTrades = fund.Trades.OrderBy(t => t.TradeDate).ToList();// this orders the trades by trade date.
            Dictionary<Security, List<Trade>> tradeDict = new Dictionary<Security, List<Trade>>();

            foreach (Trade t in allTrades)
            {
                Security security = t.Security;
                if (!tradeDict.ContainsKey(security))
                {
                    tradeDict[security] = new List<Trade> { t };
                }
                else
                {
                    tradeDict[security].Add(t);
                }
            }

            List<Position> result = new List<Position>();

            foreach (KeyValuePair<Security, List<Trade>> Kvp in tradeDict)
            {
                Position pos = new Position(Kvp.Key);
                foreach (Trade t in Kvp.Value)
                {
                    pos.AddTransaction(t);
                }
                result.Add(pos);
            }
            return result;
            
        }
    }
}
