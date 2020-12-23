using PortfolioAce.ViewModels.Modals;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.ViewModels.Windows
{
    public class SettingsWindowViewModel: ViewModelWindowBase
    {
        public SettingsWindowViewModel()
        {
        }


        public string _AlphaVantageKey;
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
            }
        }

        public string _FMPrepKey;
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
            }
        }
    }
}
