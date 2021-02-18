using Microsoft.Win32;
using PortfolioAce.EFCore.Services.DimensionServices;
using PortfolioAce.Models;
using PortfolioAce.Navigation;
using PortfolioAce.ViewModels.Factories;
using PortfolioAce.ViewModels.Modals;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;

namespace PortfolioAce.ViewModels.Windows
{
    public class ImportDataToolViewModel : ViewModelWindowBase, IFileDropTargetHelper
    {

        private IStaticReferences _staticReferences;
        public ImportDataToolViewModel(IStaticReferences staticReferences)
        {
            _staticReferences = staticReferences;
            BrowseWindowExplorerCommand = new ActionCommand(SelectCSVFile);
        }

        public ICommand BrowseWindowExplorerCommand { get; set; }

        private FileInfo _targetFile;
        public FileInfo TargetFile
        {
            get
            {
                return _targetFile;
            }
            set
            {
                _targetFile = value;
                OnPropertyChanged(nameof(TargetFile));
                OnPropertyChanged(nameof(CurrentFileDescription));
            }
        }

        public string CurrentFileDescription
        {
            get
            {
                if (_targetFile != null)
                {
                    return $"Current File: {_targetFile.Name}";
                }
                else
                {
                    return "";
                }
            }
        }

        public List<string> cmbLoadTypes
        {
            get
            {
                return new List<string> {"Transactions", "Prices", "Securities" };
            }
        }

        private void SelectCSVFile()
        {
            // TODO: this violates MVVM but i cannot think of better solution at the moment. since this is technically a dialog on top of a dialog
            OpenFileDialog windowExplorer = new OpenFileDialog();
            windowExplorer.DefaultExt = ".csv";
            windowExplorer.Filter = "Comma Seperated Values File (*.csv)|*.csv";

            if (windowExplorer.ShowDialog() == true)
            {
                FileInfo file = new FileInfo(windowExplorer.FileName);
                if (file.Name.EndsWith(".csv"))
                {
                    TargetFile = file;
                }

            }
        }
        public void OnFileDrop(string[] filepaths)
        {
            if (filepaths.Length == 1)
            {
                string fileName = filepaths[0];
                if (fileName.EndsWith(".csv"))
                {
                    FileInfo file = new FileInfo(fileName);
                    TargetFile = file;
                }
            }
        }
    }
}
