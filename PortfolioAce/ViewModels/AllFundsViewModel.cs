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
            _asOfDate = DateTime.Today; // Changed to the day i have a price for.
            _priceTable = staticReferences.GetPriceTable(_asOfDate); // to save space i can look just for asofdate OR use all securities under particular fund

            SelectFundCommand = new SelectFundCommand(this, fundService);
            
            ShowNewTradeCommand = new ActionCommand<Type, Type, ITransactionService, IStaticReferences>(
                OpenModalWindow, typeof(AddTradeWindow), typeof(AddTradeWindowViewModel), _transactionService, _staticReferences);
            ShowNewCashTradeCommand = new ActionCommand<Type, Type, ITransactionService, IStaticReferences>(
                OpenModalWindow, typeof(AddCashTradeWindow), typeof(AddCashTradeWindowViewModel), _transactionService, _staticReferences);
            ShowNewInvestorActionCommand = new ActionCommand<Type, Type, ITransferAgencyService, IStaticReferences>(
                OpenModalWindow, typeof(InvestorActionsWindow), typeof(InvestorActionViewModel), _investorService, _staticReferences);
            ShowFundInitialisationCommand = new ActionCommand<Type, Type, ITransferAgencyService, IStaticReferences>(
                OpenModalWindow, typeof(FundInitialisationWindow), typeof(FundInitialisationWindowViewModel), _investorService, _staticReferences);
            ShowNavSummaryCommand = new ActionCommand<Type, Type, ITransferAgencyService, IStaticReferences>(
                OpenNavSummaryWindow, typeof(NavSummaryWindow),typeof(NavSummaryViewModel), _investorService, _staticReferences);
            PositionDetailCommand = new ActionCommand(ViewPositionDetails);
        }

        public ICommand SelectFundCommand { get; set; }
        public ICommand ShowNewTradeCommand { get; set; }
        public ICommand ShowNewCashTradeCommand { get; set; }

        public ICommand ShowFundInitialisationCommand { get; set; }
        public ICommand ShowNewInvestorActionCommand { get; set; }
        public ICommand ShowNavSummaryCommand { get; set; }
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
                OnPropertyChanged(nameof(dgFundCashHoldings));
                OnPropertyChanged(nameof(dgFundPositions));
                OnPropertyChanged(nameof(dgFundTrades));
                OnPropertyChanged(nameof(dgFundCashBook));
                OnPropertyChanged(nameof(dgFundTA));
                OnPropertyChanged(nameof(CurrentNavPrice));
                OnPropertyChanged(nameof(LockedNav));
                OnPropertyChanged(nameof(NavValuation));
                OnPropertyChanged(nameof(dgFundClients));
                OnPropertyChanged(nameof(testPositions));
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


        private SecurityPositionValuation _dtPositionObject;
        public SecurityPositionValuation dtPositionObject
        {
            // This object will open a window that will display the position information such as local currency metrics, open lots etc..
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


        private Dictionary<(string, DateTime), decimal> _priceTable;
        public Dictionary<(string, DateTime), decimal> priceTable
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
                OnPropertyChanged(nameof(CurrentNavPrice));
                OnPropertyChanged(nameof(ShowWidget));
                OnPropertyChanged(nameof(LockedNav));
                OnPropertyChanged(nameof(NavValuation));
                OnPropertyChanged(nameof(dgFundClients));
                OnPropertyChanged(nameof(testPositions));
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
        
        private List<CashPositionValuation> _dgFundCashHoldings;
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
                        CashPositionValuation valuedCashPosition = new CashPositionValuation(cashPosition, _priceTable, _asOfDate, _currentFund.BaseCurrency);
                        valuedCashPositions.Add(valuedCashPosition);
                    }
                    return valuedCashPositions;
                }
                else
                {
                    return new List<CashPositionValuation>();
                }
            }
            set
            {
                List<CalculatedCashPosition> cashPositions = _portfolioService.GetAllCashBalances(_currentFund, _asOfDate);
                List<CashPositionValuation> valuedCashPositions = new List<CashPositionValuation>();
                foreach (CalculatedCashPosition cashPosition in cashPositions)
                {
                    CashPositionValuation valuedCashPosition = new CashPositionValuation(cashPosition, _priceTable, _asOfDate, _currentFund.BaseCurrency);
                    valuedCashPositions.Add(valuedCashPosition);
                }
                _dgFundCashHoldings = valuedCashPositions;
                OnPropertyChanged(nameof(dgFundCashHoldings));
            }
        }

        private List<ClientHolding> _dgFundClients;
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
            set
            {
                List<ClientHolding> allClientHoldings = _portfolioService.GetAllClientHoldings(_currentFund, _asOfDate);
                _dgFundClients = allClientHoldings;
                OnPropertyChanged(nameof(dgFundCashHoldings));
            }
        }
        /*
        private List<SecurityPositionValuation> _dgFundPositions;
        public List<SecurityPositionValuation> dgFundPositions
        {
            get
            {
                // This is temporary for now
                if (_currentFund != null)
                {
                    List<CalculatedSecurityPosition> allPositions = _portfolioService.GetAllSecurityPositions(_currentFund, _asOfDate);
                    List<SecurityPositionValuation> valuedPositions = new List<SecurityPositionValuation>();
                    
                    foreach(CalculatedSecurityPosition position in allPositions)
                    {
                        SecurityPositionValuation valuedPosition = new SecurityPositionValuation(position, _priceTable, _asOfDate, _currentFund.BaseCurrency);
                        valuedPositions.Add(valuedPosition);
                    }
                    return valuedPositions;
                }
                else
                {
                    return new List<SecurityPositionValuation>();
                }
            }
            set
            {
                List<CalculatedSecurityPosition> allPositions = _portfolioService.GetAllSecurityPositions(_currentFund, _asOfDate);
                List<SecurityPositionValuation> valuedPositions = new List<SecurityPositionValuation>();
                foreach (CalculatedSecurityPosition position in allPositions)
                {
                    SecurityPositionValuation valuedPosition = new SecurityPositionValuation(position, _priceTable, _asOfDate, _currentFund.BaseCurrency);
                    valuedPositions.Add(valuedPosition);
                }
                
                _dgFundPositions = valuedPositions;
                OnPropertyChanged(nameof(dgFundPositions));
            }
        }*/
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
                        SecurityPositionValuation valuedPosition = new SecurityPositionValuation(position, _priceTable, _asOfDate, _currentFund.BaseCurrency);
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
        private ListCollectionView _testPositions;
        public ListCollectionView testPositions
        {
            get
            {
                ListCollectionView cv = new ListCollectionView(dgFundPositions);
                cv.GroupDescriptions.Add(new PropertyGroupDescription("Position.security.AssetClass"));
                return cv;
            }
            set
            {
                ListCollectionView cv = new ListCollectionView(dgFundPositions);
                cv.GroupDescriptions.Add(new PropertyGroupDescription("Position.security.AssetClass"));
                _testPositions = cv;
                OnPropertyChanged(nameof(testPositions));
            }
        }
        /*
        private ListCollectionView _testPositions;
        public ListCollectionView testPositions
        {
            get
            {
                // This is temporary for now
                if (_currentFund != null)
                {
                    List<CalculatedSecurityPosition> allPositions = _portfolioService.GetAllSecurityPositions(_currentFund, _asOfDate);
                    List<SecurityPositionValuation> valuedPositions = new List<SecurityPositionValuation>();

                    foreach (CalculatedSecurityPosition position in allPositions)
                    {
                        SecurityPositionValuation valuedPosition = new SecurityPositionValuation(position, _priceTable, _asOfDate, _currentFund.BaseCurrency);
                        valuedPositions.Add(valuedPosition);
                    }
                    ListCollectionView cv = new ListCollectionView(valuedPositions);
                    cv.GroupDescriptions.Add(new PropertyGroupDescription("Position.security.AssetClass"));
                    return cv;
                }
                else
                {
                    return new ListCollectionView(new List<SecurityPositionValuation>());
                }
            }
            set
            {
                List<CalculatedSecurityPosition> allPositions = _portfolioService.GetAllSecurityPositions(_currentFund, _asOfDate);
                List<SecurityPositionValuation> valuedPositions = new List<SecurityPositionValuation>();
                foreach (CalculatedSecurityPosition position in allPositions)
                {
                    SecurityPositionValuation valuedPosition = new SecurityPositionValuation(position, _priceTable, _asOfDate, _currentFund.BaseCurrency);
                    valuedPositions.Add(valuedPosition);
                }
                ListCollectionView cv = new ListCollectionView(valuedPositions);
                cv.GroupDescriptions.Add(new PropertyGroupDescription("Position.security.AssetClass"));
                _testPositions = cv;
                OnPropertyChanged(nameof(testPositions));
            }
        }
        */

        private List<TransactionsBO> _dgFundTrades;
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
            set
            {
                _dgFundTrades = _currentFund.Transactions
                                            .Where(t => t.TransactionType.TypeClass.ToString() == "SecurityTrade" && t.TradeDate<= _asOfDate)
                                            .OrderBy(t => t.TradeDate)
                                            .ToList();
                OnPropertyChanged(nameof(dgFundTrades));
            }
        }

        private List<TransactionsBO> _dgFundCashBook;
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
            set
            {
                _dgFundCashBook = _currentFund.Transactions
                                              .Where(t=>t.TradeDate<=_asOfDate)
                                              .OrderBy(t => t.TradeDate)
                                              .ToList();
                OnPropertyChanged(nameof(dgFundCashBook));
            }
        }

        private List<TransferAgencyBO> _dgFundTA;
        public List<TransferAgencyBO> dgFundTA
        {
            get
            {
                return (_currentFund != null) ? _currentFund.TransferAgent
                                                            .Where(ta=>ta.TransactionDate<=_asOfDate)
                                                            .OrderBy(ta => ta.TransactionDate)
                                                            .ToList() : null;
            }
            set
            {
                _dgFundTA = _currentFund.TransferAgent
                                        .Where(ta=>ta.TransactionDate<=_asOfDate)
                                        .OrderBy(ta => ta.TransactionDate)
                                        .ToList();
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
}
