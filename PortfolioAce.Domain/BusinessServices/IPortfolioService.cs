using PortfolioAce.Domain.DataObjects;
using PortfolioAce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.Domain.BusinessServices
{
    public interface IPortfolioService
    {
        List<Position> GetAllPositions(Fund fund);

        CashHoldings GetAllCashBalances(Fund fund);

    }
}
