using PortfolioAce.Domain.Models;
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
        private ITransactionService _transactionService;

        public AddTradeCommand(AddTradeWindowViewModel addTradeWindowVM,
            ITransactionService transactionService)
        {
            _addTradeWindowVM = addTradeWindowVM;
            _transactionService = transactionService;
        }

        public bool CanExecute(object parameter)
        {
            return true; // true for now
        }

        public async void Execute(object parameter)
        {
            try
            {
                SecuritiesDIM security = _transactionService.GetSecurityInfo(_addTradeWindowVM.Symbol);
                TransactionTypeDIM tradeType = _transactionService.GetTradeType(_addTradeWindowVM.TradeType);
                TransactionsBO newTrade = new TransactionsBO
                {
                    SecurityId = security.SecurityId,
                    Quantity = _addTradeWindowVM.Quantity,
                    Price = _addTradeWindowVM.Price,
                    TradeAmount = _addTradeWindowVM.TradeAmount,
                    TradeDate = _addTradeWindowVM.TradeDate,
                    SettleDate = _addTradeWindowVM.SettleDate,
                    CreatedDate = _addTradeWindowVM.CreatedDate,
                    LastModified = _addTradeWindowVM.LastModifiedDate,
                    Fees = _addTradeWindowVM.Commission,
                    isActive = _addTradeWindowVM.isActive,
                    isLocked = _addTradeWindowVM.isLocked,
                    FundId = _addTradeWindowVM.FundId,
                    TransactionTypeId = tradeType.TransactionTypeId,
                    CurrencyId = security.CurrencyId,
                    Comment=""
                };
                await _transactionService.CreateTransaction(newTrade);
                _addTradeWindowVM.CloseAction();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}