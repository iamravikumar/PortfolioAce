using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore.Services;
using PortfolioAce.EFCore.Services.DimensionServices;
using PortfolioAce.EFCore.Services.SettingServices;
using PortfolioAce.HelperObjects.DeserialisedCSVObjects;
using PortfolioAce.ViewModels.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Commands.ImportCommands
{
    public class ImportPriceCommand: AsyncCommandBase
    {
        private ImportDataToolViewModel _importVM;
        private IImportService _importService;
        private IStaticReferences _staticReferences;

        public ImportPriceCommand(ImportDataToolViewModel importVM,
             IStaticReferences staticReferences, IImportService importService)
        {
            _importVM = importVM;
            _importService = importService;
            _staticReferences = staticReferences;
        }

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                if (_importVM.dgCSVPrices.Count != 0)
                {
                    Dictionary<string, int> securityMap = _importService.SecurityMap();
                    Dictionary<string, List<SecurityPriceStore>> securityPrices = new Dictionary<string, List<SecurityPriceStore>>();
                    foreach (PriceImportDataCSV data in _importVM.dgCSVPrices)
                    {

                        if (securityMap.ContainsKey(data.SecuritySymbol))
                        {
                            SecurityPriceStore price = new SecurityPriceStore
                            {
                                ClosePrice = data.ClosePrice,
                                Date = data.Date,
                                PriceSource = data.PriceSource,
                                SecurityId = securityMap[data.SecuritySymbol]
                            };

                            if (!securityPrices.ContainsKey(data.SecuritySymbol))
                            {
                                securityPrices[data.SecuritySymbol] = new List<SecurityPriceStore> { price };
                            }
                            else
                            {
                                securityPrices[data.SecuritySymbol].Add(price);
                            }
                        }

                    }
                    await _importService.AddImportedPrices(securityPrices);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
