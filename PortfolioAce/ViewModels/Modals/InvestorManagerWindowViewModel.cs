using PortfolioAce.Commands;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.EFCore.Services;
using PortfolioAce.Models;
using PortfolioAce.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.ViewModels.Modals
{
    public class InvestorManagerWindowViewModel : ViewModelWindowBase, INotifyDataErrorInfo
    {
        private ITransferAgencyService _investorService;
        private readonly ValidationErrors _validationErrors;

        public InvestorManagerWindowViewModel(ITransferAgencyService investorService)
        {
            AddInvestorCommand = new AddInvestorCommand(this, investorService);
            _investorService = investorService;
            _birthDate = null;
            _validationErrors = new ValidationErrors();
            _validationErrors.ErrorsChanged += ChangedErrorsEvents;
            _allInvestors = investorService.GetAllInvestors();
            if (_allInvestors.Count > 0)
            {
                _selectedInvestor = _allInvestors.First();
            }
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

        public ICommand AddInvestorCommand { get; set; }



        // This is for the second tab.

        public ICommand LoadInvestorProfileCommand { get; set; }
        private List<InvestorsDIM> _allInvestors;

        public List<InvestorsDIM> lbInvestors
        {
            get
            {
                return _allInvestors;
            }
        }

        private InvestorsDIM _selectedInvestor;
        public InvestorsDIM SelectedInvestor
        {
            get
            {
                return _selectedInvestor;
            }
            set
            {
                _selectedInvestor = value;
                OnPropertyChanged(nameof(SelectedInvestor));
                OnPropertyChanged(nameof(InvestorProfile));
                OnPropertyChanged(nameof(SelectedInvestorEmailLink));
            }
        }

        public string SelectedInvestorEmailLink
        {
            get
            {
                if(_selectedInvestor != null)
                {
                    return $"mailto:{_selectedInvestor.Email}";
                }
                return "";
            }
        }

        public int SelectedInvestorAge
        {
            get
            {
                if (_selectedInvestor != null)
                {
                    if (_selectedInvestor.BirthDate.HasValue)
                    {
                        DateTime today = DateTime.Today;
                        int age = today.Year - _selectedInvestor.BirthDate.Value.Year;

                        if (_selectedInvestor.BirthDate.Value > today.AddYears(-age))
                        {
                            age--;
                        }
                        return age;
                    }
                };
                return 0;
            }
        }

        public string InvestorProfile
        {
            get
            {
                if (_selectedInvestor != null)
                {
                    if(SelectedInvestorAge>0 && _selectedInvestor.Domicile!= null)
                    {
                        return $"{_selectedInvestor.FullName} is a {SelectedInvestorAge} year old from {_selectedInvestor.Domicile}.";
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        public Visibility ShowProfiles
        {
            get
            {
                return (_allInvestors.Count > 0)?Visibility.Visible:Visibility.Collapsed;
            }
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
}
