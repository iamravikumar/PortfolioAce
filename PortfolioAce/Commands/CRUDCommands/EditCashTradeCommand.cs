using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.EFCore.Services;
using PortfolioAce.ViewModels.Modals;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class EditCashTradeCommand : AsyncCommandBase
    {
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

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object parameter)
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
                await _transactionService.UpdateTransaction(_transaction);
                _editCashTradeWindowVM.CloseAction();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
