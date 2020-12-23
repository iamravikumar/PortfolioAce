using PortfolioAce.EFCore.Services.SettingServices;
using PortfolioAce.ViewModels.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class ApplyConnectionSettingsCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private SettingsWindowViewModel _settingsWindowVM;
        private ISettingService _settingService;


        public ApplyConnectionSettingsCommand(SettingsWindowViewModel settingsWindowVM,
            ISettingService settingService)
        {
            _settingsWindowVM = settingsWindowVM;
            _settingService = settingService;
        }

        public bool CanExecute(object parameter)
        {
            return true; // true for now
        }

        public void Execute(object parameter)
        {
            try
            {
                string avKeyValue = _settingsWindowVM.AlphaVantageKey;
                string FMPKeyValue = _settingsWindowVM.FMPrepKey;
                _settingService.UpdateAPIKeys(avKeyValue, FMPKeyValue);
                _settingsWindowVM.CloseAction();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
