using PortfolioAce.Domain.BusinessServices;
using PortfolioAce.Domain.DataObjects;
using PortfolioAce.EFCore.Services;
using PortfolioAce.EFCore.Services.DimensionServices;
using PortfolioAce.EFCore.Services.FactTableServices;
using PortfolioAce.EFCore.Services.PriceServices;
using PortfolioAce.ViewModels.Modals;
using PortfolioAce.Views.Modals;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace PortfolioAce.ViewModels.Factories
{
    public class WindowFactory : IWindowFactory
    {
        private IFundService _fundService;
        private ITransferAgencyService _investorService;
        private IPortfolioService _portfolioService;
        private IStaticReferences _staticReferences;
        private ITransactionService _transactionService;
        private IFactTableService _factTableService;
        private IPriceService _priceService;

        public WindowFactory(IFundService fundService,
            ITransactionService transactionService, IPortfolioService portfolioService,
            ITransferAgencyService investorService, IStaticReferences staticReferences,
            IFactTableService factTableService, IPriceService priceService)
        {
            _fundService = fundService;
            _investorService = investorService;
            _portfolioService = portfolioService;
            _transactionService = transactionService;
            _staticReferences = staticReferences;
            _factTableService = factTableService;
            _priceService = priceService;
        }

        private Window ApplyWindowAttributes(Window view, ViewModelWindowBase viewModel)
        {
            view.DataContext = viewModel;
            view.Owner = Application.Current.MainWindow;
            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (viewModel.CloseAction == null)
            {
                viewModel.CloseAction = new Action(() => view.Close());
            }
            return view;
        }


        public Window CreateEditCashTradeWindow()
        {
            throw new NotImplementedException();
        }

        public Window CreateEditTradeWindow()
        {
            throw new NotImplementedException();
        }

        public Window CreateInvestorActionWindow()
        {
            throw new NotImplementedException();
        }

        public Window CreateNewCashTradeWindow()
        {
            throw new NotImplementedException();
        }

        public Window CreateNewInvestorActionWindow()
        {
            throw new NotImplementedException();
        }

        public Window CreateNewTradeWindow()
        {
            throw new NotImplementedException();
        }

        public Window CreateFundInitialisationWindow()
        {
            throw new NotImplementedException();
        }

        public Window CreateFundMetricsWindow()
        {
            throw new NotImplementedException();
        }

        public Window CreateFundPropertiesWindow()
        {
            throw new NotImplementedException();
        }

        public Window CreateNavSummaryWindow(NavValuations navValuation)
        {
            Window view = new NavSummaryWindow();
            ViewModelWindowBase viewModel = new NavSummaryViewModel(navValuation, _investorService, _staticReferences);
            return ApplyWindowAttributes(view, viewModel);
        }

        public Window CreatePositionDetailsWindows()
        {
            throw new NotImplementedException();
        }

    }
}
