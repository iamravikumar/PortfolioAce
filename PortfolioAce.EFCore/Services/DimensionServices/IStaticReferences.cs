using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;

namespace PortfolioAce.EFCore.Services.DimensionServices
{
    public interface IStaticReferences : IBaseService
    {
        // TODO: This looks like a code smell that needs to be fixed
        List<CurrenciesDIM> GetAllCurrencies();
        List<AssetClassDIM> GetAllAssetClasses();
        List<NavFrequencyDIM> GetAllNavFrequencies();

        List<IssueTypesDIM> GetAllIssueTypes();
        List<SecurityPriceStore> GetAllSecurityPrices(DateTime asOfDate);

        Dictionary<(string, DateTime), decimal> GetPriceTable(DateTime asOfDate); // this is used to map positions to historical prices
        List<TransactionTypeDIM> GetAllTransactionTypes();
        List<CustodiansDIM> GetAllCustodians();
        List<string> GetSecuritySymbolByAssetClass(string assetClass);
        SecuritiesDIM GetSecurityInfo(string symbol);
        AssetClassDIM GetAssetClass(string name);
        CurrenciesDIM GetCurrency(string symbol);
        TransactionTypeDIM GetTransactionType(string typeName);
        CustodiansDIM GetCustodian(string typeName);
        List<AccountingPeriodsDIM> GetAllFundPeriods(int fundId);
        AccountingPeriodsDIM GetPeriod(DateTime dateTime, int fundId);
        public bool PreviousPeriodLocked(DateTime dateTime, int fundId);
        public DateTime GetMostRecentLockedDate(int fundId);
    }
}
