using PortfolioAce.Models;
using PortfolioAce.ViewModels;
using PortfolioAce.ViewModels.Factories;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Navigation
{
    public class Navigator : ObservableObject, INavigator
    {

        private IWindowFactory _windowFactory;

        public Navigator(IPortfolioAceViewModelAbstractFactory viewModelFactory, IWindowFactory windowFactory)
        {
            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(this, viewModelFactory);

            _windowFactory = windowFactory;

            ShowSettingsCommand = new ActionCommand(ShowSettingsWindow);
            ShowNewFundCommand = new ActionCommand(ShowNewFundWindow);
            ShowSecurityManagerCommand = new ActionCommand(ShowSecurityManagerWindow);
            ShowAboutCommand = new ActionCommand(ShowAboutWindow);
            ShowImportDataToolCommand = new ActionCommand(ShowImportDataToolWindow);
            CloseApplicationCommand = new ActionCommand(CloseApplication);
            ShowNewInvestorCommand = new ActionCommand(ShowNewInvestorWindow);
        }

        public ICommand CloseApplicationCommand { get; }
        public ICommand UpdateCurrentViewModelCommand { get; }
        public ICommand ShowSettingsCommand { get; }
        public ICommand ShowNewFundCommand { get; }
        public ICommand ShowAboutCommand { get; }
        public ICommand ShowImportDataToolCommand { get; }
        public ICommand ShowSecurityManagerCommand { get; }
        public ICommand ShowNewInvestorCommand { get; }

        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                _currentViewModel?.Dispose();
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public void CloseApplication()
        {
            Application.Current.MainWindow.Close();
        }
        public void ShowImportDataToolWindow()
        {
            _windowFactory.CreateImportDataToolWindow();
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
