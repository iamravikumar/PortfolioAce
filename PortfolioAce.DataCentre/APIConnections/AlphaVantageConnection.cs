using PortfolioAce.DataCentre.DeserialisedObjects;
using PortfolioAce.Domain.Models.Dimensions;
using ServiceStack;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortfolioAce.DataCentre.APIConnections
{
    // effectively with this object i will create a service layer on top of it
    public class AlphaVantageConnection
    {
        private readonly string _apiKeyAV;
        private const string BASE_ADDRESS_QUERY = "https://www.alphavantage.co";
        public AlphaVantageConnection(string apiKeyAV)
        {
            _apiKeyAV = apiKeyAV;
        }

        public async Task<IEnumerable<AVSecurityPriceData>> GetPricesAsync(SecuritiesDIM security)
        {
            string uri = GenerateURI(security);
            string connection = $"{BASE_ADDRESS_QUERY}/query?apikey={_apiKeyAV}&{uri}&datatype=csv";
            string response = await connection.GetStringFromUrlAsync();
            string assetClass = security.AssetClass.Name;
            IEnumerable<AVSecurityPriceData> result;

            if (assetClass == "Cryptocurrency")
            {
                result = response.FromCsv<List<AVCryptoPriceData>>();
            }
            else if (assetClass == "FX")
            {
                result = response.FromCsv<List<AVFXPriceData>>();
            }
            else
            {
                result = response.FromCsv<List<AVEquityPriceData>>();
            }
            return result;
        }

        private string GenerateURI(SecuritiesDIM security)
        {
            string assetClass = security.AssetClass.Name;
            string symbol = security.AlphaVantageSymbol;
            string uri;

            if (assetClass == "FX")
            {
                string from_symbol = symbol.Substring(0, 3);
                string to_symbol = symbol.Substring(3);
                uri = $"function=FX_DAILY&from_symbol={from_symbol}&to_symbol={to_symbol}";
            }
            else if (assetClass == "Cryptocurrency")
            {
                // This isnt perfect yet I need to figure out how to deserialise the ClosePrice
                uri = $"function=DIGITAL_CURRENCY_DAILY&symbol={symbol}&market=USD";
            }
            else
            {
                uri = $"function=TIME_SERIES_DAILY&symbol={symbol}";
            }
            return uri;
        }
    }
}
