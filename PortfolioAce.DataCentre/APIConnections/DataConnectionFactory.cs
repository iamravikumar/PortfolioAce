using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.DataCentre.APIConnections
{
    public class DataConnectionFactory
    {
        public DataConnectionFactory()
        {
        }

        public AlphaVantageConnection CreateAlphaVantageClient(string apiKeyAV)
        {
            return new AlphaVantageConnection(apiKeyAV);
        }

        public FMPConnection CreateFMPCleint(string apiKeyFMP)
        {
            return new FMPConnection(apiKeyFMP);
        }
    }
}
