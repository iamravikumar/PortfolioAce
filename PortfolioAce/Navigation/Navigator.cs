using PortfolioAce.EFCore.Services;
using PortfolioAce.EFCore.Services.DimensionServices;
using PortfolioAce.EFCore.Services.SettingServices;
using PortfolioAce.Models;
using PortfolioAce.ViewModels;
using PortfolioAce.ViewModels.Factories;
using PortfolioAce.ViewModels.Modals;
using PortfolioAce.ViewModels.Windows;
using PortfolioAce.Views;
using PortfolioAce.Views.Modals;
using PortfolioAce.Views.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Navigation
{
    public class Navigator: ObservableObject, INavigator
    {
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }
        private IFundService _fundService;
        private IAdminService _adminService;
        private IStaticReferences _staticReferences;
        private ITransferAgencyService _investorService;
        private ISettingService _settingService;
        private IWindowFactory _windowFactory;

        public Navigator(IPortfolioAceViewModelAbstractFactory viewModelFactory, IFundService fundService, ITransferAgencyService investorService,
            IAdminService adminService, ISettingService settingService, IStaticReferences staticReferences, IWindowFactory windowFactory)
        {
            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(this, viewModelFactory);
            _fundService = fundService;
            _adminService = adminService;
            _staticReferences = staticReferences;
            _investorService = investorService;
            _settingService = settingService;
            _windowFactory = windowFactory;
            // I can make these commands reusable
            ShowSettingsCommand = new ActionCommand(ShowSettingsWindow);
            ShowNewFundCommand = new ActionCommand(ShowNewFundWindow);
            ShowSecurityManagerCommand = new ActionCommand(ShowSecurityManagerWindow);
            ShowAboutCommand = new ActionCommand(ShowAboutWindow);
            ShowImportTradesCommand = new ActionCommand(ShowImportTradesWindow);
            CloseApplicationCommand = new ActionCommand(CloseApplication);
            ShowNewInvestorCommand = new ActionCommand(ShowNewInvestorWindow);
        }

        public ICommand CloseApplicationCommand { get; }
        public ICommand UpdateCurrentViewModelCommand { get; set; }
        public ICommand ShowSettingsCommand { get; }
        public ICommand ShowNewFundCommand { get; }
        public ICommand ShowAboutCommand { get; }
        public ICommand ShowImportTradesCommand { get; }
        public ICommand ShowSecurityManagerCommand { get; set; }
        public ICommand ShowNewInvestorCommand { get; set; }

        public void CloseApplication()
        {
            Application.Current.MainWindow.Close();
        }
        public void ShowImportTradesWindow()
        {
            _windowFactory.CreateImportTradesWindow();
        }

        public void ShowSettingsWindow()
        {
            _windowFactory.CreateSettingsWindow();
        }

        public void ShowNewFundWindow()
        {
            _windowFactory.CreateNewFundWindow();
        }

        public void ShowNewInvestorWindow()
        {
            _windowFactory.CreateNewInvestorWindow();
        }

        public void ShowSecurityManagerWindow()
        {
            _windowFactory.CreateSecurityManagerWindow();
        }

        public void ShowAboutWindow()
        {
            _windowFactory.CreateAboutWindow();
        }
    }
}
