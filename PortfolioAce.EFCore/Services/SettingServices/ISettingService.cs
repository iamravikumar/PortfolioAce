using PortfolioAce.Domain.Models;
using System.Collections.Generic;

namespace PortfolioAce.EFCore.Services.SettingServices
{
    public interface ISettingService : IBaseService
    {
        Dictionary<string, ApplicationSettings> GetAllSettings();

        void UpdateAPIKeys(string alphaVantageKeyValue, string FMPKeyValue); // I should implement Hashed API Keys
    }
}