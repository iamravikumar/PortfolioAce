using PortfolioAce.Domain.DataObjects;
using PortfolioAce.Domain.Handlers;
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

        public List<PositionHandler> GetAllPositions(Fund fund)
        {
            return new List<PositionHandler>();
            /*
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

            List<PositionHandler> result = new List<PositionHandler>();

            foreach (KeyValuePair<SecuritiesDIM, List<TradeBO>> Kvp in tradeDict)
            {
                PositionHandler pos = new PositionHandler(Kvp.Key);
                foreach (TradeBO t in Kvp.Value)
                {
                    pos.AddTransaction(t);
                }
                result.Add(pos);
            }
            return result;
            
            */
        }
    }
}
