using Microsoft.Win32;
using PortfolioAce.Domain.Models;
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
            _cmbLoadTypes = new List<string> { "Transactions", "Prices", "Securities" };
            _selectedLoadType = _cmbLoadTypes[0];
            BrowseWindowExplorerCommand = new ActionCommand(SelectCSVFile);
            _allFunds = _staticReferences.GetAllFundsReference();
        }

        private List<Fund> _allFunds;
        public List<Fund> FundList
        {
            get
            {
                return _allFunds;
            }
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

        private readonly List<string> _cmbLoadTypes;
        public List<string> cmbLoadTypes
        {
            get
            {
                return _cmbLoadTypes;
            }
        }

        private string _selectedLoadType;
        public string SelectedLoadType
        {
            get
            {
                return _selectedLoadType;
            }
            set
            {
                _selectedLoadType = value;
                OnPropertyChanged(nameof(SelectedLoadType));
            }
        }

        private Fund _selectedFund;
        public Fund SelectedFund
        {
            get
            {
                return _selectedFund;
            }
            set
            {
                _selectedFund = value;
                OnPropertyChanged(nameof(SelectedFund));
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
