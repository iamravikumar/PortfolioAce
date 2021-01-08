﻿using PortfolioAce.Domain.DataObjects;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.BackOfficeModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace PortfolioAce.ViewModels.Factories
{
    public interface IWindowFactory
    {
        // I can't use generics here because different viewmodels require different services...
        void CreateNewTradeWindow(Fund fund);
        void CreateEditTradeWindow(TransactionsBO securityTrade);
        void CreateNewCashTradeWindow(Fund fund);
        void CreateEditCashTradeWindow(TransactionsBO cashTrade);
        void CreateNewInvestorActionWindow(Fund fund);
        
        void CreateFundInitialisationWindow(Fund fund);
        void CreateNavSummaryWindow(NavValuations navValuation);
        void CreateFundPropertiesWindow(Fund fund);
        void CreateFundMetricsWindow(Fund fund, DateTime date);
        void CreatePositionDetailsWindows(SecurityPositionValuation position, Fund fund);


        void CreateImportTradesWindow();
        void CreateSettingsWindow();
        void CreateNewFundWindow();
        void CreateNewInvestorWindow();
        void CreateSecurityManagerWindow();
        void CreateAboutWindow();
    }
}