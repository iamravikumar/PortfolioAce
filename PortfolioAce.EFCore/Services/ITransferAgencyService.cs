using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.BackOfficeModels;
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

        void InitialiseFundAction(Fund fund, List<TransferAgencyBO> investors, List<TransactionsBO> transactions, NAVPriceStoreFACT initialNav);
        List<TransferAgencyBO> GetAllFundInvestorActions(int fundId);

    }
}
