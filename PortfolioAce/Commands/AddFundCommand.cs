using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore.Services;
using PortfolioAce.ViewModels.Modals;
using PortfolioAce.ViewModels.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    class AddFundCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private AddFundWindowViewModel _addFundWindowVM;
        private IFundService _fundService;

        public AddFundCommand(AddFundWindowViewModel addFundWindowVM, IFundService fundService)
        {
            _addFundWindowVM = addFundWindowVM;
            _fundService = fundService;
        }

        public bool CanExecute(object parameter)
        {
            return true; // true for now
        }

        public async void Execute(object parameter)
        {
            try
            {
                Fund newFund = new Fund
                {
                    FundName = _addFundWindowVM.FundName,
                    Symbol = _addFundWindowVM.FundSymbol,
                    BaseCurrency = _addFundWindowVM.FundCurrency,
                    ManagementFee = _addFundWindowVM.FundManFee,
                    PerformanceFee = _addFundWindowVM.FundPerfFee,
                    NAVFrequency = _addFundWindowVM.FundNavFreq,
                    LaunchDate = _addFundWindowVM.FundLaunch.Date,
                    IsInitialised =false
                };
                // i've hardcoded IsInitialised for now. false is the default option.
                await _fundService.CreateFund(newFund);
                _addFundWindowVM.CloseAction();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
