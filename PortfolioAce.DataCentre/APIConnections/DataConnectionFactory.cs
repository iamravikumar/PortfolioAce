using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.DataCentre.APIConnections
{
    public class DataConnectionFactory
    {
        private readonly string _apiKeyAV;
        private readonly string _apiKeyFMP;
        public DataConnectionFactory(string apiKeyAV="",string apiKeyFMP="")
        {
            _apiKeyAV = apiKeyAV;
            _apiKeyFMP = apiKeyFMP;
        }

        public AlphaVantageConnection CreateAlphaVantageClient()
        {
            return new AlphaVantageConnection(_apiKeyAV);
        }

        public FMPConnection CreateFMPCleint()
        {
            return new FMPConnection(_apiKeyFMP);
        }
    }
}
