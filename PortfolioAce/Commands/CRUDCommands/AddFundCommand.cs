using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore.Services;
using PortfolioAce.ViewModels.Modals;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class AddFundCommand : AsyncCommandBase
    {
        public event EventHandler CanExecuteChanged;
        private AddFundWindowViewModel _addFundWindowVM;
        private IFundService _fundService;

        public AddFundCommand(AddFundWindowViewModel addFundWindowVM, IFundService fundService)
        {
            _addFundWindowVM = addFundWindowVM;
            _fundService = fundService;
        }

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object parameter)
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
                    HasHighWaterMark = _addFundWindowVM.HighWaterMark,
                    HurdleRate = _addFundWindowVM.HurdleRate,
                    HurdleType = _addFundWindowVM.selectedHurdleType,
                    MinimumInvestment = _addFundWindowVM.MinimumInvestment,
                    IsInitialised = false
                };
                // i've hardcoded IsInitialised for now. false is the default option.
                await _fundService.CreateFund(newFund);
                _addFundWindowVM.CloseAction();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
