using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

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
            IList DataGridData = (IList)parameter;
            if (DataGridData.Count > 0)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "JavaScript Object Notation (*.json)|*.json";
                if (saveFileDialog.ShowDialog() == true)
                {
                    string json = JsonConvert.SerializeObject(DataGridData, Formatting.Indented);
                    await System.IO.File.WriteAllTextAsync(saveFileDialog.FileName, json);
                }
            }
            else
            {
                MessageBox.Show("The DataGrid is empty");
            }
        }
    }
}
