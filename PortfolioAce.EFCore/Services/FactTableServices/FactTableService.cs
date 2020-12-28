using System;
using System.Collections.Generic;
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
    }
}
