using PortfolioAce.Commands;
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
        public AllFundsViewModel(IFundRepository fundRepo, ITradeRepository tradeRepo)
        {
            _tradeRepo = tradeRepo;
            _fundRepo = fundRepo;
            _lbFunds = fundRepo.GetAllFunds().ToList();
            _currentFund = (_lbFunds.Count!=0) ? _lbFunds[0] : null;
            SelectFundCommand = new SelectFundCommand(this);
            ShowNewTradeCommand = new ActionCommand(ShowNewTradeWindow);
        }

        public ICommand SelectFundCommand { get; set; }
        public ICommand ShowNewTradeCommand { get; set; }

        // List box click should have a command and that command changes the fields displayed on the right of the allfundsview.
        private List<Fund> _lbFunds;
        public List<string> lbFunds
        {
            get
            {
                return _lbFunds.Select(f => f.Symbol).ToList();
            }
            set
            {
                _lbFunds = _fundRepo.GetAllFunds().ToList();
                OnPropertyChanged(nameof(lbFunds));
            }
        }

        private Fund _currentFund;
        public string CurrentFund
        {
            get
            {
                return (_currentFund != null)?_currentFund.FundName:null;
            }
            set
            {
                _currentFund = _lbFunds.Where(f => f.Symbol==value).FirstOrDefault();
                OnPropertyChanged(nameof(CurrentFund));
            }
        }

        public void ShowNewTradeWindow()
        {
            // if no id available then raise error;
            int fundId = _currentFund.FundId;
            Window view = new AddTradeWindow();
            ViewModelBase viewModel = new AddTradeWindowViewModel(_tradeRepo, fundId);
            view.DataContext = viewModel;
            view.Owner = Application.Current.MainWindow;
            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            view.ShowDialog();
        }
    }
}
