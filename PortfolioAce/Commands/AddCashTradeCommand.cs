using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore.Repository;
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
        private ICashTradeRepository _cashRepo;
        
        public AddCashTradeCommand(
            AddCashTradeWindowViewModel addCashTradeWindowVM,
            ICashTradeRepository cashRepo)
        {
            _addCashTradeWindowVM = addCashTradeWindowVM;
            _cashRepo = cashRepo;
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
                await _cashRepo.CreateCashTrade(newCashTrade);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
