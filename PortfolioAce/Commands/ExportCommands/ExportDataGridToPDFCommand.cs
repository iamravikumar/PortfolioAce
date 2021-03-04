using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PortfolioAce.Commands.ExportCommands
{
    public class ExportDataGridToPDFCommand:AsyncCommandBase
    {
        public ExportDataGridToPDFCommand()
        {
        }

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
