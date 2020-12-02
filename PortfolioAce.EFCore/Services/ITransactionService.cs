using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services
{
    public interface ITransactionService
    {
        Task<TransactionsBO> CreateTransaction(TransactionsBO transaction);
        SecuritiesDIM GetSecurityInfo(string symbol);
        bool SecurityExists(string symbol);
        TransactionTypeDIM GetTradeType(string name);
        CustodiansDIM GetCustodian(string name);
    }
}
