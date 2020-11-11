using PortfolioAce.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.ViewModels.Windows
{
    public class ViewModelWindowBase:ObservableObject
    {
        public Action CloseAction { get; set; }
    }
}
