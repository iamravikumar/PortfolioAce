using PortfolioAce.Commands;
using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PortfolioAce.ViewModels
{
    public class AllFundsViewModel:ViewModelBase
    {
        private IFundRepository _fundRepo;
        public AllFundsViewModel(IFundRepository fundRepo)
        {
            //
            _fundRepo = fundRepo;
            _lbFunds = fundRepo.GetAllFunds().ToList();
            _currentFund = (_lbFunds.Count!=0) ? _lbFunds[0] : null;
            SelectFundCommand = new SelectFundCommand(this);
        }

        public ICommand SelectFundCommand { get; set; }

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

    }
}
