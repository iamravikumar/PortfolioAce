using PortfolioAce.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services.SettingServices
{
    public interface ISettingService : IBaseService
    {
        Dictionary<string, ApplicationSettings> GetAllSettings();

        Task UpdateAPIKeys(string alphaVantageKeyValue, string FMPKeyValue); // I should implement Hashed API Keys
    }
}