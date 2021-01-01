using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.EFCore.Services;
using PortfolioAce.ViewModels.Modals;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class EditCashTradeCommand:ICommand
    {
        public event EventHandler CanExecuteChanged;
        private EditCashTradeWindowViewModel _editCashTradeWindowVM;
        private ITransactionService _transactionService;
        private TransactionsBO _transaction;

        public EditCashTradeCommand(EditCashTradeWindowViewModel editCashTradeWindowVM,
            ITransactionService transactionService, TransactionsBO transaction)
        {
            _editCashTradeWindowVM = editCashTradeWindowVM;
            _transactionService = transactionService;
            _transaction = transaction;
        }

        public bool CanExecute(object parameter)
        {
            return true; // true for now
        }

        public void Execute(object parameter)
        {
            try
            {
                // cash symbol would be something like EURc name = EUR CASH
                TransactionTypeDIM tradeType = _transactionService.GetTradeType(_editCashTradeWindowVM.CashType);
                CustodiansDIM custodian = _transactionService.GetCustodian(_editCashTradeWindowVM.Custodian);
                _transaction.Comment = _editCashTradeWindowVM.Notes;
                _transaction.TradeAmount = _editCashTradeWindowVM.CashAmount;
                _transaction.LastModified = _editCashTradeWindowVM.LastModifiedDate;
                _transaction.TransactionTypeId = tradeType.TransactionTypeId;
                _transaction.TransactionType = tradeType;
                _transaction.CustodianId = custodian.CustodianId;
                _transaction.Custodian = custodian;
                _transaction.TradeDate = _editCashTradeWindowVM.TradeDate;
                _transaction.SettleDate = _editCashTradeWindowVM.SettleDate;

                _transactionService.UpdateTransaction(_transaction);
                _editCashTradeWindowVM.CloseAction();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
