using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services.SettingServices
{
    public interface IImportService:IBaseService
    {


        Dictionary<string, int> CurrencyMap();
        Dictionary<string, int> AssetClassMap();
        Dictionary<string, int> SecurityMap();

        Task AddImportedPrices(Dictionary<string, List<SecurityPriceStore>> newPrices);
        Task AddImportedSecurities(List<SecuritiesDIM> newSecurities);

    }
}
