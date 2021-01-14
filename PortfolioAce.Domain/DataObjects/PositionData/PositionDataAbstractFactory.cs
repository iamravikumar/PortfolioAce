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

        public CalculatedCashPosition CreateCashPosition(CurrenciesDIM currency, CustodiansDIM custodian)
        {
            return new LiquidCashPosition(currency, custodian);
        }

        public ValuedCashPosition CreateValuedCashPosition(CalculatedCashPosition cashPosition, Dictionary<(string, DateTime), decimal> priceTable, DateTime asOfDate, string FundBaseCurrency)
        {
            return new ValuedLiquidCashPosition(cashPosition, priceTable, asOfDate, FundBaseCurrency);
        }


        public CalculatedSecurityPosition CreateSecurityPosition(SecuritiesDIM security, CustodiansDIM custodian)
        {
            string assetClass = security.AssetClass.Name;
            switch (assetClass)
            {
                //"Equity, Cryptocurrency, FX"
                case "Equity":
                    return new EquityPosition(security, custodian);
                case "Cryptocurrency":
                    return new CryptoPosition(security, custodian);
                case "FX":
                    return new FXPosition(security, custodian);
                default:
                    throw new ArgumentException("The asset class is not implemented as a security", "assetClass");
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
                    return new ValuedCryptoPosition(position, priceTable, asOfDate, FundBaseCurrency);
                case "FX":
                    return new ValuedFXPosition(position, priceTable, asOfDate, FundBaseCurrency);
                default:
                    throw new ArgumentException("The asset class is not implemented as a security", "assetClass");
            }
        }
    }
}
