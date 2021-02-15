using PortfolioAce.Domain.DataObjects;
using PortfolioAce.Domain.DataObjects.PositionData;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.EFCore.Services;
using PortfolioAce.EFCore.Services.DimensionServices;
using PortfolioAce.EFCore.Services.FactTableServices;
using PortfolioAce.EFCore.Services.PriceServices;
using PortfolioAce.EFCore.Services.SettingServices;
using PortfolioAce.ViewModels.Modals;
using PortfolioAce.ViewModels.Windows;
using PortfolioAce.Views.Modals;
using PortfolioAce.Views.Windows;
using System;
using System.Windows;

namespace PortfolioAce.ViewModels.Factories
{
    public class WindowFactory : IWindowFactory
    {
        private IFundService _fundService;
        private ITransferAgencyService _investorService;
        private IStaticReferences _staticReferences;
        private ITransactionService _transactionService;
        private IFactTableService _factTableService;
        private IPriceService _priceService;
        private IAdminService _adminService;
        private ISettingService _settingService;

        public WindowFactory(IFundService fundService,
            ITransactionService transactionService,
            IAdminService adminService, ISettingService settingService,
            ITransferAgencyService investorService, IStaticReferences staticReferences,
            IFactTableService factTableService, IPriceService priceService)
        {
            _fundService = fundService;
            _investorService = investorService;
            _transactionService = transactionService;
            _adminService = adminService;
            _settingService = settingService;
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
            view.ShowDialog();
        }

        public void CreateNewTradeWindow(Fund fund)
        {
            Window view = new AddTradeWindow();
            ViewModelWindowBase viewModel = new AddTradeWindowViewModel(_transactionService, _staticReferences, fund);
            view = ApplyWindowAttributes(view, viewModel);
            view.ShowDialog();
        }

        public void CreateEditTradeWindow(TransactionsBO securityTrade)
        {
            Window view = new EditTradeWindow();
            ViewModelWindowBase viewModel = new EditTradeWindowViewModel(_transactionService, _staticReferences, securityTrade);
            view = ApplyWindowAttributes(view, viewModel);
            view.ShowDialog();
        }

        public void CreateNewCashTradeWindow(Fund fund)
        {
            Window view = new AddCashTradeWindow();
            ViewModelWindowBase viewModel = new AddCashTradeWindowViewModel(_transactionService, _staticReferences, fund);
            view = ApplyWindowAttributes(view, viewModel);
            view.ShowDialog();
        }

        public void CreateEditCashTradeWindow(TransactionsBO cashTrade)
        {
            Window view = new EditCashTradeWindow();
            ViewModelWindowBase viewModel = new EditCashTradeWindowViewModel(_transactionService, _staticReferences, cashTrade);
            view = ApplyWindowAttributes(view, viewModel);
            view.ShowDialog();
        }

        public void CreateNewInvestorActionWindow(Fund fund)
        {
            Window view = new InvestorActionsWindow();
            ViewModelWindowBase viewModel = new InvestorActionViewModel(_investorService, _staticReferences, fund);
            view = ApplyWindowAttributes(view, viewModel);
            view.ShowDialog();
        }

        public void CreateFundInitialisationWindow(Fund fund)
        {
            Window view = new FundInitialisationWindow();
            ViewModelWindowBase viewModel = new FundInitialisationWindowViewModel(_investorService, _staticReferences, fund);
            view = ApplyWindowAttributes(view, viewModel);
            view.ShowDialog();
        }

        public void CreateFundPropertiesWindow(Fund fund)
        {
            Window view = new FundPropertiesWindow();
            ViewModelWindowBase viewModel = new FundPropertiesViewModel(_factTableService, _staticReferences, fund);
            view = ApplyWindowAttributes(view, viewModel);
            view.ShowDialog();
        }

        public void CreateFundMetricsWindow(Fund fund, DateTime date)
        {
            Window view = new FundMetricsWindow();
            ViewModelWindowBase viewModel = new FundMetricsViewModel(_factTableService, _staticReferences, fund, date);
            view = ApplyWindowAttributes(view, viewModel);
            view.ShowDialog();
        }

        public void CreatePositionDetailsWindows(ValuedSecurityPosition position, Fund fund)
        {
            Window view = new PositionDetailWindow();
            ViewModelWindowBase viewModel = new PositionDetailWindowViewModel(_priceService, _factTableService, position, fund);
            view = ApplyWindowAttributes(view, viewModel);
            view.ShowDialog();
        }

        public void CreateImportTradesWindow()
        {
            Window view = new ImportTradesWindow();
            ViewModelWindowBase viewModel = new ImportTradesViewModel();
            view = ApplyWindowAttributes(view, viewModel);
            view.ShowDialog();
        }

        public void CreateSettingsWindow()
        {
            Window view = new SettingsWindow();
            ViewModelWindowBase viewModel = new SettingsWindowViewModel(_settingService);
            view = ApplyWindowAttributes(view, viewModel);
            view.ShowDialog();
        }

        public void CreateNewFundWindow()
        {
            Window view = new AddFundWindow();
            ViewModelWindowBase viewModel = new AddFundWindowViewModel(_fundService, _staticReferences);
            view = ApplyWindowAttributes(view, viewModel);
            view.ShowDialog();
        }

        public void CreateNewInvestorWindow()
        {
            Window view = new InvestorManagerWindow();
            ViewModelWindowBase viewModel = new InvestorManagerWindowViewModel(_investorService);
            view = ApplyWindowAttributes(view, viewModel);
            view.ShowDialog();
        }

        public void CreateSecurityManagerWindow()
        {
            Window view = new SecurityManagerWindow();
            ViewModelWindowBase viewModel = new SecurityManagerWindowViewModel(_adminService, _staticReferences);
            view = ApplyWindowAttributes(view, viewModel);
            view.ShowDialog();
        }

        public void CreateAboutWindow()
        {
            Window view = new AboutWindow();
            ViewModelWindowBase viewModel = new AboutWindowViewModel();
            view = ApplyWindowAttributes(view, viewModel);
            view.ShowDialog();
        }

        public void CreateNewFXTradeWindow(Fund fund)
        {
            Window view = new AddFXTradeWindow();
            ViewModelWindowBase viewModel = new AddFXTradeWindowViewModel(_transactionService, _staticReferences, fund);
            view = ApplyWindowAttributes(view, viewModel);
            view.ShowDialog();
        }
    }
}
