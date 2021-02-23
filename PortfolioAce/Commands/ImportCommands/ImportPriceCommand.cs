using PortfolioAce.EFCore.Services;
using PortfolioAce.EFCore.Services.DimensionServices;
using PortfolioAce.ViewModels.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PortfolioAce.Commands.ImportCommands
{
    public class ImportPriceCommand:ICommand
    {
        public event EventHandler CanExecuteChanged;

        private ImportDataToolViewModel _importVM;
        private IAdminService _adminService;
        private IStaticReferences _staticReferences;

        public ImportPriceCommand(ImportDataToolViewModel importVM,
             IStaticReferences staticReferences, IAdminService adminService)
        {
            _importVM = importVM;
            _adminService = adminService;
            _staticReferences = staticReferences;
        }

        public bool CanExecute(object parameter)
        {
            return true; // for now
        }

        public void Execute(object parameter)
        {

            try
            {

            }
            catch (Exception e)
            {
            }
        }
    }
}
