using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services.DimensionServices
{
    public interface IStaticReferences
    {
        // TODO: This looks like a code smell that needs to be fixed
        List<CurrenciesDIM> GetAllCurrencies();
        List<AssetClassDIM> GetAllAssetClasses();
        List<NavFrequencyDIM> GetAllNavFrequencies();

        List<TradeTypesDIM> GetAllTradeTypes();

        List<CashTradeTypesDIM> GetAllCashTradeTypes();
        List<IssueTypesDIM> GetAllIssueTypes();

        List<TransactionTypeDIM> GetAllTransactionTypes();

        AssetClassDIM GetAssetClass(string name);
        CurrenciesDIM GetCurrency(string symbol);
        TransactionTypeDIM GetTransactionType(string typeName);
    }
}
