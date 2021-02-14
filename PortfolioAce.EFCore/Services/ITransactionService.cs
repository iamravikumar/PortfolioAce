using PortfolioAce.Domain.DataObjects.DTOs;
using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services
{
    public interface ITransactionService : IBaseService
    {
        Task<TransactionsBO> CreateTransaction(TransactionsBO transaction);
        void CreateCashTransfer(List<TransactionsBO> transfers);
        Task<TransactionsBO> CreateFXTransaction(ForexDTO fxTransaction);
        void UpdateTransaction(TransactionsBO transaction);

        void DeleteTransaction(TransactionsBO transaction);
        void RestoreTransaction(TransactionsBO transaction);
        void DeleteFXTransaction(TransactionsBO transaction);
        void RestoreFXTransaction(TransactionsBO transaction);
        SecuritiesDIM GetSecurityInfo(string symbol);
        bool SecurityExists(string symbol);
        TransactionTypeDIM GetTradeType(string name);
        CustodiansDIM GetCustodian(string name);
    }
}
