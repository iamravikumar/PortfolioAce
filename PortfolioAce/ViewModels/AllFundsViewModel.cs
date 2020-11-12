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
        private IPortfolioService _portfolioService;

        public AllFundsViewModel(IFundRepository fundRepo, 
            ITradeRepository tradeRepo, ICashTradeRepository cashRepo,
            IPortfolioService portfolioService)
        {
            _tradeRepo = tradeRepo;
            _fundRepo = fundRepo;
            _cashRepo = cashRepo;
            //_lbFunds = fundRepo.GetAllFunds().ToList();
            _portfolioService = portfolioService;

            List<Fund> allFunds = fundRepo.GetAllFunds();
            _lbFunds = allFunds.Select(f => f.Symbol).ToList();
            _currentFund = (_lbFunds.Count!=0) ? _fundRepo.GetFund(_lbFunds[0]) : null;
            
            SelectFundCommand = new SelectFundCommand(this, fundRepo);

            // I can make these commands reusable 
            ShowNewTradeCommand = new ActionCommand(ShowNewTradeWindow);
            ShowNewCashTradeCommand = new ActionCommand(ShowNewCashTradeWindow);
        }

        public ICommand SelectFundCommand { get; set; }
        public ICommand ShowNewTradeCommand { get; set; }
        public ICommand ShowNewCashTradeCommand { get; set; }

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

        public void ShowNewTradeWindow()
        {
            // if no id available then raise error;
            int fundId = _currentFund.FundId;
            Window view = new AddTradeWindow();
            ViewModelWindowBase viewModel = new AddTradeWindowViewModel(_tradeRepo, fundId);
            view.DataContext = viewModel;
            view.Owner = Application.Current.MainWindow;
            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (viewModel.CloseAction == null)
            {
                viewModel.CloseAction = new Action(() => view.Close());
            }
            view.ShowDialog();
        }

        public void ShowNewCashTradeWindow()
        {
            // if no id available then raise error;
            int fundId = _currentFund.FundId;
            Window view = new AddCashTradeWindow();
            ViewModelWindowBase viewModel = new AddCashTradeWindowViewModel(_cashRepo, fundId);
            view.DataContext = viewModel;
            view.Owner = Application.Current.MainWindow;
            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (viewModel.CloseAction == null)
            {
                viewModel.CloseAction = new Action(() => view.Close());
            }
            view.ShowDialog();
        }
    }
}
