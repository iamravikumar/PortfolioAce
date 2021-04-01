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
                string? domicile = (_addInvestorVM.Domicile != null)?_addInvestorVM.Domicile.EnglishName:null;
                if (_addInvestorVM.FullName == "")
                {
                    throw new ArgumentException("You must provide a full name");
                }

                InvestorsDIM newInvestor = new InvestorsDIM
                {
                    FullName = _addInvestorVM.FullName,
                    BirthDate = _addInvestorVM.BirthDate,
                    Domicile = domicile,
                    Email = _addInvestorVM.Email,
                    MobileNumber = _addInvestorVM.MobileNumber,
                    NativeLanguage = _addInvestorVM.NativeLanguage
                };
                var savedInvestor = await _investorService.CreateInvestor(newInvestor);
                if (savedInvestor.InvestorId != 0)
                {
                    MessageBox.Show($"{_addInvestorVM.FullName} has been saved.");
                    _addInvestorVM.ResetValues(selectInvestorName: _addInvestorVM.FullName);
                }
                else
                {
                    MessageBox.Show($"ERROR INVNOTSAVED");
                    _addInvestorVM.CloseAction();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
