using PortfolioAce.ViewModels.Modals;
using System.Reflection;

namespace PortfolioAce.ViewModels.Windows
{
    public class AboutWindowViewModel : ViewModelWindowBase
    {
        public AboutWindowViewModel()
        {
            _assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }


        private string _assemblyVersion;
        public string AssemblyVersion
        {
            get
            {
                return _assemblyVersion;
            }
        }
    }
}
