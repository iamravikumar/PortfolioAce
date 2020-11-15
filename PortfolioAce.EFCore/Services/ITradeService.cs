using PortfolioAce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services
{
    public interface ITradeService
    {
        Task<Trade> GetTradeById(int id);
        Task<Trade> CreateTrade(Trade trade);
        Task<Trade> UpdateTrade(Trade trade);
        Task<Trade> DeleteTrade(int id);
        bool SecurityExists(string symbol);
        List<Trade> GetTradesBySymbol(string Symbol, int fundId);
        List<Trade> GetAllFundTrades(int fundId);

    }
}
