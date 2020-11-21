using ServiceStack;
using System;
using System.Collections.Generic;
using System.Text;
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

        public async Task<T> GetAsync<T>(string uri)
        {
            // i would use this like Task<List<object>>...
            // This generic method is very flexible because i can manipulate the uri for services to get different types of data...
            // as well as create different types of objects
            string connection = $"{BASE_ADDRESS_QUERY}/query?apikey={_apiKeyAV}&{uri}&datatype=csv";
            string response = await connection.GetStringFromUrlAsync();
            T result = response.FromCsv<T>();
            return result;
        }
    }
}
