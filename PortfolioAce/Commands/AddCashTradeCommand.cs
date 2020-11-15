using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore.Services;
using PortfolioAce.ViewModels.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class AddCashTradeCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private AddCashTradeWindowViewModel _addCashTradeWindowVM;
        private ICashTradeService _cashService;
        
        public AddCashTradeCommand(
            AddCashTradeWindowViewModel addCashTradeWindowVM,
            ICashTradeService cashService)
        {
            _addCashTradeWindowVM = addCashTradeWindowVM;
            _cashService = cashService;
        }

        public bool CanExecute(object parameter)
        {
            return true; // true for now
        }

        public async void Execute(object parameter)
        {
            try
            {
                CashTrade newCashTrade = new CashTrade
                {
                    CashType = _addCashTradeWindowVM.CashType,
                    Amount = _addCashTradeWindowVM.CashAmount,
                    TradeDate = _addCashTradeWindowVM.TradeDate,
                    SettleDate = _addCashTradeWindowVM.SettleDate,
                    Currency = _addCashTradeWindowVM.TradeCurrency,
                    Comment = _addCashTradeWindowVM.Notes,
                    FundId = _addCashTradeWindowVM.FundId
                };
                await _cashService.CreateCashTrade(newCashTrade);
                _addCashTradeWindowVM.CloseAction();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
