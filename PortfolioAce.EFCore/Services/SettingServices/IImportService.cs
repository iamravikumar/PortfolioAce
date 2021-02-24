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
    }
}
