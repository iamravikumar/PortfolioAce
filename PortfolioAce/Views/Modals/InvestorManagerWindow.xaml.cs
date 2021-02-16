using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace PortfolioAce.Views.Modals
{
    /// <summary>
    /// Interaction logic for AddClientWindow.xaml
    /// </summary>
    public partial class InvestorManagerWindow : Window
    {
        public InvestorManagerWindow()
        {
            InitializeComponent();
        }
        private void Link_LaunchEmail(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }
    }
}
