using PortfolioAce.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.ViewModels
{
    public class MainViewModel: ViewModelBase
    {
        public INavigator Navigator { get; set; } = new Navigator();
    }
}
