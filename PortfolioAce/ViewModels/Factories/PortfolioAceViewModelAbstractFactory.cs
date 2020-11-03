using PortfolioAce.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.ViewModels.Factories
{
    public class PortfolioAceViewModelAbstractFactory : IPortfolioAceViewModelAbstractFactory
    {

        private readonly CreateViewModel<HomeViewModel> _createHomeViewModel;


        public PortfolioAceViewModelAbstractFactory(
            CreateViewModel<HomeViewModel> createHomeViewModel
)
        {
            _createHomeViewModel = createHomeViewModel;

        }

        public ViewModelBase CreateViewModel(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Home:
                    return _createHomeViewModel();
                    

                   
                default:
                    throw new ArgumentException("The ViewType does not have a ViewModel", "viewType");
            }
        }
    }
}
