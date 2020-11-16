using PortfolioAce.Commands;
using PortfolioAce.Domain.BusinessServices;
using PortfolioAce.Domain.DataObjects;
using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore.Services;
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
    public class AllFundsViewModel:ViewModelBase
    {
        private IFundService _fundService;
        private ITradeService _tradeService;
        private ICashTradeService _cashService;
        private ITransferAgencyService _investorService;
        private IPortfolioService _portfolioService;

        public AllFundsViewModel(IFundService fundService,
            ITradeService tradeService, ICashTradeService cashService,
            IPortfolioService portfolioService, ITransferAgencyService investorService)
        {
            _tradeService = tradeService;
            _fundService = fundService;
            _cashService = cashService;
            _investorService = investorService;
            _portfolioService = portfolioService;

            List<Fund> allFunds = fundService.GetAllFunds();
            _lbFunds = allFunds.Select(f => f.Symbol).ToList();
            _currentFund = (_lbFunds.Count!=0) ? _fundService.GetFund(_lbFunds[0]) : null;
            
            SelectFundCommand = new SelectFundCommand(this, fundService);
            
            ShowNewTradeCommand = new ActionCommand<Type, Type, ITradeService>(
                OpenModalWindow, typeof(AddTradeWindow), typeof(AddTradeWindowViewModel), _tradeService);
            ShowNewCashTradeCommand = new ActionCommand<Type, Type, ICashTradeService>(
                OpenModalWindow, typeof(AddCashTradeWindow), typeof(AddCashTradeWindowViewModel), _cashService);
            ShowNewInvestorActionCommand = new ActionCommand<Type, Type, ITransferAgencyService>(
                OpenModalWindow, typeof(InvestorActionsWindow), typeof(InvestorActionViewModel), _investorService
                );
            PositionDetailCommand = new ActionCommand(ViewPositionDetails);
        }

        public ICommand SelectFundCommand { get; set; }
        public ICommand ShowNewTradeCommand { get; set; }
        public ICommand ShowNewCashTradeCommand { get; set; }
        
        public ICommand ShowNewInvestorActionCommand { get; set; }

        public ICommand PositionDetailCommand { get; set; }


        private Position _dtPositionObject;
        public Position dtPositionObject
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
                OnPropertyChanged(nameof(CurrentFund));
                OnPropertyChanged(nameof(dgFundPositions));
                OnPropertyChanged(nameof(dgFundCashHoldings));
                OnPropertyChanged(nameof(dgFundTrades));
                OnPropertyChanged(nameof(dgFundCashBook));
                OnPropertyChanged(nameof(dgFundTA));
            }
        }

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

        private List<Position> _dgFundPositions;
        public List<Position> dgFundPositions
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

        private List<Trade> _dgFundTrades;
        public List<Trade> dgFundTrades
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

        private List<CashBook> _dgFundCashBook;
        public List<CashBook> dgFundCashBook
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

        private List<TransferAgency> _dgFundTA;
        public List<TransferAgency> dgFundTA
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

        public void OpenModalWindow(Type windowType, Type viewModelType, object myService)
        {
            int fundId = _currentFund.FundId;
            Window view = (Window)Activator.CreateInstance(windowType);
            ViewModelWindowBase viewModel = (ViewModelWindowBase)Activator.CreateInstance(viewModelType, myService, _currentFund);
            
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
            Console.WriteLine(_dtPositionObject);
        }

        public void ViewPositionDetails()
        {
            MessageBox.Show($"Name: {_dtPositionObject.security.Symbol} Quantity: {_dtPositionObject.NetQuantity} ");
        }
    }
}
