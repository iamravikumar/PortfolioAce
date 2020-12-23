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

        public Navigator(IPortfolioAceViewModelAbstractFactory viewModelFactory, IFundService fundService, ITransferAgencyService investorService,
            IAdminService adminService, ISettingService settingService, IStaticReferences staticReferences)
        {
            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(this, viewModelFactory);
            _fundService = fundService;
            _adminService = adminService;
            _staticReferences = staticReferences;
            _investorService = investorService;
            _settingService = settingService;
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
            Window view = new ImportTradesWindow();
            ViewModelBase viewModel = new ImportTradesViewModel();
            view.DataContext = viewModel;
            view.Owner = Application.Current.MainWindow;
            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            view.ShowDialog();
        }

        public void ShowSettingsWindow()
        {
            Window view = new SettingsWindow();
            ViewModelWindowBase viewModel = new SettingsWindowViewModel(_settingService);
            view.DataContext = viewModel;
            view.Owner = Application.Current.MainWindow;
            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (viewModel.CloseAction == null)
            {
                viewModel.CloseAction = new Action(() => view.Close());
            }
            view.ShowDialog();
        }

        public void ShowNewFundWindow()
        {
            Window view = new AddFundWindow();
            ViewModelWindowBase viewModel = new AddFundWindowViewModel(_fundService, _staticReferences);
            view.DataContext = viewModel;
            view.Owner = Application.Current.MainWindow;
            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (viewModel.CloseAction == null)
            {
                viewModel.CloseAction = new Action(() => view.Close());
            }
            
            view.ShowDialog();
        }

        public void ShowNewInvestorWindow()
        {
            Window view = new AddInvestorWindow();
            ViewModelWindowBase viewModel = new AddInvestorWindowViewModel(_investorService);
            view.DataContext = viewModel;
            view.Owner = Application.Current.MainWindow;
            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (viewModel.CloseAction == null)
            {
                viewModel.CloseAction = new Action(() => view.Close());
            }

            view.ShowDialog();
        }

        public void ShowSecurityManagerWindow()
        {
            Window view = new SecurityManagerWindow();
            ViewModelBase viewModel = new SecurityManagerWindowViewModel(_adminService, _staticReferences);
            view.DataContext = viewModel;
            view.Owner = Application.Current.MainWindow;
            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            view.ShowDialog();
        }

        public void ShowAboutWindow()
        {
            Window view = new AboutWindow();
            ViewModelBase viewModel = new AboutWindowViewModel();
            view.DataContext = viewModel;
            view.Owner = Application.Current.MainWindow;
            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            view.ShowDialog();
        }
    }
}
