using PortfolioAce.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.ViewModels
{
    public delegate TViewModel CreateViewModel<TViewModel>() where TViewModel: ViewModelBase;
    public class ViewModelBase: ObservableObject
    {
        public virtual void Dispose() { }
    }
}
