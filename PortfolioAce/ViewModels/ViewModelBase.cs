using PortfolioAce.Commands.ExportCommands;
using PortfolioAce.Models;
using System.Windows.Input;

namespace PortfolioAce.ViewModels
{
    public delegate TViewModel CreateViewModel<TViewModel>() where TViewModel : ViewModelBase;
    public class ViewModelBase : ObservableObject
    {
        public ViewModelBase()
        {
            ExportDataGridToCSVCommand = new ExportDataGridToCSVCommand();
            ExportDataGridToTXTCommand = new ExportDataGridToTXTCommand();
            ExportDataGridToPDFCommand = new ExportDataGridToPDFCommand();
            ExportDataGridToJSONCommand = new ExportDataGridToJSONCommand();
        }

        public ICommand ExportDataGridToCSVCommand { get; }
        public ICommand ExportDataGridToPDFCommand { get; }
        public ICommand ExportDataGridToTXTCommand { get; }
        public ICommand ExportDataGridToJSONCommand { get; }
        public virtual void Dispose() { }
    }
}
