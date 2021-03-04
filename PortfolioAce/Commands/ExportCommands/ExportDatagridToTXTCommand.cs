using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PortfolioAce.Commands.ExportCommands
{
    public class ExportDataGridToTXTCommand:AsyncCommandBase
    {

        public ExportDataGridToTXTCommand()
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
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text Documents (*.txt)|*.txt";
                if (saveFileDialog.ShowDialog() == true)
                {
                    using (var writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        var config = new CsvConfiguration(CultureInfo.CurrentCulture)
                        {
                            Delimiter = "\t",
                            Encoding = Encoding.UTF8
                        };
                        var txt = new CsvWriter(writer, config);
                        txt.WriteHeader(DataGridData[0].GetType());
                        txt.NextRecord();
                        await txt.WriteRecordsAsync(DataGridData);
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
