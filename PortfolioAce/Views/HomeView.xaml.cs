using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
