using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore.Services.DimensionServices;
using PortfolioAce.EFCore.Services.FactTableServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.ViewModels.Modals
{
    public class FundMetricsViewModel: ViewModelWindowBase
    {
        private IFactTableService _factTableService;
        private IStaticReferences _staticReferences;
        private Fund _fund;
        public FundMetricsViewModel(IFactTableService factTableService, IStaticReferences staticReferences, Fund fund)
        {
            _factTableService = factTableService;
            _staticReferences = staticReferences;
            _fund = fund;
        }
    }
}
