using PortfolioAce.Domain.Models;
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

        List<IssueTypesDIM> GetAllIssueTypes();
        Dictionary<(SecuritiesDIM, DateTime), List<SecurityPriceStore>> GetAllSecurityPrices();
        List<TransactionTypeDIM> GetAllTransactionTypes();
        List<CustodiansDIM> GetAllCustodians();
        SecuritiesDIM GetSecurityInfo(string symbol);
        AssetClassDIM GetAssetClass(string name);
        CurrenciesDIM GetCurrency(string symbol);
        TransactionTypeDIM GetTransactionType(string typeName);
        CustodiansDIM GetCustodian(string typeName);
    }
}
