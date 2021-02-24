using PortfolioAce.EFCore.Services;
using PortfolioAce.EFCore.Services.DimensionServices;
using PortfolioAce.EFCore.Services.SettingServices;
using PortfolioAce.ViewModels.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PortfolioAce.Commands.ImportCommands
{
    public class ImportSecuritiesCommand:ICommand
    {
        public event EventHandler CanExecuteChanged;

        private ImportDataToolViewModel _importVM;
        private IImportService _importService;
        private IStaticReferences _staticReferences;

        public ImportSecuritiesCommand(ImportDataToolViewModel importVM,
             IStaticReferences staticReferences, IImportService importService)
        {
            _importVM = importVM;
            _importService = importService;
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
