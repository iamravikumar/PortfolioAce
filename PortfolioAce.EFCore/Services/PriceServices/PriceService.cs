using PortfolioAce.DataCentre.APIConnections;
using PortfolioAce.DataCentre.DeserialisedObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services.PriceServices
{
    public class PriceService : IPriceService
    {
        private readonly PortfolioAceDbContextFactory _contextFactory;
        private readonly DataConnectionFactory _dataFactory;
        public PriceService(PortfolioAceDbContextFactory contextFactory, DataConnectionFactory dataFactory)
        {
            this._contextFactory = contextFactory;
            this._dataFactory = dataFactory;
        }

        public async Task<List<AVSecurityPriceData>> GetPrices(string symbol)
        {
            AlphaVantageConnection df = _dataFactory.CreateAlphaVantageClient();
            string uri = $"function=TIME_SERIES_DAILY&symbol={symbol}";
            var x = await df.GetAsync<List<AVSecurityPriceData>>(uri);
            return x;
        }
    }
}
