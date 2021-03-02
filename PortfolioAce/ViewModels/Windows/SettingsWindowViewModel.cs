using PortfolioAce.Commands;
using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore.Services.SettingServices;
using PortfolioAce.Navigation;
using PortfolioAce.ViewModels.Modals;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.ViewModels.Windows
{
    public class SettingsWindowViewModel : ViewModelWindowBase
    {
        private ISettingService _settingService;
        private Dictionary<string, ApplicationSettings> _settingsTable;
        public SettingsWindowViewModel(ISettingService settingService)
        {
            ApplyConnectionSettingsCommand = new ApplyConnectionSettingsCommand(this, settingService);


            _settingService = settingService;
            _settingsTable = settingService.GetAllSettings();

            _AlphaVantageKey = _settingsTable["AlphaVantageAPI"].SettingValue;
            _storedAVKey = _settingsTable["AlphaVantageAPI"].SettingValue;

            _FMPrepKey = _settingsTable["FMPrepAPI"].SettingValue;
            _storedFMPKey = _settingsTable["FMPrepAPI"].SettingValue;


            ShowAlphaVantageKeyCommand = new ActionCommand(ShowAlphaVantageKey);
            ShowFMPKeyCommand = new ActionCommand(ShowFMPKey);
        }

        public ICommand ApplyConnectionSettingsCommand { get;}

        private readonly string _storedAVKey;
        private readonly string _storedFMPKey;

        public ICommand ShowAlphaVantageKeyCommand { get;}
        public ICommand ShowFMPKeyCommand { get;}

        private string _AlphaVantageKey;
        public string AlphaVantageKey
        {
            get
            {
                return _AlphaVantageKey;
            }
            set
            {
                _AlphaVantageKey = value;
                OnPropertyChanged(nameof(AlphaVantageKey));
                OnPropertyChanged(nameof(enableUpdateSetting));
            }
        }

        private string _FMPrepKey;
        public string FMPrepKey
        {
            get
            {
                return _FMPrepKey;
            }
            set
            {
                _FMPrepKey = value;
                OnPropertyChanged(nameof(FMPrepKey));
                OnPropertyChanged(nameof(enableUpdateSetting));

            }
        }

        public bool enableUpdateSetting
        {
            get
            {
                return (_storedFMPKey != _FMPrepKey || _storedAVKey != _AlphaVantageKey); // if they do not match it means you can update.
            }
        }

        public void ShowAlphaVantageKey()
        {
            MessageBox.Show(_AlphaVantageKey, "AlphaVantage API Key");
        }

        public void ShowFMPKey()
        {
            MessageBox.Show(_FMPrepKey, "FinancialModeling Prep API Key");
        }
    }
}
