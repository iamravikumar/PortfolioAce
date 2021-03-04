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
            ExportDatagridToCSVCommand = new ExportDatagridToCSVCommand();
            ExportDatagridToTXTCommand = new ExportDatagridToTXTCommand();
            ExportDataGridToPDFCommand = new ExportDataGridToPDFCommand();
            ExportDataGridToJSONCommand = new ExportDataGridToJSONCommand();
        }

        public ICommand ExportDatagridToCSVCommand { get; }
        public ICommand ExportDataGridToPDFCommand { get; }
        public ICommand ExportDatagridToTXTCommand { get; }
        public ICommand ExportDataGridToJSONCommand { get; }
        public virtual void Dispose() { }
    }
}
