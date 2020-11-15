using PortfolioAce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services
{
    public interface ICashTradeRepository
    {
        Task<CashTrade> GetCashTradeById(int id);
        Task<CashTrade> CreateCashTrade(CashTrade cashTrade);
        Task<CashTrade> UpdateCashTrade(CashTrade cashTrade);
        Task<CashTrade> DeleteCashTrade(int id);
        List<CashTrade> GetAllFundCashTrades(int fundId);

    }
}
