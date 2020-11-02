using PortfolioAce.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.ViewModels.Factories
{
    public class PortfolioAceViewModelAbstractFactory : IPortfolioAceViewModelAbstractFactory
    {
        public ViewModelBase CreateViewModel(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Home:
                    return new HomeViewModel();
                    
                case ViewType.About:
                    return new AboutViewModel();
                   
                default:
                    throw new ArgumentException("The ViewType does not have a ViewModel", "viewType");
            }
        }
    }
}
