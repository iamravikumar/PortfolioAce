using PortfolioAce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.EFCore.Services.SettingServices
{
    public interface ISettingService
    {
        Dictionary<string, ApplicationSettings> GetAllSettings();
    }
}
