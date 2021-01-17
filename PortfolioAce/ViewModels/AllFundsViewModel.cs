using PortfolioAce.Commands;
using PortfolioAce.Domain.BusinessServices;
using PortfolioAce.Domain.DataObjects;
using PortfolioAce.Domain.DataObjects.PositionData;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.Domain.Models.FactTables;
using PortfolioAce.EFCore.Services;
using PortfolioAce.EFCore.Services.DimensionServices;
using PortfolioAce.EFCore.Services.FactTableServices;
using PortfolioAce.EFCore.Services.PriceServices;
using PortfolioAce.Navigation;
using PortfolioAce.ViewModels.Factories;
using PortfolioAce.ViewModels.Modals;
using PortfolioAce.ViewModels.Windows;
using PortfolioAce.Views.Modals;
using PortfolioAce.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace PortfolioAce.ViewModels
{
    public class AllFundsViewModel : ViewModelBase
    {
        private IFundService _fundService;
        private IPortfolioService _portfolioService;
        private IStaticReferences _staticReferences;
        private ITransactionService _transactionService;
        private IWindowFactory _windowFactory;

        public AllFundsViewModel(IFundService fundService,
            ITransactionService transactionService, IPortfolioService portfolioService,
            IStaticReferences staticReferences, IWindowFactory windowFactory)
        {
            _fundService = fundService;
            _portfolioService = portfolioService;
            _transactionService = transactionService;
            _staticReferences = staticReferences;
            _windowFactory = windowFactory;

            _currentFund = (lbFunds.Count != 0) ? _fundService.GetFund(lbFunds[0]) : null;
            _asOfDate = (_currentFund != null)?staticReferences.GetMostRecentLockedDate(_currentFund.FundId):DateTime.Today;

            SelectFundCommand = new SelectFundCommand(this, fundService);

            ShowNewTradeCommand = new ActionCommand(OpenNewTradeWindow);

            ShowEditTradeCommand = new ActionCommand(OpenEditTradeWindow);

            ShowDeleteTradeCommand = new ActionCommand(DeleteTradeDialog);

            ShowRestoreTradeCommand = new ActionCommand(RestoreTradeDialog);

            ShowNewFXTradeCommand = new ActionCommand(OpenNewFxTradeWindow);
            ShowNewCashTradeCommand = new ActionCommand(OpenNewCashTradeWindow);
            ShowEditCashTradeCommand = new ActionCommand(OpenEditCashTradeWindow);
            ShowNewInvestorActionCommand = new ActionCommand(OpenInvestorActionWindow);
            ShowFundInitialisationCommand = new ActionCommand(OpenFundInitialisationWindow);
            ShowFundPropertiesCommand = new ActionCommand(OpenFundPropertiesWindow);

            ShowFundMetricsCommand = new ActionCommand(OpenMetricsWindow);

            ShowPositionDetailsCommand = new ActionCommand(OpenPositionDetailWindow);

            ShowNavSummaryCommand = new ActionCommand(OpenNavSummaryWindow);

        }

        public ICommand ShowPositionDetailsCommand { get; set; }
        public ICommand SelectFundCommand { get; set; }
        public ICommand ShowNewTradeCommand { get; set; }
        public ICommand ShowEditTradeCommand { get; set; }
        public ICommand ShowRestoreTradeCommand { get; set; }
        public ICommand ShowDeleteTradeCommand { get; set; }
        public ICommand ShowNewFXTradeCommand { get; set; }

        public ICommand ShowNewCashTradeCommand { get; set; }
        public ICommand ShowEditCashTradeCommand { get; set; }
        public ICommand ShowFundPropertiesCommand { get; set; }
        public ICommand ShowFundInitialisationCommand { get; set; }
        public ICommand ShowNewInvestorActionCommand { get; set; }
        public ICommand ShowNavSummaryCommand { get; set; }
        public ICommand ShowFundMetricsCommand { get; set; }

        private DateTime _asOfDate;
        public DateTime asOfDate
        {
            get
            {
                return _asOfDate;
            }
            set
            {
                _asOfDate = value;
                OnPropertyChanged("");
            }
        }

        public NavValuations NavValuation
        {
            // with this is the fund is unlocked then i can create Estimate NAV as of XXX. Estimate Nav pershare as of:
            get
            {
                return new NavValuations(dgFundPositions, dgFundCashHoldings,dgFundClients, _asOfDate, _currentFund); ;
            }
        }



        //private Dictionary<(string, DateTime), decimal> _priceTable;
        public Dictionary<(string, DateTime), decimal> priceTable
        {
            get
            {
                return _staticReferences.GetPriceTable(_asOfDate);
            }
        }
        

        // List box click should have a command and that command changes the fields displayed on the right of the allfundsview.
        public List<string> lbFunds
        {
            get
            {
                return _fundService.GetAllFundSymbols();
            }
        }

        private Fund _currentFund;
        public Fund CurrentFund
        {
            get
            {
                return _currentFund;
            }
            set
            {
                _currentFund = value;
                OnPropertyChanged(""); // This updates all onproperty changed properties
            }
        }

        
        public bool IsInitialised
        {
            // need to find a way to incorporate no fund option so it doesn't break
            get
            {
                return (_currentFund != null) ? _currentFund.IsInitialised : false;
            }
            private set
            {

            }
        }

        public NAVPriceStoreFACT CurrentNavPrice
        {
            get
            {
                return (_currentFund != null) ? _currentFund.NavPrices.Where(np=>np.FinalisedDate==_asOfDate).FirstOrDefault():null; //maybe nav periods...
            }
            private set
            {

            }
        }

        public Visibility LockedNav
        {
            // true = this will put a green tick is the nav is locked which means the price and amount you see are finalised...
            // false = this will put a red cross if the nav is unlocked which means the price you see is not yet final
            get
            {
                if(_currentFund != null)
                {
                    AccountingPeriodsDIM period = _currentFund.NavPrices.Where(np => np.NAVPeriod.AccountingDate == _asOfDate).Select(np=>np.NAVPeriod).FirstOrDefault();
                    if(period != null)
                    {
                        return (period.isLocked) ? Visibility.Visible : Visibility.Collapsed;
                    }
                }
                return Visibility.Collapsed;
            }
            private set
            {
            }
        }

        public bool EnableFundDataMetrics
        {
            // true = this will put a green tick is the nav is locked which means the price and amount you see are finalised...
            // false = this will put a red cross if the nav is unlocked which means the price you see is not yet final
            get
            {
                if (_currentFund != null)
                {
                    AccountingPeriodsDIM period = _currentFund.NavPrices.Where(np => np.NAVPeriod.AccountingDate == _asOfDate).Select(np => np.NAVPeriod).FirstOrDefault();
                    if (period != null)
                    {
                        return (period.isLocked);
                    }
                }
                return false;
            }
        }

        public Visibility ShowWidget
        {
            get
            {
                return (IsInitialised)?Visibility.Visible: Visibility.Collapsed;
            }
            private set
            {

            }
        }
        
        public List<ValuedCashPosition> dgFundCashHoldings
        {
            get
            {
                if(_currentFund != null)
                {
                    return _portfolioService.GetAllValuedCashPositions(_currentFund, _asOfDate, priceTable);
                }
                else
                {
                    return new List<ValuedCashPosition>();
                }
            }
        }

        public List<ClientHolding> dgFundClients
        {
            get
            {
                if (_currentFund != null)
                {
                    List<ClientHolding> allClientHoldings = _portfolioService.GetAllClientHoldings(_currentFund, asOfDate);
                    return allClientHoldings; // This is temporary until I make the NaValuation live in this view...
                }
                else
                {
                    return new List<ClientHolding>();
                }
            }
        }

        public List<ValuedSecurityPosition> dgFundPositions
        {
            get
            {
                if (_currentFund != null)
                {
                    return _portfolioService.GetAllValuedSecurityPositions(_currentFund, _asOfDate, priceTable);
                }
                else
                {
                    return new List<ValuedSecurityPosition>();
                }
            }
        }
        private ListCollectionView _groupedPositions;
        public ListCollectionView groupedPositions
        {
            get
            {
                ListCollectionView cv = new ListCollectionView(dgFundPositions);
                cv.GroupDescriptions.Add(new PropertyGroupDescription("Position.Security.AssetClass"));
                return cv;
            }
        }


        public List<TransactionsBO> dgFundTrades
        {
            get
            {
                // Transactions filtered for just security trades
                return (_currentFund != null) ? _currentFund.Transactions
                                                            .Where(t => (t.TransactionType.TypeClass == "SecurityTrade" || t.TransactionType.TypeName=="FXTrade") && t.TradeDate <= _asOfDate)
                                                            .OrderBy(t=>t.TradeDate)
                                                            .ToList() : null;
            }
        }

        private TransactionsBO _selectedTransaction;
        public TransactionsBO selectedTransaction
        {
            get
            {
                return _selectedTransaction;
            }
            set
            {
                _selectedTransaction = value;
                OnPropertyChanged(nameof(selectedTransaction));
            }
        }

        private ValuedSecurityPosition _selectedPosition;
        public ValuedSecurityPosition selectedPosition
        {
            get
            {
                return _selectedPosition;
            }
            set
            {
                _selectedPosition = value;
                OnPropertyChanged(nameof(selectedPosition));
            }
        }

        public List<TransactionsBO> dgFundCashBook
        {
            get
            {
                // all Transactions ordered by date... 
                return (_currentFund != null) ? _currentFund.Transactions
                                                            .Where(t=>t.TradeDate<=_asOfDate && t.TransactionType.TypeClass!="FXTrade")
                                                            .OrderBy(t => t.TradeDate)
                                                            .ToList() : null;
            }
        }


        public List<TransferAgencyBO> dgFundTA
        {
            get
            {
                return (_currentFund != null) ? _currentFund.TransferAgent
                                                            .Where(ta=>ta.TransactionDate<=_asOfDate)
                                                            .OrderBy(ta => ta.TransactionDate)
                                                            .ToList() : null;
            }
        }

        public void OpenNewTradeWindow()
        {
            _windowFactory.CreateNewTradeWindow(_currentFund);
            SelectFundCommand.Execute(_currentFund.Symbol);
        }

        public void OpenEditTradeWindow()
        {
            if (_selectedTransaction.Security.AssetClass.Name == "FXForward")
            {
                MessageBox.Show("Editing FX Trades are currently not supported", "Information");
            }
            else
            {
                _windowFactory.CreateEditTradeWindow(_selectedTransaction);
                OnPropertyChanged("");
            }

        }

        public void OpenNewFxTradeWindow()
        {
            _windowFactory.CreateNewFXTradeWindow(_currentFund);
            SelectFundCommand.Execute(_currentFund.Symbol);
        }

        public void OpenNewCashTradeWindow()
        {
            _windowFactory.CreateNewCashTradeWindow(_currentFund);
            SelectFundCommand.Execute(_currentFund.Symbol);
        }

        public void OpenEditCashTradeWindow()
        {
            _windowFactory.CreateEditCashTradeWindow(_selectedTransaction);
            OnPropertyChanged("");
        }

        public void OpenInvestorActionWindow()
        {
            _windowFactory.CreateNewInvestorActionWindow(_currentFund);
            OnPropertyChanged("");
        }

        public void OpenFundInitialisationWindow()
        {
            _windowFactory.CreateFundInitialisationWindow(_currentFund);
            SelectFundCommand.Execute(_currentFund.Symbol);
        }

        public void OpenMetricsWindow()
        {
            _windowFactory.CreateFundMetricsWindow(_currentFund, _asOfDate);
        }

        public void OpenFundPropertiesWindow()
        {
            _windowFactory.CreateFundPropertiesWindow(_currentFund);
        }

        public void OpenNavSummaryWindow()
        {
            _windowFactory.CreateNavSummaryWindow(NavValuation);
            SelectFundCommand.Execute(_currentFund.Symbol);
        }


        public void OpenPositionDetailWindow()
        {
            _windowFactory.CreatePositionDetailsWindows(_selectedPosition, _currentFund);
        }

        public void DeleteTradeDialog()
        {
            string n = Environment.NewLine;
            string message;
            DateTime tradeDate = _selectedTransaction.TradeDate;
            int tradeFundId = _selectedTransaction.FundId;
            if (!_selectedTransaction.isCashTransaction)
            {
                string secSymbol = _selectedTransaction.Security.Symbol;
                string secName = _selectedTransaction.Security.SecurityName;
                decimal quantity = _selectedTransaction.Quantity;
                decimal price = _selectedTransaction.Price;
                message = $"Are you sure you want to delete the following Trade:{n}Security: {secName} ({secSymbol}){n}Quantity: {quantity}{n}Price: {price}";
            }
            else
            {
                string tradeType = _selectedTransaction.TransactionType.TypeName; 
                string cashSymbol = _selectedTransaction.Currency.Symbol;
                decimal amount = _selectedTransaction.TradeAmount;
                message = $"Are you sure you want to delete the following Trade:{n}Type: {tradeType}{n}Security: {cashSymbol}{n}Transaction Amount: {amount}{n}";
            }
            MessageBoxResult result =  MessageBox.Show(message, "Delete Trade", button: MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    if (!_staticReferences.GetPeriod(tradeDate, tradeFundId).isLocked)
                    {
                        if (_selectedTransaction.Security.AssetClass.Name == "FXForward")
                        {
                            _transactionService.DeleteFXTransaction(_selectedTransaction);
                        }
                        else
                        {
                            _transactionService.DeleteTransaction(_selectedTransaction); 
                        }
                        SelectFundCommand.Execute(_currentFund.Symbol);
                    }
                    else
                    {
                        MessageBox.Show($"To Delete this trade the period of {tradeDate.ToString("dd-MM-yyyy")} must be unlocked", "Unable to Restore");
                    }
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        public void RestoreTradeDialog()
        {
            DateTime tradeDate = _selectedTransaction.TradeDate;
            int tradeFundId = _selectedTransaction.FundId;
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to restore this trade?", "Restore Trade", button: MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    if (!_staticReferences.GetPeriod(tradeDate, tradeFundId).isLocked)
                    {
                        if (_selectedTransaction.Security.AssetClass.Name == "FXForward")
                        {
                            _transactionService.RestoreFXTransaction(_selectedTransaction);
                        }
                        else
                        {
                            _transactionService.RestoreTransaction(_selectedTransaction);
                        }

                        SelectFundCommand.Execute(_currentFund.Symbol);
                    }
                    else
                    {
                        MessageBox.Show($"To Restore this trade the period of {tradeDate.ToString("dd-MM-yyyy")} must be unlocked", "Unable to Restore");
                    }
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
    }
}
