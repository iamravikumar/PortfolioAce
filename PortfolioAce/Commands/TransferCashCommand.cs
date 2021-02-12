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
    public class TransferCashCommand:ICommand
    {
        public event EventHandler CanExecuteChanged;
        private AddCashTradeWindowViewModel _addCashTradeWindowVM;
        private ITransactionService _transactionService;

        public TransferCashCommand(
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

        public void Execute(object parameter)
        {
            try
            {
                // cash symbol would be something like EURc name = EUR CASH
                SecuritiesDIM security = _transactionService.GetSecurityInfo(_addCashTradeWindowVM.Symbol);
                TransactionTypeDIM tradeType = _transactionService.GetTradeType(_addCashTradeWindowVM.CashType);
                CustodiansDIM payeeCustodian = _transactionService.GetCustodian(_addCashTradeWindowVM.PayeeCustodian);
                CustodiansDIM recipientCustodian = _transactionService.GetCustodian(_addCashTradeWindowVM.RecipientCustodian);
                TransactionsBO payeeTrade = new TransactionsBO
                {
                    SecurityId = security.SecurityId,
                    Quantity = (_addCashTradeWindowVM.Quantity + _addCashTradeWindowVM.PayeeFee) * -1,
                    TradeAmount = (_addCashTradeWindowVM.CashAmount + _addCashTradeWindowVM.PayeeFee) * -1,
                    TradeDate = _addCashTradeWindowVM.TradeDate,
                    SettleDate = _addCashTradeWindowVM.SettleDate,
                    CreatedDate = _addCashTradeWindowVM.CreatedDate,
                    LastModified = _addCashTradeWindowVM.LastModifiedDate,
                    Fees = _addCashTradeWindowVM.PayeeFee,
                    isActive = _addCashTradeWindowVM.isActive,
                    isLocked = _addCashTradeWindowVM.isLocked,
                    isCashTransaction = true,
                    FundId = _addCashTradeWindowVM.FundId,
                    TransactionTypeId = tradeType.TransactionTypeId,
                    CurrencyId = security.CurrencyId,
                    Comment = _addCashTradeWindowVM.PayeesNotes,
                    CustodianId = payeeCustodian.CustodianId
                };

                TransactionsBO recipientTrade = new TransactionsBO
                {
                    SecurityId = security.SecurityId,
                    Quantity = _addCashTradeWindowVM.Quantity - _addCashTradeWindowVM.RecipientFee,
                    Price = _addCashTradeWindowVM.Price,
                    TradeAmount = _addCashTradeWindowVM.CashAmount - _addCashTradeWindowVM.RecipientFee,
                    TradeDate = _addCashTradeWindowVM.TradeDate,
                    SettleDate = _addCashTradeWindowVM.SettleDate,
                    CreatedDate = _addCashTradeWindowVM.CreatedDate,
                    LastModified = _addCashTradeWindowVM.LastModifiedDate,
                    Fees = _addCashTradeWindowVM.RecipientFee,
                    isActive = _addCashTradeWindowVM.isActive,
                    isLocked = _addCashTradeWindowVM.isLocked,
                    isCashTransaction = true,
                    FundId = _addCashTradeWindowVM.FundId,
                    TransactionTypeId = tradeType.TransactionTypeId,
                    CurrencyId = security.CurrencyId,
                    Comment = _addCashTradeWindowVM.RecipientNotes,
                    CustodianId = recipientCustodian.CustodianId
                };
                List<TransactionsBO> transfer = new List<TransactionsBO> { payeeTrade, recipientTrade };
                _transactionService.CreateCashTransfer(transfer);
                _addCashTradeWindowVM.CloseAction();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
