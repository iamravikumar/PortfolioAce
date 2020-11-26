using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.BackOfficeModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services
{
    public interface ICashTradeService
    {
        Task<CashTradeBO> GetCashTradeById(int id);
        Task<CashTradeBO> CreateCashTrade(CashTradeBO cashTrade);
        Task<CashTradeBO> UpdateCashTrade(CashTradeBO cashTrade);
        Task<CashTradeBO> DeleteCashTrade(int id);
        List<CashTradeBO> GetAllFundCashTrades(int fundId);

    }
}
