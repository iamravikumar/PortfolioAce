using PortfolioAce.Domain.DataObjects.DTOs;
using PortfolioAce.EFCore.Services;
using PortfolioAce.ViewModels.Modals;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class AddFXTradeCommand : AsyncCommandBase
    {
        private AddFXTradeWindowViewModel _addFXTradeWindowVM;
        private ITransactionService _transactionService;

        public AddFXTradeCommand(AddFXTradeWindowViewModel addFXTradeWindowVM,
            ITransactionService transactionService)
        {
            _addFXTradeWindowVM = addFXTradeWindowVM;
            _transactionService = transactionService;
        }

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                ForexDTO fxTransaction = new ForexDTO
                {
                    TradeDate = _addFXTradeWindowVM.TradeDate,
                    SettleDate = _addFXTradeWindowVM.SettleDate,
                    Price = _addFXTradeWindowVM.Price,
                    BuyAmount = _addFXTradeWindowVM.BuyAmount,
                    SellAmount = _addFXTradeWindowVM.SellAmount,
                    SellCurrency = _addFXTradeWindowVM.SellCurrency,
                    BuyCurrency = _addFXTradeWindowVM.BuyCurrency,
                    Custodian = _addFXTradeWindowVM.Custodian,
                    FundId = _addFXTradeWindowVM.FundId
                };

                await _transactionService.CreateFXTransaction(fxTransaction);
                _addFXTradeWindowVM.CloseAction();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
