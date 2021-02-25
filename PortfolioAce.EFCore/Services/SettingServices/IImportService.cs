using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.EFCore.Services.SettingServices
{
    public interface IImportService:IBaseService
    {


        Dictionary<string, int> CurrencyMap();
        Dictionary<string, int> AssetClassMap();
        Dictionary<string, int> SecurityMap();

        void AddImportedPrices(Dictionary<string, List<SecurityPriceStore>> newPrices);
        void AddImportedSecurities(List<SecuritiesDIM> newSecurities);

    }
}
