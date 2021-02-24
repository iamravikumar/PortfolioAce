using CsvHelper;
using Microsoft.Win32;
using PortfolioAce.Commands.ImportCommands;
using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore.Services;
using PortfolioAce.EFCore.Services.DimensionServices;
using PortfolioAce.EFCore.Services.SettingServices;
using PortfolioAce.HelperObjects.DeserialisedCSVObjects;
using PortfolioAce.Models;
using PortfolioAce.Navigation;
using PortfolioAce.ViewModels.Factories;
using PortfolioAce.ViewModels.Modals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.ViewModels.Windows
{
    public class ImportDataToolViewModel : ViewModelWindowBase, IFileDropTargetHelper
    {

        private IStaticReferences _staticReferences;
        public ImportDataToolViewModel(IStaticReferences staticReferences, IImportService importService)
        {
            _staticReferences = staticReferences;
            _cmbLoadTypes = new List<string> { "Transactions", "Prices", "Securities" };
            BrowseWindowExplorerCommand = new ActionCommand(SelectCSVFile);
            ExtractFromCSVCommand = new ActionCommand(ExtractCSV);
            ImportPriceCommand = new ImportPriceCommand(this, staticReferences, importService);
            ImportSecuritiesCommand = new ImportSecuritiesCommand(this, staticReferences, importService);

            _allFunds = _staticReferences.GetAllFundsReference();
            dgCSVPrices = new ObservableCollection<PriceImportDataCSV>();
            dgCSVSecurities = new ObservableCollection<SecurityImportDataCSV>();
            dgCSVTransactions = new ObservableCollection<TransactionImportDataCSV>();
        }

        private List<Fund> _allFunds;
        public List<Fund> FundList
        {
            get
            {
                return _allFunds;
            }
        }

        public ICommand ImportSecuritiesCommand { get; set; }
        public ICommand ImportPriceCommand { get; set; }
        public ICommand ExtractFromCSVCommand {get;set;}
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
                if(value != null)
                {
                    _targetFile = value;
                }
                OnPropertyChanged(nameof(TargetFile));
                OnPropertyChanged(nameof(CurrentFileDescription));
                OnPropertyChanged(nameof(ShowLoadButton));
                OnPropertyChanged(nameof(LoadButtonEnabled));
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
                _currentTemplate = "ImportBox";
                _targetFile = null;
                OnPropertyChanged(nameof(SelectedLoadType));
                OnPropertyChanged(nameof(CurrentTemplate));
                OnPropertyChanged(nameof(CurrentFileDescription));
                OnPropertyChanged(nameof(ShowLoadButton));
                OnPropertyChanged(nameof(LoadButtonEnabled));
                ClearCollections();
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

        private string _currentTemplate;
        public string CurrentTemplate
        {
            // TODO: Make this an enum. this needs refactoring though
            get
            {
                return _currentTemplate;
            }
            set
            {
                _currentTemplate = value;
                OnPropertyChanged(nameof(CurrentTemplate));
                OnPropertyChanged(nameof(ShowLoadButton));
                OnPropertyChanged(nameof(LoadButtonEnabled));
            }
        }

        public bool ShowLoadButton
        {
            get
            {
                return (CurrentTemplate!="ImportBox" && CurrentTemplate!=null);
            }
        }

        public bool LoadButtonEnabled
        {
            get
            {
                return (_currentTemplate != "ImportBox" && _targetFile!=null && _selectedFund!=null);
            }
        }

        public ObservableCollection<PriceImportDataCSV> dgCSVPrices { get; set; }
        public ObservableCollection<TransactionImportDataCSV> dgCSVTransactions { get; set; }
        public ObservableCollection<SecurityImportDataCSV> dgCSVSecurities { get; set; }

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
                    SetTemplate();
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
                    SetTemplate();
                }
            }
        }

        
        private void ExtractCSV()
        {
            if(_selectedFund != null)
            {
                // "Transactions", "Prices", "Securities"
                // try catch, need to catch the IOExceptionif file is used by another process
                // if records are empty then theres an issue
                ClearCollections();
                using (var reader = new StreamReader(_targetFile.FullName))
                using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
                {
                    if(_selectedLoadType == "Transactions")
                    {
                        var records = csv.GetRecords<TransactionImportDataCSV>();
                        foreach (TransactionImportDataCSV record in records)
                        {
                            dgCSVTransactions.Add(record);
                        }
                    }
                    else if(_selectedLoadType == "Prices")
                    {
                        IEnumerable<PriceImportDataCSV> records = csv.GetRecords<PriceImportDataCSV>();
                        // if there are no records then raise messagebox, this could also be due to an invalid file.
                        foreach(PriceImportDataCSV record in records)
                        {
                            dgCSVPrices.Add(record);
                        }
                    }
                    else if (_selectedLoadType == "Securities")
                    {
                        var records = csv.GetRecords<SecurityImportDataCSV>();
                        foreach (SecurityImportDataCSV record in records)
                        {
                            dgCSVSecurities.Add(record);
                        }

                    }
                    else
                    {
                        MessageBox.Show("The load type is not recognised.");
                    }
                }
                
            }
            else
            {
                MessageBox.Show("You need to select a valid fund");
            }

        }



        private void ClearCollections()
        {
            dgCSVPrices.Clear();
            dgCSVSecurities.Clear();
            dgCSVTransactions.Clear();
        }

        private void SetTemplate()
        {
            //"Transactions", "Prices", "Securities"
            switch (_selectedLoadType)
            {
                case "Transactions":
                    CurrentTemplate = "TransactionsDockPanel";
                    break;
                case "Prices":
                    CurrentTemplate = "PricesDockPanel";
                    break;
                case "Securities":
                    CurrentTemplate = "SecuritiesDockPanel";
                    break;
                default:
                    CurrentTemplate = "ImportBox";
                    TargetFile = null;
                    break;
            }
        }
    }
}
