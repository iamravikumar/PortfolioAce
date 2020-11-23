using PortfolioAce.DataCentre.APIConnections;
using PortfolioAce.DataCentre.DeserialisedObjects;
using PortfolioAce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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


        public async Task<List<AVSecurityPriceData>> AddPrices(string symbol)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                int symbolId = context.Securities.Where(s=>s.Symbol == symbol).Select(s=>s.SecurityId).FirstOrDefault();
                AlphaVantageConnection avConn = _dataFactory.CreateAlphaVantageClient();
                string uri = $"function=TIME_SERIES_DAILY&symbol={symbol}";
                var allPrices = await avConn.GetAsync<List<AVSecurityPriceData>>(uri);
                HashSet<DateTime> existingDates = context.SecurityPriceData.Where(spd => spd.Security.Symbol == symbol).Select(spd => spd.Date).ToHashSet();
                foreach(AVSecurityPriceData price in allPrices)
                {
                    if (!existingDates.Contains(price.TimeStamp))
                    {
                        SecurityPriceStore newPrice = new SecurityPriceStore { Date = price.TimeStamp, ClosePrice = price.Close, SecurityId = symbolId };
                        context.SecurityPriceData.Add(newPrice);
                    }
                }
                await context.SaveChangesAsync();


                return allPrices;
            }
        }
    }
}
