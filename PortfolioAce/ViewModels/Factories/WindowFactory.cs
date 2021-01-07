using PortfolioAce.Domain.BusinessServices;
using PortfolioAce.Domain.DataObjects;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.BackOfficeModels;
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

        public void CreateNavSummaryWindow(NavValuations navValuation)
        {
            Window view = new NavSummaryWindow();
            ViewModelWindowBase viewModel = new NavSummaryViewModel(navValuation, _investorService, _staticReferences);
            view = ApplyWindowAttributes(view, viewModel);
            view.Show();
        }

        public void CreateNewTradeWindow(Fund fund)
        {
            Window view = new AddTradeWindow();
            ViewModelWindowBase viewModel = new AddTradeWindowViewModel(_transactionService, _staticReferences, fund);
            view = ApplyWindowAttributes(view, viewModel);
            view.Show();
        }

        public void CreateEditTradeWindow(TransactionsBO securityTrade)
        {
            Window view = new EditTradeWindow();
            ViewModelWindowBase viewModel = new EditTradeWindowViewModel(_transactionService, _staticReferences,securityTrade);
            view = ApplyWindowAttributes(view, viewModel);
            view.Show();
        }

        public void CreateNewCashTradeWindow(Fund fund)
        {
            Window view = new AddCashTradeWindow();
            ViewModelWindowBase viewModel = new AddCashTradeWindowViewModel(_transactionService, _staticReferences, fund);
            view = ApplyWindowAttributes(view, viewModel);
            view.Show();
        }

        public void CreateEditCashTradeWindow(TransactionsBO cashTrade)
        {
            Window view = new EditCashTradeWindow();
            ViewModelWindowBase viewModel = new EditCashTradeWindowViewModel(_transactionService, _staticReferences, cashTrade);
            view = ApplyWindowAttributes(view, viewModel);
            view.Show();
        }

        public void CreateNewInvestorActionWindow(Fund fund)
        {
            Window view = new InvestorActionsWindow();
            ViewModelWindowBase viewModel = new InvestorActionViewModel(_investorService, _staticReferences, fund);
            view = ApplyWindowAttributes(view, viewModel);
            view.Show();
        }

        public void CreateFundInitialisationWindow(Fund fund)
        {
            Window view = new FundInitialisationWindow();
            ViewModelWindowBase viewModel = new InvestorActionViewModel(_investorService, _staticReferences, fund);
            view = ApplyWindowAttributes(view, viewModel);
            view.Show();
        }

        public void CreateFundPropertiesWindow(Fund fund)
        {
            Window view = new FundPropertiesWindow();
            ViewModelWindowBase viewModel = new FundPropertiesViewModel(_factTableService, _staticReferences,fund);
            view = ApplyWindowAttributes(view, viewModel);
            view.Show();
        }

        public void CreateFundMetricsWindow(Fund fund, DateTime date)
        {
            Window view = new FundMetricsWindow();
            ViewModelWindowBase viewModel = new FundMetricsViewModel(_factTableService, _staticReferences, fund, date);
            view = ApplyWindowAttributes(view, viewModel);
            view.Show();
        }

        public void CreatePositionDetailsWindows(SecurityPositionValuation position, Fund fund)
        {
            Window view = new PositionDetailWindow();
            ViewModelWindowBase viewModel = new PositionDetailWindowViewModel(_priceService, _factTableService, position, fund);
            view = ApplyWindowAttributes(view, viewModel);
            view.Show();
        }
    }
}
