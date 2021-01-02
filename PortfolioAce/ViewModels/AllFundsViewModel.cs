using PortfolioAce.Commands;
using PortfolioAce.Domain.BusinessServices;
using PortfolioAce.Domain.DataObjects;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.Domain.Models.FactTables;
using PortfolioAce.EFCore.Services;
using PortfolioAce.EFCore.Services.DimensionServices;
using PortfolioAce.EFCore.Services.FactTableServices;
using PortfolioAce.Navigation;
using PortfolioAce.ViewModels.Modals;
using PortfolioAce.ViewModels.Windows;
using PortfolioAce.Views.Modals;
using PortfolioAce.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace PortfolioAce.ViewModels
{
    public class AllFundsViewModel : ViewModelBase
    {
        private IFundService _fundService;
        private ITransferAgencyService _investorService;
        private IPortfolioService _portfolioService;
        private IStaticReferences _staticReferences;
        private ITransactionService _transactionService;
        private IFactTableService _factTableService;
        public AllFundsViewModel(IFundService fundService,
            ITransactionService transactionService, IPortfolioService portfolioService,
            ITransferAgencyService investorService, IStaticReferences staticReferences,
            IFactTableService factTableService)
        {
            _fundService = fundService;
            _investorService = investorService;
            _portfolioService = portfolioService;
            _transactionService = transactionService;
            _staticReferences = staticReferences;
            _factTableService = factTableService;

            List<Fund> allFunds = fundService.GetAllFunds();
            _lbFunds = allFunds.Select(f => f.Symbol).ToList();
            _currentFund = (_lbFunds.Count != 0) ? _fundService.GetFund(_lbFunds[0]) : null;
            _asOfDate = (_currentFund != null)?staticReferences.GetMostRecentLockedDate(_currentFund.FundId):DateTime.Today;

            SelectFundCommand = new SelectFundCommand(this, fundService);
            
            ShowNewTradeCommand = new ActionCommand<Type, Type, ITransactionService, IStaticReferences>(
                OpenModalWindow, typeof(AddTradeWindow), typeof(AddTradeWindowViewModel), _transactionService, _staticReferences);

            ShowEditTradeCommand = new ActionCommand<Type, Type, ITransactionService, IStaticReferences>(
                OpenEditTradeWindow, typeof(EditTradeWindow), typeof(EditTradeWindowViewModel), _transactionService, _staticReferences);

            ShowDeleteTradeCommand = new ActionCommand(DeleteTradeDialog);

            ShowRestoreTradeCommand = new ActionCommand(RestoreTradeDialog);

            ShowNewCashTradeCommand = new ActionCommand<Type, Type, ITransactionService, IStaticReferences>(
                OpenModalWindow, typeof(AddCashTradeWindow), typeof(AddCashTradeWindowViewModel), _transactionService, _staticReferences);
            ShowEditCashTradeCommand = new ActionCommand<Type, Type, ITransactionService, IStaticReferences>(
                OpenEditTradeWindow, typeof(EditCashTradeWindow), typeof(EditCashTradeWindowViewModel), _transactionService, _staticReferences);
            ShowNewInvestorActionCommand = new ActionCommand<Type, Type, ITransferAgencyService, IStaticReferences>(
                OpenModalWindow, typeof(InvestorActionsWindow), typeof(InvestorActionViewModel), _investorService, _staticReferences);
            ShowFundInitialisationCommand = new ActionCommand<Type, Type, ITransferAgencyService, IStaticReferences>(
                OpenModalWindow, typeof(FundInitialisationWindow), typeof(FundInitialisationWindowViewModel), _investorService, _staticReferences);
            ShowNavSummaryCommand = new ActionCommand<Type, Type, ITransferAgencyService, IStaticReferences>(
                OpenNavSummaryWindow, typeof(NavSummaryWindow),typeof(NavSummaryViewModel), _investorService, _staticReferences);
            ShowFundPropertiesCommand = new ActionCommand<Type, Type, IFactTableService, IStaticReferences>(
                OpenModalWindow, typeof(FundPropertiesWindow), typeof(FundPropertiesViewModel), _factTableService, _staticReferences);
            ShowFundMetricsCommand = new ActionCommand<Type, Type, IFactTableService, IStaticReferences>(
                OpenMetricsWindow, typeof(FundMetricsWindow), typeof(FundMetricsViewModel), _factTableService, _staticReferences);


            PositionDetailsCommand = new PositionDetailsCommand();


        }

        public ICommand PositionDetailsCommand { get; set; }
        public ICommand SelectFundCommand { get; set; }
        public ICommand ShowNewTradeCommand { get; set; }
        public ICommand ShowEditTradeCommand { get; set; }
        public ICommand ShowRestoreTradeCommand { get; set; }
        public ICommand ShowDeleteTradeCommand { get; set; }
        public ICommand ShowNewCashTradeCommand { get; set; }
        public ICommand ShowEditCashTradeCommand { get; set; }
        public ICommand ShowFundPropertiesCommand { get; set; }
        public ICommand ShowFundInitialisationCommand { get; set; }
        public ICommand ShowNewInvestorActionCommand { get; set; }
        public ICommand ShowNavSummaryCommand { get; set; }
        public ICommand ShowFundMetricsCommand { get; set; }

        private DateTime _asOfDate;
        public DateTime asOfDate
        {
            get
            {
                return _asOfDate;
            }
            set
            {
                _asOfDate = value;
                OnPropertyChanged(nameof(asOfDate));
                OnPropertyChanged(nameof(dgFundCashHoldings));
                OnPropertyChanged(nameof(dgFundPositions));
                OnPropertyChanged(nameof(dgFundTrades));
                OnPropertyChanged(nameof(dgFundCashBook));
                OnPropertyChanged(nameof(dgFundTA));
                OnPropertyChanged(nameof(CurrentNavPrice));
                OnPropertyChanged(nameof(LockedNav));
                OnPropertyChanged(nameof(EnableFundDataMetrics));
                OnPropertyChanged(nameof(NavValuation));
                OnPropertyChanged(nameof(dgFundClients));
                OnPropertyChanged(nameof(groupedPositions));
                OnPropertyChanged(nameof(priceTable));
            }
        }

        public NavValuations NavValuation
        {
            // with this is the fund is unlocked then i can create Estimate NAV as of XXX. Estimate Nav pershare as of:
            get
            {
                return new NavValuations(dgFundPositions, dgFundCashHoldings,dgFundClients, _asOfDate, _currentFund); ;
            }
        }



        //private Dictionary<(string, DateTime), decimal> _priceTable;
        public Dictionary<(string, DateTime), decimal> priceTable
        {
            get
            {
                return _staticReferences.GetPriceTable(_asOfDate);
            }
        }
        

        // List box click should have a command and that command changes the fields displayed on the right of the allfundsview.
        private List<string> _lbFunds;
        public List<string> lbFunds
        {
            get
            {
                return _lbFunds;
            }
            set
            {
                List<Fund> allFunds = _fundService.GetAllFunds();
                _lbFunds = allFunds.Select(f => f.Symbol).ToList();
                OnPropertyChanged(nameof(lbFunds));
            }
        }

        private Fund _currentFund;
        public Fund CurrentFund
        {
            get
            {
                return (_currentFund != null)?_currentFund:null;
            }
            set
            {
                _currentFund = value;
                // all variables pass through here so that they update the view
                OnPropertyChanged(nameof(CurrentFund));
                OnPropertyChanged(nameof(dgFundPositions));
                OnPropertyChanged(nameof(dgFundCashHoldings));
                OnPropertyChanged(nameof(dgFundTrades));
                OnPropertyChanged(nameof(dgFundCashBook));
                OnPropertyChanged(nameof(dgFundTA));
                OnPropertyChanged(nameof(IsInitialised));
                OnPropertyChanged(nameof(CurrentNavPrice));
                OnPropertyChanged(nameof(ShowWidget));
                OnPropertyChanged(nameof(LockedNav));
                OnPropertyChanged(nameof(EnableFundDataMetrics));
                OnPropertyChanged(nameof(NavValuation));
                OnPropertyChanged(nameof(dgFundClients));
                OnPropertyChanged(nameof(groupedPositions));
            }
        }

        
        public bool IsInitialised
        {
            // need to find a way to incorporate no fund option so it doesn't break
            get
            {
                return (_currentFund != null) ? _currentFund.IsInitialised : false;
            }
            private set
            {

            }
        }

        public NAVPriceStoreFACT CurrentNavPrice
        {
            get
            {
                return (_currentFund != null) ? _currentFund.NavPrices.Where(np=>np.FinalisedDate==_asOfDate).FirstOrDefault():null; //maybe nav periods...
            }
            private set
            {

            }
        }

        public Visibility LockedNav
        {
            // true = this will put a green tick is the nav is locked which means the price and amount you see are finalised...
            // false = this will put a red cross if the nav is unlocked which means the price you see is not yet final
            get
            {
                if(_currentFund != null)
                {
                    AccountingPeriodsDIM period = _currentFund.NavPrices.Where(np => np.NAVPeriod.AccountingDate == _asOfDate).Select(np=>np.NAVPeriod).FirstOrDefault();
                    if(period != null)
                    {
                        return (period.isLocked) ? Visibility.Visible : Visibility.Collapsed;
                    }
                }
                return Visibility.Collapsed;
            }
            private set
            {
            }
        }

        public bool EnableFundDataMetrics
        {
            // true = this will put a green tick is the nav is locked which means the price and amount you see are finalised...
            // false = this will put a red cross if the nav is unlocked which means the price you see is not yet final
            get
            {
                if (_currentFund != null)
                {
                    AccountingPeriodsDIM period = _currentFund.NavPrices.Where(np => np.NAVPeriod.AccountingDate == _asOfDate).Select(np => np.NAVPeriod).FirstOrDefault();
                    if (period != null)
                    {
                        return (period.isLocked);
                    }
                }
                return false;
            }
        }

        public Visibility ShowWidget
        {
            get
            {
                return (IsInitialised)?Visibility.Visible: Visibility.Collapsed;
            }
            private set
            {

            }
        }
        
        public List<CashPositionValuation> dgFundCashHoldings
        {
            get
            {
                if(_currentFund != null)
                {
                    List<CalculatedCashPosition> cashPositions = _portfolioService.GetAllCashBalances(_currentFund, _asOfDate);
                    List<CashPositionValuation> valuedCashPositions = new List<CashPositionValuation>();
                    foreach(CalculatedCashPosition cashPosition in cashPositions)
                    {
                        CashPositionValuation valuedCashPosition = new CashPositionValuation(cashPosition, priceTable, _asOfDate, _currentFund.BaseCurrency);
                        valuedCashPositions.Add(valuedCashPosition);
                    }
                    return valuedCashPositions;
                }
                else
                {
                    return new List<CashPositionValuation>();
                }
            }
        }

        public List<ClientHolding> dgFundClients
        {
            get
            {
                if (_currentFund != null)
                {
                    List<ClientHolding> allClientHoldings = _portfolioService.GetAllClientHoldings(_currentFund, asOfDate);
                    return allClientHoldings; // This is temporary until I make the NaValuation live in this view...
                }
                else
                {
                    return new List<ClientHolding>();
                }
            }
        }

        public List<SecurityPositionValuation> dgFundPositions
        {
            get
            {
                if (_currentFund != null)
                {
                    List<CalculatedSecurityPosition> allPositions = _portfolioService.GetAllSecurityPositions(_currentFund, _asOfDate);
                    List<SecurityPositionValuation> valuedPositions = new List<SecurityPositionValuation>();

                    foreach (CalculatedSecurityPosition position in allPositions)
                    {
                        SecurityPositionValuation valuedPosition = new SecurityPositionValuation(position, priceTable, _asOfDate, _currentFund.BaseCurrency);
                        valuedPositions.Add(valuedPosition);
                    }
                    return valuedPositions;
                }
                else
                {
                    return new List<SecurityPositionValuation>();
                }
            }
        }
        private ListCollectionView _groupedPositions;
        public ListCollectionView groupedPositions
        {
            get
            {
                ListCollectionView cv = new ListCollectionView(dgFundPositions);
                cv.GroupDescriptions.Add(new PropertyGroupDescription("Position.security.AssetClass"));
                return cv;
            }
        }


        public List<TransactionsBO> dgFundTrades
        {
            get
            {
                // Transactions filtered for just security trades
                return (_currentFund != null) ? _currentFund.Transactions
                                                            .Where(t => t.TransactionType.TypeClass.ToString() == "SecurityTrade" && t.TradeDate <= _asOfDate)
                                                            .OrderBy(t=>t.TradeDate)
                                                            .ToList() : null;
            }
        }

        private TransactionsBO _selectedTransaction;
        public TransactionsBO selectedTransaction
        {
            get
            {
                return _selectedTransaction;
            }
            set
            {
                _selectedTransaction = value;
                OnPropertyChanged(nameof(selectedTransaction));
            }
        }

        public List<TransactionsBO> dgFundCashBook
        {
            get
            {
                // all Transactions ordered by date... 
                return (_currentFund != null) ? _currentFund.Transactions
                                                            .Where(t=>t.TradeDate<=_asOfDate)
                                                            .OrderBy(t => t.TradeDate)
                                                            .ToList() : null;
            }
        }


        public List<TransferAgencyBO> dgFundTA
        {
            get
            {
                return (_currentFund != null) ? _currentFund.TransferAgent
                                                            .Where(ta=>ta.TransactionDate<=_asOfDate)
                                                            .OrderBy(ta => ta.TransactionDate)
                                                            .ToList() : null;
            }
        }

        public void OpenModalWindow(Type windowType, Type viewModelType, object myService, object myService2)
        {
            int fundId = _currentFund.FundId;
            Window view = (Window)Activator.CreateInstance(windowType);
            ViewModelWindowBase viewModel = (ViewModelWindowBase)Activator.CreateInstance(viewModelType, myService, myService2, _currentFund);
            
            view.DataContext = viewModel;
            view.Owner = Application.Current.MainWindow;
            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (viewModel.CloseAction == null)
            {
                viewModel.CloseAction = new Action(() => view.Close());
            }
            view.ShowDialog();
        }

        public void OpenMetricsWindow(Type windowType, Type viewModelType, object myService, object myService2)
        {
            int fundId = _currentFund.FundId;
            Window view = (Window)Activator.CreateInstance(windowType);
            ViewModelWindowBase viewModel = (ViewModelWindowBase)Activator.CreateInstance(viewModelType, myService, myService2, _currentFund, _asOfDate);

            view.DataContext = viewModel;
            view.Owner = Application.Current.MainWindow;
            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (viewModel.CloseAction == null)
            {
                viewModel.CloseAction = new Action(() => view.Close());
            }
            view.ShowDialog();
        }

        public void OpenNavSummaryWindow(Type windowType, Type viewModelType, object myService, object myService2)
        {
            // This is temporary
            Window view = (Window)Activator.CreateInstance(windowType);
            ViewModelWindowBase viewModel = (ViewModelWindowBase)Activator.CreateInstance(viewModelType, NavValuation, myService, myService2);

            view.DataContext = viewModel;
            view.Owner = Application.Current.MainWindow;
            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (viewModel.CloseAction == null)
            {
                viewModel.CloseAction = new Action(() => view.Close());
            }
            view.ShowDialog();
        }

        public void OpenEditTradeWindow(Type windowType, Type viewModelType, object myService, object myService2)
        {
            // This is temporary
            Window view = (Window)Activator.CreateInstance(windowType);
            ViewModelWindowBase viewModel = (ViewModelWindowBase)Activator.CreateInstance(viewModelType, myService, myService2, _selectedTransaction);

            view.DataContext = viewModel;
            view.Owner = Application.Current.MainWindow;
            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (viewModel.CloseAction == null)
            {
                viewModel.CloseAction = new Action(() => view.Close());
            }
            view.ShowDialog();
        }

        public void DeleteTradeDialog()
        {
            string n = Environment.NewLine;
            string message;
            DateTime tradeDate = _selectedTransaction.TradeDate;
            int tradeFundId = _selectedTransaction.FundId;
            if (!_selectedTransaction.isCashTransaction)
            {
                string secSymbol = _selectedTransaction.Security.Symbol;
                string secName = _selectedTransaction.Security.SecurityName;
                decimal quantity = _selectedTransaction.Quantity;
                decimal price = _selectedTransaction.Price;
                message = $"Are you sure you want to delete the following Trade:{n}Security: {secName} ({secSymbol}){n}Quantity: {quantity}{n}Price: {price}";
            }
            else
            {
                string tradeType = _selectedTransaction.TransactionType.TypeName.ToString(); 
                string cashSymbol = _selectedTransaction.Currency.Symbol.ToString();
                decimal amount = _selectedTransaction.TradeAmount;
                message = $"Are you sure you want to delete the following Trade:{n}Type: {tradeType}{n}Security: {cashSymbol}{n}Transaction Amount: {amount}{n}";
            }
            MessageBoxResult result =  MessageBox.Show(message, "Delete Trade", button: MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    if (!_staticReferences.GetPeriod(tradeDate, tradeFundId).isLocked)
                    {
                        _transactionService.DeleteTransaction(selectedTransaction);
                    }
                    else
                    {
                        MessageBox.Show($"To Delete this trade the period of {tradeDate.ToString("dd-MM-yyyy")} must be unlocked", "Unable to Restore");
                    }
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        public void RestoreTradeDialog()
        {
            DateTime tradeDate = _selectedTransaction.TradeDate;
            int tradeFundId = _selectedTransaction.FundId;
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to restore this trade?", "Restore Trade", button: MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    if (!_staticReferences.GetPeriod(tradeDate, tradeFundId).isLocked)
                    {
                        _transactionService.RestoreTransaction(selectedTransaction);
                    }
                    else
                    {
                        MessageBox.Show($"To Restore this trade the period of {tradeDate.ToString("dd-MM-yyyy")} must be unlocked", "Unable to Restore");
                    }
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
    }
}
