using PortfolioAce.Commands;
using PortfolioAce.Domain.BusinessServices;
using PortfolioAce.Domain.DataObjects;
using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore.Repository;
using PortfolioAce.Navigation;
using PortfolioAce.ViewModels.Windows;
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
        private IFundRepository _fundRepo;
        private ITradeRepository _tradeRepo;
        private ICashTradeRepository _cashRepo;
        private ITransferAgencyRepository _investorRepo;
        private IPortfolioService _portfolioService;

        public AllFundsViewModel(IFundRepository fundRepo,
            ITradeRepository tradeRepo, ICashTradeRepository cashRepo,
            IPortfolioService portfolioService, ITransferAgencyRepository investorRepo)
        {
            _tradeRepo = tradeRepo;
            _fundRepo = fundRepo;
            _cashRepo = cashRepo;
            _investorRepo = investorRepo;
            _portfolioService = portfolioService;

            List<Fund> allFunds = fundRepo.GetAllFunds();
            _lbFunds = allFunds.Select(f => f.Symbol).ToList();
            _currentFund = (_lbFunds.Count!=0) ? _fundRepo.GetFund(_lbFunds[0]) : null;
            
            SelectFundCommand = new SelectFundCommand(this, fundRepo);
            
            ShowNewTradeCommand = new ActionCommand<Type, Type, ITradeRepository>(
                OpenModalWindow, typeof(AddTradeWindow), typeof(AddTradeWindowViewModel), _tradeRepo);
            ShowNewCashTradeCommand = new ActionCommand<Type, Type, ICashTradeRepository>(
                OpenModalWindow, typeof(AddCashTradeWindow), typeof(AddCashTradeWindowViewModel), _cashRepo);
            ShowNewInvestorActionCommand = new ActionCommand<Type, Type, ITransferAgencyRepository>(
                OpenModalWindow, typeof(InvestorActionsWindow), typeof(InvestorActionViewModel), _investorRepo
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
                List<Fund> allFunds = _fundRepo.GetAllFunds();
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

        public void OpenModalWindow(Type windowType, Type viewModelType, object myRepo)
        {
            int fundId = _currentFund.FundId;
            Window view = (Window)Activator.CreateInstance(windowType);
            ViewModelWindowBase viewModel = (ViewModelWindowBase)Activator.CreateInstance(viewModelType, myRepo, _currentFund);
            
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
            MessageBox.Show($"Name: {_dtPositionObject.symbol} Quantity: {_dtPositionObject.NetQuantity} ");
        }
    }
}
