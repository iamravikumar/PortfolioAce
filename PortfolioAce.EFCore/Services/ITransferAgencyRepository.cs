using PortfolioAce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services
{
    public interface ITransferAgencyRepository
    {
        Task<TransferAgency> GetInvestorActionById(int id);
        Task<TransferAgency> CreateInvestorAction(TransferAgency investorAction);
        Task<TransferAgency> UpdateInvestorAction(TransferAgency investorAction);
        Task<TransferAgency> DeleteInvestorAction(int id);
        List<TransferAgency> GetAllFundInvestorActions(int fundId);

    }
}
