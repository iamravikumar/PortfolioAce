using PortfolioAce.Domain.DataObjects;
using PortfolioAce.Domain.DataObjects.PositionData;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.BackOfficeModels;
using System;

namespace PortfolioAce.ViewModels.Factories
{
    public interface IWindowFactory
    {
        // I can't use generics here because different viewmodels require different services...
        void CreateNewTradeWindow(Fund fund);
        void CreateEditTradeWindow(TransactionsBO securityTrade);

        void CreateNewFXTradeWindow(Fund fund);
        void CreateNewCashTradeWindow(Fund fund);
        void CreateEditCashTradeWindow(TransactionsBO cashTrade);
        void CreateNewInvestorActionWindow(Fund fund);

        void CreateFundInitialisationWindow(Fund fund);
        void CreateNavSummaryWindow(NavValuations navValuation);
        void CreateFundPropertiesWindow(Fund fund);
        void CreateFundMetricsWindow(Fund fund, DateTime date);
        void CreatePositionDetailsWindows(ValuedSecurityPosition position, Fund fund);


        void CreateImportTradesWindow();
        void CreateSettingsWindow();
        void CreateNewFundWindow();
        void CreateNewInvestorWindow();
        void CreateSecurityManagerWindow();
        void CreateAboutWindow();
    }
}
