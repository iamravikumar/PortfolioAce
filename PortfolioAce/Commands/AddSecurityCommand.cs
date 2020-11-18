using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore.Services;
using PortfolioAce.ViewModels.Modals;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class AddSecurityCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private SecurityManagerWindowViewModel _SecurityManagerVM;
        private IAdminService _adminService;

        public AddSecurityCommand(SecurityManagerWindowViewModel securityManagerVM, 
            IAdminService adminService)
        {
            _SecurityManagerVM = securityManagerVM;
            _adminService = adminService;
        }

        public bool CanExecute(object parameter)
        {
            return true; // for now
        }

        public void Execute(object parameter)
        {
            try
            {
                Security newSecurity = new Security
                {
                    AssetClass = _SecurityManagerVM.AssetClass,
                    Symbol = _SecurityManagerVM.SecuritySymbol,
                    Currency = _SecurityManagerVM.Currency,
                    SecurityName = _SecurityManagerVM.SecurityName,
                    ISIN = _SecurityManagerVM.ISIN
                };
                _adminService.AddSecurityInfo(newSecurity);
                return;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
