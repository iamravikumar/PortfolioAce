﻿using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
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
    public class AddTradeCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private AddTradeWindowViewModel _addTradeWindowVM;
        private ITradeService _tradeService;

        public AddTradeCommand(AddTradeWindowViewModel addTradeWindowVM,
            ITradeService tradeService)
        {
            _addTradeWindowVM = addTradeWindowVM;
            _tradeService = tradeService;
        }

        public bool CanExecute(object parameter)
        {
            return true; // true for now
        }

        public async void Execute(object parameter)
        {
            try
            {
                SecuritiesDIM security = _tradeService.GetSecurityInfo(_addTradeWindowVM.Symbol);
                TradeBO newTrade = new TradeBO
                {
                    TradeType = _addTradeWindowVM.TradeType,
                    SecurityId = security.SecurityId,
                    Quantity = _addTradeWindowVM.Quantity,
                    Price = _addTradeWindowVM.Price,
                    TradeAmount = _addTradeWindowVM.TradeAmount,
                    TradeDate = _addTradeWindowVM.TradeDate,
                    SettleDate = _addTradeWindowVM.SettleDate,
                    Currency = _addTradeWindowVM.TradeCurrency,
                    Commission = _addTradeWindowVM.Commission,
                    FundId = _addTradeWindowVM.FundId
                };
                await _tradeService.CreateTrade(newTrade);
                _addTradeWindowVM.CloseAction();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}