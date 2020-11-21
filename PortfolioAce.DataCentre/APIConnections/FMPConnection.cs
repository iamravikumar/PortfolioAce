using ServiceStack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.DataCentre.APIConnections
{
    // FMP = Financial Modeling Prep
    // effectively with this object i will create a service layer on top of it
    public class FMPConnection
    {
        private readonly string _apiKeyFMP;
        private const string BASE_ADDRESS_QUERY = "https://financialmodelingprep.com/api/v3/";
        public FMPConnection(string apiKeyFMP)
        {
            _apiKeyFMP = apiKeyFMP;
        }

        public async Task<T> GetAsync<T>(string uri)
        {
            string connection = $"{BASE_ADDRESS_QUERY}{uri}?apikey={_apiKeyFMP}";
            string response = await connection.GetStringFromUrlAsync();
            T result = response.FromJson<T>();
            return result;
        }
    }
}
