using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore.Services;
using PortfolioAce.Models;
using PortfolioAce.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace PortfolioAce.ViewModels.Modals
{
    public class FundInitialisationWindowViewModel : ViewModelWindowBase, INotifyDataErrorInfo
    {

        private ITransferAgencyService investorService;
        private Fund _fund;
        private readonly ValidationErrors _validationErrors;

        public FundInitialisationWindowViewModel(ITransferAgencyService investorService, Fund fund)
        {
            // take in the fundservice too so i can unpdate the isinitialised property
            this.investorService = investorService;
            this._fund = fund;

            _validationErrors = new ValidationErrors();
            _validationErrors.ErrorsChanged += ChangedErrorsEvents;
            dgSeedingInvestors = new ObservableCollection<SeedingInvestor>();
            ViewSeeds = new ActionCommand(ViewPositionDetails);
        }
        public ICommand ViewSeeds { get; set; }

        public ObservableCollection<SeedingInvestor> dgSeedingInvestors { get; set; }

        public void ViewPositionDetails()
        {
            Console.WriteLine(dgSeedingInvestors);
        }


        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool CanCreate => !HasErrors;

        public bool HasErrors => _validationErrors.HasErrors;

        public IEnumerable GetErrors(string propertyName)
        {
            return _validationErrors.GetErrors(propertyName);
        }

        private void ChangedErrorsEvents(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanCreate));
        }
    }

    public class SeedingInvestor
    {
        public string InvestorName { get; set; }
        public decimal SeedAmount { get; set; }
        public SeedingInvestor(string InvestorName, decimal SeedAmount)
        {
            this.InvestorName = InvestorName;
            this.SeedAmount = SeedAmount;
        }
        public SeedingInvestor()
        {

        }
    }
}
