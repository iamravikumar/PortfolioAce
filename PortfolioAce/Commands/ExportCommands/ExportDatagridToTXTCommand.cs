using System;
using System.Collections;
using System.Threading.Tasks;
using System.Windows;

namespace PortfolioAce.Commands.ExportCommands
{
    public class ExportDatagridToTXTCommand:AsyncCommandBase
    {

        public ExportDatagridToTXTCommand()
        {
        }

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object parameter)
        {
            IList DataGridData = (IList)parameter;
            if (DataGridData.Count > 0)
            {

            }
            else
            {
                MessageBox.Show("The DataGrid is empty");
            }
        }
    }
}
