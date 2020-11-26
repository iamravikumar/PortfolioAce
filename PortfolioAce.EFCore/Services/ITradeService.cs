using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services
{
    public interface ITradeService
    {
        Task<TradeBO> GetTradeById(int id);
        Task<TradeBO> CreateTrade(TradeBO trade);
        Task<TradeBO> UpdateTrade(TradeBO trade);
        Task<TradeBO> DeleteTrade(int id);
        SecuritiesDIM GetSecurityInfo(string symbol);
        bool SecurityExists(string symbol);
        List<TradeBO> GetTradesBySymbol(string Symbol, int fundId);
        List<TradeBO> GetAllFundTrades(int fundId);

    }
}
