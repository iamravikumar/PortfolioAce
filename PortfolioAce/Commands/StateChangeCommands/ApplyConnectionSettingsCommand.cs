using PortfolioAce.EFCore.Services.SettingServices;
using PortfolioAce.ViewModels.Windows;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class ApplyConnectionSettingsCommand : AsyncCommandBase
    {
        private SettingsWindowViewModel _settingsWindowVM;
        private ISettingService _settingService;


        public ApplyConnectionSettingsCommand(SettingsWindowViewModel settingsWindowVM,
            ISettingService settingService)
        {
            _settingsWindowVM = settingsWindowVM;
            _settingService = settingService;
        }

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object parameter)
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
