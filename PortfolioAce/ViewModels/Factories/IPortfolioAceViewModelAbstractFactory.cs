using PortfolioAce.Navigation;

namespace PortfolioAce.ViewModels.Factories
{
    public interface IPortfolioAceViewModelAbstractFactory
    {
        ViewModelBase CreateViewModel(ViewType viewType);
    }
}
