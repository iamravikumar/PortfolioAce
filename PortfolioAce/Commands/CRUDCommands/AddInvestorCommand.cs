using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.EFCore.Services;
using PortfolioAce.ViewModels.Modals;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class AddInvestorCommand : AsyncCommandBase
    {
        private InvestorManagerWindowViewModel _addInvestorVM;
        private ITransferAgencyService _investorService;

        public AddInvestorCommand(InvestorManagerWindowViewModel addInvestorVM,
            ITransferAgencyService investorService)
        {
            _addInvestorVM = addInvestorVM;
            _investorService = investorService;
        }

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                InvestorsDIM newInvestor = new InvestorsDIM
                {
                    FullName = _addInvestorVM.FullName,
                    BirthDate = _addInvestorVM.BirthDate,
                    Domicile = _addInvestorVM.Domicile.EnglishName,
                    Email = _addInvestorVM.Email,
                    MobileNumber = _addInvestorVM.MobileNumber,
                    NativeLanguage = _addInvestorVM.NativeLanguage
                };
                await _investorService.CreateInvestor(newInvestor);
                _addInvestorVM.CloseAction();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
