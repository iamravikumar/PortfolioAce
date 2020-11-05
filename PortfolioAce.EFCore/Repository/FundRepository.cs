using PortfolioAce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.EFCore.Repository
{
    public class FundRepository: RepositoryBase<Fund>, IFundRepository
    {
        public FundRepository(PortfolioAceDbContextFactory contextFactory)
            :base(contextFactory)
        {

        }
    }
}
