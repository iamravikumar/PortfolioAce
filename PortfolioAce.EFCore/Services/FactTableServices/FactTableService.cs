using Microsoft.EntityFrameworkCore;
using PortfolioAce.Domain.Models.FactTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioAce.EFCore.Services.FactTableServices
{
    public class FactTableService:IFactTableService
    {
        private readonly PortfolioAceDbContextFactory _contextFactory;


        public FactTableService(PortfolioAceDbContextFactory contextFactory)
        {
            this._contextFactory = contextFactory;
        }

        public List<PositionFACT> GetAllStoredPositions(DateTime date, int FundId, bool onlyActive = false)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                if (onlyActive)
                {
                    return context.Positions.Where(p=>p.Quantity!=0 && p.FundId ==FundId && p.PositionDate== date)
                        .Include(p => p.AssetClass)
                        .Include(p => p.Security)
                        .ToList();
                }
                else
                {
                    return context.Positions.Where(p => p.FundId == FundId && p.PositionDate == date)
                        .Include(p => p.AssetClass)
                        .Include(p => p.Security)
                        .ToList();
                }


            }
        }
    }
}
