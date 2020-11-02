using PortfolioAce.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.ViewModels.Factories
{
    public interface IPortfolioAceViewModelAbstractFactory
    {
        ViewModelBase CreateViewModel(ViewType viewType);
    }
}
