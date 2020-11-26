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
            List<TradeBO> allTrades = fund.Trades.OrderBy(t => t.TradeDate).ToList();// this orders the trades by trade date.
            Dictionary<SecuritiesDIM, List<TradeBO>> tradeDict = new Dictionary<SecuritiesDIM, List<TradeBO>>();

            foreach (TradeBO t in allTrades)
            {
                SecuritiesDIM security = t.Security;
                if (!tradeDict.ContainsKey(security))
                {
                    tradeDict[security] = new List<TradeBO> { t };
                }
                else
                {
                    tradeDict[security].Add(t);
                }
            }

            List<Position> result = new List<Position>();

            foreach (KeyValuePair<SecuritiesDIM, List<TradeBO>> Kvp in tradeDict)
            {
                Position pos = new Position(Kvp.Key);
                foreach (TradeBO t in Kvp.Value)
                {
                    pos.AddTransaction(t);
                }
                result.Add(pos);
            }
            return result;
            
        }
    }
}
