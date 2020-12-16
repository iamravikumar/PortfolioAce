using PortfolioAce.Commands;
using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.EFCore.Services;
using PortfolioAce.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace PortfolioAce.ViewModels.Modals
{
    public class AddInvestorWindowViewModel : ViewModelWindowBase, INotifyDataErrorInfo
    {
        private ITransferAgencyService _investorService;
        private readonly ValidationErrors _validationErrors;

        public AddInvestorWindowViewModel(ITransferAgencyService investorService)
        {
            AddInvestorCommand = new AddInvestorCommand(this, investorService);
            _investorService = investorService;
            _birthDate = null;
            _validationErrors = new ValidationErrors();
            _validationErrors.ErrorsChanged += ChangedErrorsEvents;

        }

        private string _fullName;
        public string FullName
        {
            get
            {
                return _fullName;
            }
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }

        private DateTime? _birthDate;
        public DateTime? BirthDate
        {
            get
            {
                return _birthDate;
            }
            set
            {
                _birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
            }
        }

        private string _domicile;
        public string Domicile
        {
            get
            {
                return _domicile;
            }
            set
            {
                _domicile = value;
                OnPropertyChanged(nameof(Domicile));
            }
        }

        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string _mobileNumber;
        public string MobileNumber
        {
            get
            {
                return _mobileNumber;
            }
            set
            {
                _mobileNumber = value;
                OnPropertyChanged(nameof(MobileNumber));
            }
        }

        private string _nativeLanguage;
        public string NativeLanguage
        {
            get
            {
                return _nativeLanguage;
            }
            set
            {
                _nativeLanguage = value;
                OnPropertyChanged(nameof(NativeLanguage));
            }
        }

        public List<InvestorsDIM> dgInvestors
        {
            get
            {
                return _investorService.GetAllInvestors();
            }
        }

        public ICommand AddInvestorCommand { get; set; }


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
}
