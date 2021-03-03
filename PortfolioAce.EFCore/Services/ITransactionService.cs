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
        Task CreateCashTransfer(List<TransactionsBO> transfers);
        Task<TransactionsBO> CreateFXTransaction(ForexDTO fxTransaction);
        Task UpdateTransaction(TransactionsBO transaction);

        Task DeleteTransaction(TransactionsBO transaction);
        Task RestoreTransaction(TransactionsBO transaction);
        Task DeleteFXTransaction(TransactionsBO transaction);
        Task RestoreFXTransaction(TransactionsBO transaction);
        SecuritiesDIM GetSecurityInfo(string symbol);
        bool SecurityExists(string symbol);
        TransactionTypeDIM GetTradeType(string name);
        CustodiansDIM GetCustodian(string name);
    }
}
