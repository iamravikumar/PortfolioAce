using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        }

        // List box click should have a command and that command changes the fields displayed on the right of the allfundsview.
        private List<Fund> _lbFunds;
        public List<string> lbFunds
        {
            get
            {
                return _lbFunds.Select(f => f.FundName).ToList();
            }
            set
            {
                _lbFunds = _fundRepo.GetAllFunds().ToList();
                OnPropertyChanged(nameof(lbFunds));
            }
        }
    }
}
