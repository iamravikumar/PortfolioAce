using PortfolioAce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services
{
    public interface ITransferAgencyService
    {
        Task<TransferAgency> GetInvestorActionById(int id);
        Task<TransferAgency> CreateInvestorAction(TransferAgency investorAction);
        Task<TransferAgency> UpdateInvestorAction(TransferAgency investorAction);
        Task<TransferAgency> DeleteInvestorAction(int id);

        void InitialiseFundAction(Fund fund, List<TransferAgency> investors, NAVPriceStore initialNav);
        List<TransferAgency> GetAllFundInvestorActions(int fundId);

    }
}
