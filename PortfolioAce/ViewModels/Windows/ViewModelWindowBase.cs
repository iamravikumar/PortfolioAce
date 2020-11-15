using PortfolioAce.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.ViewModels.Modals
{
    public class ViewModelWindowBase:ObservableObject
    {
        public Action CloseAction { get; set; }
    }
}
