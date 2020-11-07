using PortfolioAce.EFCore.Repository;
using PortfolioAce.Models;
using PortfolioAce.ViewModels;
using PortfolioAce.ViewModels.Factories;
using PortfolioAce.ViewModels.Windows;
using PortfolioAce.Views;
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
        private IFundRepository _fundRepo;

        public Navigator(IPortfolioAceViewModelAbstractFactory viewModelFactory, IFundRepository fundRepo)
        {
            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(this, viewModelFactory);
            _fundRepo = fundRepo;
            ShowSettingsCommand = new ActionCommand(ShowSettingsWindow);
            ShowNewFundCommand = new ActionCommand(ShowNewFundWindow);
            ShowAboutCommand = new ActionCommand(ShowAboutWindow);
            ShowImportTradesCommand = new ActionCommand(ShowImportTradesWindow);
            CloseApplicationCommand = new ActionCommand(CloseApplication);
        }

        public ICommand CloseApplicationCommand { get; }
        public ICommand UpdateCurrentViewModelCommand { get; set; }
        public ICommand ShowSettingsCommand { get; }
        public ICommand ShowNewFundCommand { get; }
        public ICommand ShowAboutCommand { get; }
        public ICommand ShowImportTradesCommand { get; }


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
            ViewModelBase viewModel = new SettingsWindowViewModel();
            view.DataContext = viewModel;
            view.Owner = Application.Current.MainWindow;
            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            view.ShowDialog();
        }

        public void ShowNewFundWindow()
        {
            Window view = new AddFundWindow();
            ViewModelBase viewModel = new AddFundWindowViewModel(_fundRepo);
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
