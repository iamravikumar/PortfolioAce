using PortfolioAce.Domain.Models.FactTables;
using System;
using System.Collections.Generic;

namespace PortfolioAce.EFCore.Services.FactTableServices
{
    public interface IFactTableService : IBaseService
    {
        List<PositionFACT> GetAllStoredPositions(DateTime date, int FundId, bool onlyActive = false);
        List<PositionFACT> GetAllFundStoredPositions(int fundId, int securityId);
        List<NAVPriceStoreFACT> GetAllNAVPrices();

        List<NAVPriceStoreFACT> GetAllFundNAVPrices(int fundId);
    }
}
