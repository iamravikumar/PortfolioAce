﻿using PortfolioAce.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PortfolioAce.ViewModels
{
    public class MainViewModel: ViewModelBase
    {
        public INavigator Navigator { get; set; }
    
        public MainViewModel(INavigator navigator)
        {
            Navigator = navigator;
            Navigator.UpdateCurrentViewModelCommand.Execute(ViewType.Home);

        }


    }
}
