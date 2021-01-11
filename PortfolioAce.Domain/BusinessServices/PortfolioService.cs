using PortfolioAce.Domain.DataObjects;
using PortfolioAce.Domain.DataObjects.PositionData;
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
        private readonly PositionDataAbstractFactory _positionFactory = new PositionDataAbstractFactory();
        public PortfolioService()
        {
        }



        public List<CalculatedCashPosition> GetAllCashPositions(Fund fund, DateTime asOfDate)
        {
            List<TransactionsBO> allTrades = fund.Transactions
                                     .Where(t => t.TradeDate <= asOfDate && t.isActive)
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

            List<CalculatedCashPosition> allCashPositions = new List<CalculatedCashPosition>();

            foreach (KeyValuePair<(string, string), List<TransactionsBO>> Kvp in cashTradesByCurrencyAndCustodian)
            {
                CurrenciesDIM currency = Kvp.Value[0].Currency;
                CustodiansDIM custodian = Kvp.Value[0].Custodian;
                CalculatedCashPosition balance = _positionFactory.CreateCashPosition(currency, custodian);
                balance.AddTransactionRange(Kvp.Value);
                allCashPositions.Add(balance);
            }
            return allCashPositions;
        }

        public List<ValuedCashPosition> GetAllValuedCashPositions(Fund fund, DateTime asOfDate, Dictionary<(string, DateTime), decimal> priceTable)
        {
            List<CalculatedCashPosition> allCashPositions = GetAllCashPositions(fund, asOfDate);
            List<ValuedCashPosition> valuedCashPositions = new List<ValuedCashPosition>();
            foreach (CalculatedCashPosition cashPosition in allCashPositions)
            {
                ValuedCashPosition valuedCashPosition = _positionFactory.CreateValuedCashPosition(cashPosition, priceTable, asOfDate, fund.BaseCurrency);
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
                CalculatedSecurityPosition position= _positionFactory.CreateSecurityPosition(security, custodian);
                position.AddTransactionRange(Kvp.Value);
                allPositions.Add(position);
            }

            return allPositions;
        }

        public List<ValuedSecurityPosition> GetAllValuedSecurityPositions(Fund fund, DateTime asOfDate, Dictionary<(string, DateTime), decimal> priceTable)
        {
            // SecurityPositionValuation
            List<CalculatedSecurityPosition> allPositions = GetAllSecurityPositions(fund, asOfDate);
            List<ValuedSecurityPosition> valuedPositions = new List<ValuedSecurityPosition>();

            foreach (CalculatedSecurityPosition position in allPositions)
            {
                ValuedSecurityPosition valuedPosition = _positionFactory.CreateValuedSecurityPosition(position, priceTable, asOfDate, fund.BaseCurrency);
                valuedPositions.Add(valuedPosition);
            }
            return valuedPositions;
        }


        public List<ValuedPosition> GetAllValuedPosiitons(Fund fund, DateTime asOfDate, Dictionary<(string, DateTime), decimal> priceTable)
        {
            List<ValuedPosition> allValuedPositions = new List<ValuedPosition>();
            List<CalculatedSecurityPosition> allSecurityPositions = GetAllSecurityPositions(fund, asOfDate);

            foreach (CalculatedSecurityPosition position in allSecurityPositions)
            {
                ValuedPosition valuedPosition = _positionFactory.CreateValuedSecurityPosition(position, priceTable, asOfDate, fund.BaseCurrency);
                allValuedPositions.Add(valuedPosition);
            }

            List<CalculatedCashPosition> allCashPositions = GetAllCashPositions(fund, asOfDate);
            foreach(CalculatedCashPosition cashPosition in allCashPositions)
            {
                ValuedPosition valuedCashPosition = _positionFactory.CreateValuedCashPosition(cashPosition, priceTable, asOfDate, fund.BaseCurrency);
                allValuedPositions.Add(valuedCashPosition);
            }

            return allValuedPositions;
        }
    }
}
