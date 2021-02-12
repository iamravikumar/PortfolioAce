﻿using PortfolioAce.Domain.Models.FactTables;
using PortfolioAce.EFCore.Services;
using PortfolioAce.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class SelectFundCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private AllFundsViewModel _allFundsWindowVM;
        private IFundService _fundService;
        public SelectFundCommand(AllFundsViewModel allFundsViewModel, IFundService fundService)
        {
            _allFundsWindowVM = allFundsViewModel;
            _fundService = fundService;
        }

        public bool CanExecute(object parameter)
        {
            return true; //For now
        }

        public void Execute(object parameter)
        {
            string fundSymbol = (string)parameter;
            _allFundsWindowVM.CurrentFund = _fundService.GetFund(fundSymbol);
            // this will return the most recent nav or the funds launch
            if (_allFundsWindowVM.CurrentFund.NavPrices.Count == 0)
            {
                _allFundsWindowVM.asOfDate = _allFundsWindowVM.CurrentFund.LaunchDate;
            }
            else
            {
                _allFundsWindowVM.asOfDate = _allFundsWindowVM.CurrentFund.NavPrices.OrderBy(f => f.FinalisedDate).Last().FinalisedDate;
            }
        }
    }
}
