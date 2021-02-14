using PortfolioAce.Models;

namespace PortfolioAce.ViewModels
{
    public delegate TViewModel CreateViewModel<TViewModel>() where TViewModel : ViewModelBase;
    public class ViewModelBase : ObservableObject
    {
        public virtual void Dispose() { }
    }
}
