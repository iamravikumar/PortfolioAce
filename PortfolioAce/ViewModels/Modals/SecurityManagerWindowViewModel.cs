using PortfolioAce.EFCore.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.ViewModels.Modals
{
    class SecurityManagerWindowViewModel : ViewModelBase
    {
        private IAdminService _adminService;

        public SecurityManagerWindowViewModel(IAdminService adminService)
        {
            _adminService = adminService;
        }
    }
}
