using PortfolioAce.Commands;
using PortfolioAce.Domain.BusinessServices;
using PortfolioAce.Domain.DataObjects;
using PortfolioAce.Domain.Handlers;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.FactTables;
using PortfolioAce.EFCore.Services;
using PortfolioAce.EFCore.Services.DimensionServices;
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
        public AllFundsViewModel(IFundService fundService,
            ITransactionService transactionService, IPortfolioService portfolioService,
            ITransferAgencyService investorService, IStaticReferences staticReferences)
        {
            _fundService = fundService;
            _investorService = investorService;
            _portfolioService = portfolioService;
            _transactionService = transactionService;
            _staticReferences = staticReferences;

            List<Fund> allFunds = fundService.GetAllFunds();
            _lbFunds = allFunds.Select(f => f.Symbol).ToList();
            _currentFund = (_lbFunds.Count != 0) ? _fundService.GetFund(_lbFunds[0]) : null;

            SelectFundCommand = new SelectFundCommand(this, fundService);
            
            ShowNewTradeCommand = new ActionCommand<Type, Type, ITransactionService, IStaticReferences>(
                OpenModalWindow, typeof(AddTradeWindow), typeof(AddTradeWindowViewModel), _transactionService, _staticReferences);
            ShowNewCashTradeCommand = new ActionCommand<Type, Type, ITransactionService, IStaticReferences>(
                OpenModalWindow, typeof(AddCashTradeWindow), typeof(AddCashTradeWindowViewModel), _transactionService, _staticReferences);
            ShowNewInvestorActionCommand = new ActionCommand<Type, Type, ITransferAgencyService, IStaticReferences>(
                OpenModalWindow, typeof(InvestorActionsWindow), typeof(InvestorActionViewModel), _investorService, _staticReferences);
            ShowFundInitialisationCommand = new ActionCommand<Type, Type, ITransferAgencyService, IStaticReferences>(
                OpenModalWindow, typeof(FundInitialisationWindow), typeof(FundInitialisationWindowViewModel), _investorService, _staticReferences);
            PositionDetailCommand = new ActionCommand(ViewPositionDetails);
        }

        public ICommand SelectFundCommand { get; set; }
        public ICommand ShowNewTradeCommand { get; set; }
        public ICommand ShowNewCashTradeCommand { get; set; }

        public ICommand ShowFundInitialisationCommand { get; set; }
        public ICommand ShowNewInvestorActionCommand { get; set; }

        public ICommand PositionDetailCommand { get; set; }


        private PositionHandler _dtPositionObject;
        public PositionHandler dtPositionObject
        {
            // This object will open a window that will display the position information such as currency, direction, ITD realised pnl, open lots,
            // positionbreakdown
            get
            {
                return _dtPositionObject;
            }
            set
            {
                _dtPositionObject = value;
                OnPropertyChanged(nameof(dtPositionObject));
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
                //OnPropertyChanged(nameof(dgFundCashHoldings));
                OnPropertyChanged(nameof(dgFundTrades));
                OnPropertyChanged(nameof(dgFundCashBook));
                OnPropertyChanged(nameof(dgFundTA));
                OnPropertyChanged(nameof(IsInitialised));
                OnPropertyChanged(nameof(LatestNav));
                OnPropertyChanged(nameof(ShowWidget));
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

        public NAVPriceStoreFACT LatestNav
        {
            get
            {
                return (_currentFund != null) ? _currentFund.NavPrices.OrderByDescending(cf=>cf.FinalisedDate).FirstOrDefault():null;
            }
            private set
            {

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
        /*
        private List<CashAccountBalance> _dgFundCashHoldings;
        public List<CashAccountBalance> dgFundCashHoldings
        {
            get
            {
                if (_currentFund != null)
                {
                    CashHoldings holdings = _portfolioService.GetAllCashBalances(_currentFund);
                    return holdings.GetCashBalances();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                CashHoldings holdings = _portfolioService.GetAllCashBalances(_currentFund); 
                _dgFundCashHoldings = holdings.GetCashBalances();
                OnPropertyChanged(nameof(dgFundCashHoldings));
            }
        }
        */
        private List<PositionHandler> _dgFundPositions;
        public List<PositionHandler> dgFundPositions
        {
            get
            {
                return (_currentFund!=null)?_portfolioService.GetAllPositions(_currentFund):null;
            }
            set
            {
                _dgFundPositions = _portfolioService.GetAllPositions(_currentFund);
                OnPropertyChanged(nameof(dgFundPositions));
            }
        }

        private List<TransactionsBO> _dgFundTrades;
        public List<TransactionsBO> dgFundTrades
        {
            get
            {
                // Transactions filtered for just security trades
                return (_currentFund != null) ? _currentFund.Transactions.Where(t => t.TransactionType.TypeClass.ToString() == "SecurityTrade").OrderBy(t=>t.TradeDate).ToList() : null;
            }
            set
            {
                _dgFundTrades = _currentFund.Transactions.Where(t => t.TransactionType.TypeClass.ToString() == "SecurityTrade").OrderBy(t => t.TradeDate).ToList();
                OnPropertyChanged(nameof(dgFundTrades));
            }
        }

        private List<TransactionsBO> _dgFundCashBook;
        public List<TransactionsBO> dgFundCashBook
        {
            get
            {
                // all Transactions ordered by date... 
                return (_currentFund != null) ? _currentFund.Transactions.OrderBy(t => t.TradeDate).ToList() : null;
            }
            set
            {
                _dgFundCashBook = _currentFund.Transactions.OrderBy(t => t.TradeDate).ToList();
                OnPropertyChanged(nameof(dgFundCashBook));
            }
        }
        /*
        private List<TradeBO> _dgFundTrades;
        public List<TradeBO> dgFundTrades
        {
            get
            {
                return (_currentFund != null) ?_currentFund.Trades.OrderBy(t=>t.TradeDate).ToList() : null;
            }
            set
            {
                _dgFundTrades = _currentFund.Trades.OrderBy(t => t.TradeDate).ToList();
                OnPropertyChanged(nameof(dgFundTrades));
            }
        }

        private List<CashBookBO> _dgFundCashBook;
        public List<CashBookBO> dgFundCashBook
        {
            get
            {
                return (_currentFund != null) ? _currentFund.CashBooks.OrderBy(t => t.TransactionDate).ToList() : null;
            }
            set
            {
                _dgFundCashBook = _currentFund.CashBooks.OrderBy(t => t.TransactionDate).ToList();
                OnPropertyChanged(nameof(dgFundCashBook));
            }
        }
        */
        private List<TransferAgencyBO> _dgFundTA;
        public List<TransferAgencyBO> dgFundTA
        {
            get
            {
                return (_currentFund != null) ? _currentFund.TransferAgent.OrderBy(ta => ta.TransactionDate).ToList() : null;
            }
            set
            {
                _dgFundTA = _currentFund.TransferAgent.OrderBy(ta => ta.TransactionDate).ToList();
                OnPropertyChanged(nameof(dgFundTA));
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

        public void OpenEditModalWindow()
        {
            // Not ready yet..
            Console.WriteLine(_dtPositionObject);
        }

        public void ViewPositionDetails()
        {
            // This will be a window at some point..
            MessageBox.Show($"Name: {_dtPositionObject.security.Symbol} Quantity: {_dtPositionObject.NetQuantity} ");
        }
    }
}
