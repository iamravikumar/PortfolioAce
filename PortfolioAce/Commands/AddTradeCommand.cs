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
    public class AddTradeCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private AddTradeWindowViewModel _addTradeWindowVM;
        private ITradeRepository _tradeRepo;

        public AddTradeCommand(AddTradeWindowViewModel addTradeWindowVM,
            ITradeRepository tradeRepo)
        {
            _addTradeWindowVM = addTradeWindowVM;
            _tradeRepo = tradeRepo;
        }

        public bool CanExecute(object parameter)
        {
            return true; // true for now
        }

        public async void Execute(object parameter)
        {
            try
            {
                Trade newTrade = new Trade
                {
                    TradeType = _addTradeWindowVM.TradeType,
                    Symbol = _addTradeWindowVM.Symbol,
                    Quantity = _addTradeWindowVM.Quantity,
                    Price = _addTradeWindowVM.Price,
                    TradeAmount = _addTradeWindowVM.TradeAmount,
                    TradeDate = _addTradeWindowVM.TradeDate,
                    SettleDate = _addTradeWindowVM.SettleDate,
                    Currency = _addTradeWindowVM.TradeCurrency,
                    Commission = _addTradeWindowVM.Commission,
                    FundId = _addTradeWindowVM.FundId
                };
                await _tradeRepo.CreateTrade(newTrade);
                _addTradeWindowVM.CloseAction();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}