using PortfolioAce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.EFCore.Services.SettingServices
{
    public interface ISettingService
    {
        Dictionary<string, ApplicationSettings> GetAllSettings();

        void UpdateAPIKeys(string alphaVantageKeyValue, string FMPKeyValue); // I should implement Hashed API Keys
    }
}
