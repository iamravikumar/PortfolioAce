﻿using Microsoft.EntityFrameworkCore;
using PortfolioAce.DataCentre.APIConnections;
using PortfolioAce.DataCentre.DeserialisedObjects;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.Linq;
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


        public async Task<int> AddDailyPrices(SecuritiesDIM security)
        {
            
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                string avKey = context.AppSettings.Where(ap => ap.SettingName == "AlphaVantageAPI").First().SettingValue;
                AlphaVantageConnection avConn = _dataFactory.CreateAlphaVantageClient(avKey);
                IEnumerable<AVSecurityPriceData> allPrices = await avConn.GetPricesAsync(security);
                HashSet<DateTime> existingDates = context.SecurityPriceData.Where(spd => spd.Security.Symbol == security.Symbol).Select(spd => spd.Date).ToHashSet();
                string assetClass = security.AssetClass.Name;
                int pricesSaved = 0;
                foreach (AVSecurityPriceData price in allPrices)
                {
                    if (!existingDates.Contains(price.TimeStamp))
                    {
                        // i should the indirect quote therefore i inverse the price here...
                        if (assetClass == "FX")
                        {
                            price.Close = 1 / price.Close;
                        }
                        if (security.Currency.Symbol == "GBP" && assetClass !="FX")
                        {
                            price.Close /= 100;
                        }
                        SecurityPriceStore newPrice = new SecurityPriceStore { Date = price.TimeStamp, ClosePrice = price.Close, SecurityId = security.SecurityId, PriceSource = price.PriceSource };
                        context.SecurityPriceData.Add(newPrice);
                        pricesSaved += 1;
                    }
                }
                await context.SaveChangesAsync();

                return pricesSaved;
            }
        }


        public SecuritiesDIM GetSecurityInfo(string symbol, string assetClass)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Securities.AsNoTracking().Where(s => s.Symbol == symbol && s.AssetClass.Name== assetClass).Include(s => s.Currency).Include(s => s.AssetClass).FirstOrDefault();
            }
        }

        public List<SecurityPriceStore> GetSecurityPrices(string symbol, string assetClass)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.SecurityPriceData.Where(spd => spd.Security.Symbol == symbol).Include(spd=>spd.Security.AssetClass).OrderBy(spd => spd.Date).ToList();
            }
        }

        public async Task AddManualPrices(List<SecurityPriceStore> prices)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                context.SecurityPriceData.AddRange(prices);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateManualPrices(List<SecurityPriceStore> prices)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                context.SecurityPriceData.UpdateRange(prices);
                await context.SaveChangesAsync();
            }
        }
    }
}
