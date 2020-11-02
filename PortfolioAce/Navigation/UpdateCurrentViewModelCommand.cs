using PortfolioAce.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace PortfolioAce.Navigation
{
    class UpdateCurrentViewModelCommand:ICommand
    {
        public event EventHandler CanExecutedChanged;
        public event EventHandler CanExecuteChanged;

        private INavigator _navigator;

        public UpdateCurrentViewModelCommand(INavigator navigator)
        {
            _navigator = navigator;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if(parameter is ViewType)
            {
                ViewType viewType = (ViewType)parameter;
                switch (viewType)
                {
                    case ViewType.Home:
                        _navigator.CurrentViewModel = new HomeViewModel();
                        break;
                    case ViewType.About:
                        _navigator.CurrentViewModel = new AboutViewModel();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
