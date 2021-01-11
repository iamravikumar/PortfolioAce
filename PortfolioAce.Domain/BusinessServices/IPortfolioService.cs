using PortfolioAce.Domain.DataObjects;
using PortfolioAce.Domain.DataObjects.PositionData;
using PortfolioAce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.Domain.BusinessServices
{
    public interface IPortfolioService
    {

        List<CalculatedSecurityPosition> GetAllSecurityPositions(Fund fund, DateTime asOfDate);
        List<CalculatedCashPosition> GetAllCashPositions(Fund fund, DateTime asOfDate);
        List<ClientHolding> GetAllClientHoldings(Fund fund, DateTime asOfDate);
        List<ValuedSecurityPosition> GetAllValuedSecurityPositions(Fund fund, DateTime asOfDate, Dictionary<(string, DateTime), decimal> priceTable);

        List<ValuedCashPosition> GetAllValuedCashPositions(Fund fund, DateTime asOfDate, Dictionary<(string, DateTime), decimal> priceTable);
        List<ValuedPosition> GetAllValuedPosiitons(Fund fund, DateTime asOfDate, Dictionary<(string, DateTime), decimal> priceTable);

    }
}
