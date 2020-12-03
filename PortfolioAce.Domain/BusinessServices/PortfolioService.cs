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

        public List<CalculatedSecurityPosition> GetAllSecurityPositions(Fund fund)
        {
            List<TransactionsBO> allTrades = fund.Transactions
                                                  .Where(t => t.isActive && t.TransactionType.TypeClass.ToString() == "SecurityTrade")
                                                  .OrderBy(t => t.TradeDate)
                                                  .ToList();
            Dictionary<SecuritiesDIM, List<TransactionsBO>> tradeDict = new Dictionary<SecuritiesDIM, List<TransactionsBO>>();

            var x = new Dictionary<(SecuritiesDIM, CustodiansDIM), List<TransactionsBO>>(); // USE THIS ONE GOING FORWARD
            foreach (TransactionsBO t in allTrades)
            {
                SecuritiesDIM security = t.Security;
                CustodiansDIM custodians = t.Custodian;
                if (!x.ContainsKey((security,custodians)))
                {
                    x[(security,custodians)] = new List<TransactionsBO> { t };
                }
                else
                {
                    x[(security, custodians)].Add(t);
                }
            }
            /*
            foreach (TransactionsBO t in allTrades)
            {
                SecuritiesDIM security = t.Security;
                if (!tradeDict.ContainsKey(security))
                {
                    tradeDict[security] = new List<TransactionsBO> { t };
                }
                else
                {
                    tradeDict[security].Add(t);
                }
            }
            */
            List<CalculatedSecurityPosition> result = new List<CalculatedSecurityPosition>();
            // i need to calculate positions based on the custodians look into ToLookUp instead of dictionary...
            foreach (KeyValuePair<SecuritiesDIM, List<TransactionsBO>> Kvp in tradeDict)
            {
                CalculatedSecurityPosition pos = new CalculatedSecurityPosition(Kvp.Key);
                foreach (TransactionsBO t in Kvp.Value)
                {
                    pos.AddTransaction(t);
                }
                result.Add(pos);
            }
            return result;
        }
    }
}
