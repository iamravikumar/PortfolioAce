using PortfolioAce.Domain.DataObjects;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.Domain.Models.FactTables;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services
{
    public interface ITransferAgencyService
    {
        Task<TransferAgencyBO> GetInvestorActionById(int id);
        Task<TransferAgencyBO> CreateInvestorAction(TransferAgencyBO investorAction);
        Task<TransferAgencyBO> UpdateInvestorAction(TransferAgencyBO investorAction);
        Task<TransferAgencyBO> DeleteInvestorAction(int id);


        Task<InvestorsDIM> CreateInvestor(InvestorsDIM investor);

        void InitialiseFundAction(Fund fund, List<TransferAgencyBO> investorSubscriptions, List<TransactionsBO> transactions, NAVPriceStoreFACT initialNav, List<FundInvestorBO> fundInvestors);

        void LockNav(NavValuations navValuations);

        List<TransferAgencyBO> GetAllFundInvestorActions(int fundId);
        List<InvestorsDIM> GetAllInvestors();

    }
}
