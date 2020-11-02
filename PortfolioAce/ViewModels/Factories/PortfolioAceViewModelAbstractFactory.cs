using PortfolioAce.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.ViewModels.Factories
{
    public class PortfolioAceViewModelAbstractFactory : IPortfolioAceViewModelAbstractFactory
    {

        private readonly CreateViewModel<HomeViewModel> _createHomeViewModel;
        private readonly CreateViewModel<AboutViewModel> _createAboutViewModel;

        public PortfolioAceViewModelAbstractFactory(
            CreateViewModel<HomeViewModel> createHomeViewModel,
            CreateViewModel<AboutViewModel> createAboutViewModel)
        {
            _createHomeViewModel = createHomeViewModel;
            _createAboutViewModel = createAboutViewModel;
        }

        public ViewModelBase CreateViewModel(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Home:
                    return _createHomeViewModel();
                    
                case ViewType.About:
                    return _createAboutViewModel();
                   
                default:
                    throw new ArgumentException("The ViewType does not have a ViewModel", "viewType");
            }
        }
    }
}
