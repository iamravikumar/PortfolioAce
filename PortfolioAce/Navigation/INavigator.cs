using PortfolioAce.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PortfolioAce.Navigation
{
    public enum ViewType
    {
        Home
        
    }
    public interface INavigator
    {
        ViewModelBase CurrentViewModel { get; set; }
        ICommand UpdateCurrentViewModelCommand { get; }

        void ShowSettingsWindow();
        void ShowNewFundWindow();
        void ShowAboutWindow();
        void ShowImportTradesWindow();
    }
}
