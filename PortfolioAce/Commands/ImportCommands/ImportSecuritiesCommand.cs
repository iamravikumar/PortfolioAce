using PortfolioAce.Domain.Models.Dimensions;
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
    public class ImportSecuritiesCommand: AsyncCommandBase
    {

        private ImportDataToolViewModel _importVM;
        private IImportService _importService;
        private IStaticReferences _staticReferences;
        // This only works for equities and crypto at the moment.
        public ImportSecuritiesCommand(ImportDataToolViewModel importVM,
             IStaticReferences staticReferences, IImportService importService)
        {
            _importVM = importVM;
            _importService = importService;
            _staticReferences = staticReferences;
        }

        public override bool CanExecute(object parameter)
        {
            // I would apply my logic here AND the base logic.
            // so something like return xxx && base.CanExecute(parameter);
            return base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                if (_importVM.dgCSVSecurities.Count != 0)
                {
                    Dictionary<string, int> assetClassMap = _importService.AssetClassMap();
                    Dictionary<string, int> currencyMap = _importService.CurrencyMap();

                    List<SecuritiesDIM> newSecurities = new List<SecuritiesDIM>();
                    foreach (SecurityImportDataCSV data in _importVM.dgCSVSecurities)
                    {
                        SecuritiesDIM newSecurity = new SecuritiesDIM
                        {
                            AssetClassId = assetClassMap[data.AssetClass],
                            CurrencyId = currencyMap[data.Currency],
                            SecurityName = data.Name,
                            FMPSymbol = data.FMPSymbol,
                            ISIN = data.ISIN,
                            Symbol = data.Symbol,
                            AlphaVantageSymbol = data.AVSymbol
                        };
                        newSecurities.Add(newSecurity);
                    }

                    await _importService.AddImportedSecurities(newSecurities);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
