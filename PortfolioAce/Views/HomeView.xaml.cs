using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace PortfolioAce.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void Onclick_RamonGithub(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/RamonWill") { UseShellExecute = true });
            e.Handled = true;
        }

        private void Onclick_RamonYoutube(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.youtube.com/c/RamonWilliams") { UseShellExecute = true });
            e.Handled = true;
        }

        private void Onclick_RamonLinkedin(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.linkedin.com/in/ramon-w-6b951a5a") { UseShellExecute = true });
            e.Handled = true;
        }
    }
}
