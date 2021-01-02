using Microsoft.EntityFrameworkCore;
using PortfolioAce.DataCentre.APIConnections;
using PortfolioAce.DataCentre.DeserialisedObjects;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.Dimensions;
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


        public async Task<List<AVSecurityPriceData>> AddDailyPrices(SecuritiesDIM security)
        {
            // This is for Equity Prices
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                string avKey = context.AppSettings.Where(ap => ap.SettingName == "AlphaVantageAPI").First().SettingValue;
                AlphaVantageConnection avConn = _dataFactory.CreateAlphaVantageClient(avKey);
                string uri = GenerateURI(security);
                var allPrices = await avConn.GetAsync<List<AVSecurityPriceData>>(uri);
                HashSet<DateTime> existingDates = context.SecurityPriceData.Where(spd => spd.Security.Symbol == security.Symbol).Select(spd => spd.Date).ToHashSet();

                foreach(AVSecurityPriceData price in allPrices)
                {
                    if (!existingDates.Contains(price.TimeStamp))
                    {
                        SecurityPriceStore newPrice = new SecurityPriceStore { Date = price.TimeStamp, ClosePrice = price.Close, SecurityId = security.SecurityId};
                        context.SecurityPriceData.Add(newPrice);
                    }
                }
                await context.SaveChangesAsync();

                return allPrices;
            }
        }
        private string GenerateURI(SecuritiesDIM security)
        {
            string assetClass = security.AssetClass.Name.ToString();
            string symbol = security.Symbol;
            string uri;
            
            if (assetClass == "FX")
            {
                string from_symbol = symbol.Substring(0,3);
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

        public SecuritiesDIM GetSecurityInfo(string symbol)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Securities.Where(s => s.Symbol == symbol).Include(s => s.Currency).Include(s => s.AssetClass).FirstOrDefault();
            }
        }

        public HashSet<string> GetAllPricedSecuritySymbols()
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.SecurityPriceData.Select(spd=>spd.Security.Symbol).ToHashSet();
            }
        }

        public List<SecurityPriceStore> GetSecurityPrices(string symbol)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.SecurityPriceData.Where(spd => spd.Security.Symbol == symbol).OrderBy(spd=>spd.Date).ToList();
            }
        }
    }
}
