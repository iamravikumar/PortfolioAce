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
    public class AddCashTradeCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private AddCashTradeWindowViewModel _addCashTradeWindowVM;
        private ITransactionService _transactionService;

        public AddCashTradeCommand(
            AddCashTradeWindowViewModel addCashTradeWindowVM,
            ITransactionService transactionService)
        {
            _addCashTradeWindowVM = addCashTradeWindowVM;
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
                // cash symbol would be something like EURc name = EUR CASH
                SecuritiesDIM security = _transactionService.GetSecurityInfo(_addCashTradeWindowVM.Symbol);
                TransactionTypeDIM tradeType = _transactionService.GetTradeType(_addCashTradeWindowVM.CashType);
                CustodiansDIM custodian = _transactionService.GetCustodian(_addCashTradeWindowVM.Custodian);
                TransactionsBO newCashTrade = new TransactionsBO
                {
                    SecurityId = security.SecurityId,
                    Quantity = _addCashTradeWindowVM.Quantity,
                    Price = _addCashTradeWindowVM.Price,
                    TradeAmount = _addCashTradeWindowVM.CashAmount,
                    TradeDate = _addCashTradeWindowVM.TradeDate,
                    SettleDate = _addCashTradeWindowVM.SettleDate,
                    CreatedDate = _addCashTradeWindowVM.CreatedDate,
                    LastModified = _addCashTradeWindowVM.LastModifiedDate,
                    Fees = _addCashTradeWindowVM.Fees,
                    isActive = _addCashTradeWindowVM.isActive,
                    isLocked = _addCashTradeWindowVM.isLocked,
                    FundId = _addCashTradeWindowVM.FundId,
                    TransactionTypeId = tradeType.TransactionTypeId,
                    CurrencyId= security.CurrencyId,
                    Comment=_addCashTradeWindowVM.Notes,
                    CustodianId=custodian.CustodianId
                };
                await _transactionService.CreateTransaction(newCashTrade);
                _addCashTradeWindowVM.CloseAction();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
