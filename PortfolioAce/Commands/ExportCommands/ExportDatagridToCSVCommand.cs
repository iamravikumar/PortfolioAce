using CsvHelper;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace PortfolioAce.Commands.ExportCommands
{
    public class ExportDatagridToCSVCommand:AsyncCommandBase
    {

        public ExportDatagridToCSVCommand()
        {
        }

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object parameter)
        {
            // the parameter will be the observable collection...
            IList x = (IList)parameter;
            if (x.Count > 0)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Comma Seperated Values File (*.csv)|*.csv";
                if (saveFileDialog.ShowDialog() == true)
                {
                    using (var writer = new StreamWriter(saveFileDialog.FileName))
                    using (var csv = new CsvWriter(writer, CultureInfo.CurrentCulture))
                    {
                        csv.WriteHeader(x[0].GetType());
                        csv.NextRecord();
                        await csv.WriteRecordsAsync(x);
                    }
                }
            }
            else
            {
                MessageBox.Show("The DataGrid is empty");
            }

        }
    }
}
