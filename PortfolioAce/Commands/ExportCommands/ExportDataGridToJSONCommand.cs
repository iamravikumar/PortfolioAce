using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.Commands.ExportCommands
{
    public class ExportDataGridToJSONCommand:AsyncCommandBase
    {
        public ExportDataGridToJSONCommand()
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
