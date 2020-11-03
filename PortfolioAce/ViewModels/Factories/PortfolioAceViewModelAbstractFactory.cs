using PortfolioAce.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.ViewModels.Factories
{
    public class PortfolioAceViewModelAbstractFactory : IPortfolioAceViewModelAbstractFactory
    {

        private readonly CreateViewModel<HomeViewModel> _createHomeViewModel;
        private readonly CreateViewModel<AllFundsViewModel> _createAllFundsViewModel;
        private readonly CreateViewModel<SystemFXRatesViewModel> _createFXViewModel;
        private readonly CreateViewModel<SystemSecurityPricesViewModel> _createSPViewModel;

        public PortfolioAceViewModelAbstractFactory(
            CreateViewModel<HomeViewModel> createHomeViewModel,
            CreateViewModel<AllFundsViewModel> createAllFundsViewModel,
            CreateViewModel<SystemFXRatesViewModel> createFXViewModel,
            CreateViewModel<SystemSecurityPricesViewModel> createSPViewModel
)
        {
            _createHomeViewModel = createHomeViewModel;
            _createAllFundsViewModel = createAllFundsViewModel;
            _createFXViewModel = createFXViewModel;
            _createSPViewModel = createSPViewModel;
        }

        public ViewModelBase CreateViewModel(ViewType viewType)
        {

            switch (viewType)
            {
                case ViewType.Home:
                    return _createHomeViewModel();

                case ViewType.FundsView:
                    return _createAllFundsViewModel();

                case ViewType.SysFXRates:
                    return _createFXViewModel();

                case ViewType.SysSecurityPrices:
                    return _createSPViewModel();

                   
                default:
                    throw new ArgumentException("The ViewType does not have a ViewModel", "viewType");
            }
        }
    }
}
