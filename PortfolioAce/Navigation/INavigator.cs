using PortfolioAce.ViewModels;
using System.Windows.Input;

namespace PortfolioAce.Navigation
{
    public enum ViewType
    {
        Home,
        FundsView,
        SysSecurityPrices

    }
    public interface INavigator
    {
        ViewModelBase CurrentViewModel { get; set; }
        ICommand UpdateCurrentViewModelCommand { get; }

        void CloseApplication();
        void ShowSettingsWindow();
        void ShowNewFundWindow();
        void ShowAboutWindow();
        void ShowImportTradesWindow();
        void ShowNewInvestorWindow();
        void ShowSecurityManagerWindow();
    }
}
