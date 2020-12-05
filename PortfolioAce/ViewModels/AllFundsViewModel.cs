using PortfolioAce.Commands;
using PortfolioAce.Domain.BusinessServices;
using PortfolioAce.Domain.DataObjects;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
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
            _asOfDate = DateTime.Today.AddDays(-1); // Changed to the day i have a price for.
            _priceTable = staticReferences.GetPriceTable(_asOfDate);

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
            }
        }


        private DataGridValuedPosition _dtPositionObject;
        public DataGridValuedPosition dtPositionObject
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


        private Dictionary<(int, DateTime), decimal> _priceTable;
        public Dictionary<(int, DateTime), decimal> priceTable
        {
            get
            {
                return _priceTable;
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
        
        private List<CalculatedCashPosition> _dgFundCashHoldings;
        public List<CalculatedCashPosition> dgFundCashHoldings
        {
            get
            {
                return (_currentFund != null) ? _portfolioService.GetAllCashBalances(_currentFund, _asOfDate) : null;
            }
            set
            {
                _dgFundCashHoldings = _portfolioService.GetAllCashBalances(_currentFund, _asOfDate);
                OnPropertyChanged(nameof(dgFundCashHoldings));
            }
        }
        /*
        private List<CalculatedSecurityPosition> _dgFundPositions;
        public List<CalculatedSecurityPosition> dgFundPositions
        {
            get
            {
                return (_currentFund!=null)?_portfolioService.GetAllSecurityPositions(_currentFund, _asOfDate):null;
            }
            set
            {
                _dgFundPositions = _portfolioService.GetAllSecurityPositions(_currentFund, _asOfDate);
                OnPropertyChanged(nameof(dgFundPositions));
            }
        }
        */
        private List<DataGridValuedPosition> _dgFundPositions;
        public List<DataGridValuedPosition> dgFundPositions
        {
            get
            {
                // This is temporary for now
                if (_currentFund != null)
                {
                    List<CalculatedSecurityPosition> allPositions = _portfolioService.GetAllSecurityPositions(_currentFund, _asOfDate);
                    List<DataGridValuedPosition> dgPositions = new List<DataGridValuedPosition>();
                    foreach(CalculatedSecurityPosition position in allPositions)
                    {
                        var p = new DataGridValuedPosition(position, _priceTable, _asOfDate);
                        dgPositions.Add(p);
                    }
                    return dgPositions;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                List<CalculatedSecurityPosition> allPositions = _portfolioService.GetAllSecurityPositions(_currentFund, _asOfDate);
                List<DataGridValuedPosition> dgPositions = new List<DataGridValuedPosition>();
                foreach (CalculatedSecurityPosition position in allPositions)
                {
                    var p = new DataGridValuedPosition(position, _priceTable, _asOfDate);
                    dgPositions.Add(p);
                }
                
                _dgFundPositions = dgPositions;
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
            MessageBox.Show($"Name: {_dtPositionObject.Position.security.Symbol} Quantity: {_dtPositionObject.Position.NetQuantity} ");
        }
    }

    public class DataGridValuedPosition
    {
        public CalculatedSecurityPosition Position { get; set; }
        public decimal MarketValue { get; set; }
        public decimal unrealisedPnl {get;set;}
        public decimal unrealisedPnLPercent { get; set; }
        public decimal price { get; set; }
        public DateTime AsOfDate { get; set; }

        // I can even put performance metrics here
        public DataGridValuedPosition(CalculatedSecurityPosition position, Dictionary<(int, DateTime), decimal> priceTable, DateTime asOfDate)
        {
            this.Position = position;
            this.AsOfDate = asOfDate;
            ValueTuple<int, DateTime> tableKey = (position.security.SecurityId, asOfDate);
            int multiplierPnL = (position.NetQuantity>=0) ? 1 : -1;
            this.price = priceTable.ContainsKey(tableKey) ?priceTable[tableKey]: decimal.Zero;
            this.MarketValue = Math.Round(position.NetQuantity * price,2);
            this.unrealisedPnl = Math.Round(position.NetQuantity*(position.AverageCost-this.price)*multiplierPnL, 2);
        }
    }
}
