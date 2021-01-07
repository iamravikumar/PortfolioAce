using PortfolioAce.Domain.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace PortfolioAce.ViewModels.Factories
{
    public interface IWindowFactory
    {
        // Firstly this wouldnt need to take service as parameters and the window/viewmodels
        // i need to figure out how to
        // This replaces the previous commands I can't use generics here because different services are required...
        Window CreateNewTradeWindow();
        Window CreateEditTradeWindow();
        Window CreateNewCashTradeWindow();
        Window CreateEditCashTradeWindow();
        Window CreateNewInvestorActionWindow();
        
        Window CreateFundInitialisationWindow();
        Window CreateNavSummaryWindow(NavValuations navValuation);
        Window CreateFundPropertiesWindow();
        Window CreateFundMetricsWindow();
        Window CreatePositionDetailsWindows();

    }
}
