using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.Domain.DataObjects.PositionData
{
    public class PositionDataAbstractFactory
    {
        public PositionDataAbstractFactory()
        {
        }

        public CalculatedSecurityPosition CreateSecurityPosition(SecuritiesDIM security, CustodiansDIM custodian)
        {
            string assetClass = security.AssetClass.Name;
            switch (assetClass)
            {
                //"Equity, Cryptocurrency, FX"
                case "Equity":
                    return new CalculatedEquityPosition(security, custodian);
                case "Cryptocurrency":
                    return new CalculatedCryptoPosition(security, custodian);
                case "FX":
                    return new CalculatedFXPosition(security, custodian);
                default:
                    throw new ArgumentException("The asset class is not implemented", "assetClass");
            }
        }

        public ValuedSecurityPosition CreateValuedSecurityPosition(CalculatedSecurityPosition position, Dictionary<(string, DateTime), decimal> priceTable, DateTime asOfDate, string FundBaseCurrency)
        {
            string assetClass = position.Security.AssetClass.Name;
            switch (assetClass)
            {
                //"Equity, Cryptocurrency, FX"
                case "Equity":
                    return new ValuedEquityPosition(position,  priceTable, asOfDate, FundBaseCurrency);
                case "Cryptocurrency":
                    return new ValuedCryptoPosition();
                case "FX":
                    return new ValuedFXPosition();
                default:
                    throw new ArgumentException("The asset class is not implemented", "assetClass");
            }
        }
    }
}
