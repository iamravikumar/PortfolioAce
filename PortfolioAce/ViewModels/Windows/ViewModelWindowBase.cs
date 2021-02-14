using PortfolioAce.Models;
using System;

namespace PortfolioAce.ViewModels.Modals
{
    public class ViewModelWindowBase : ObservableObject
    {
        public Action CloseAction { get; set; }
    }
}
