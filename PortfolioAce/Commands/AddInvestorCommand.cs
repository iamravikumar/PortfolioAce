using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.EFCore.Services;
using PortfolioAce.ViewModels.Modals;
using System;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class AddInvestorCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private AddInvestorWindowViewModel _addInvestorVM;
        private ITransferAgencyService _investorService;

        public AddInvestorCommand(AddInvestorWindowViewModel addInvestorVM,
    ITransferAgencyService investorService)
        {
            _addInvestorVM = addInvestorVM;
            _investorService = investorService;
        }

        public bool CanExecute(object parameter)
        {
            return true; // true for now
        }

        public async void Execute(object parameter)
        {
            try
            {
                InvestorsDIM newInvestor = new InvestorsDIM
                {
                    FullName = _addInvestorVM.FullName,
                    BirthDate = _addInvestorVM.BirthDate,
                    Domicile = _addInvestorVM.Domicile,
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
