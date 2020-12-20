//Copyright © 2020 Ramon Williams

// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//    The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PortfolioAce.DataCentre.APIConnections;
using PortfolioAce.Domain.BusinessServices;
using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore;
using PortfolioAce.EFCore.Services;
using PortfolioAce.EFCore.Services.DimensionServices;
using PortfolioAce.EFCore.Services.PriceServices;
using PortfolioAce.Navigation;
using PortfolioAce.ViewModels;
using PortfolioAce.ViewModels.Factories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;


namespace PortfolioAce
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = CreateHostBuilder().Build();
        }

        public static IHostBuilder CreateHostBuilder(string[] args = null)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(c =>
                {
                    c.AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    // Add API Services here
                    string apiKeyAV = "demo";
                    string apiKeyFMP = "demo";
                    services.AddSingleton<DataConnectionFactory>(new DataConnectionFactory(apiKeyAV, apiKeyFMP));

                    string connectionString = "Data Source=PortfolioAce.db";
                    Action<DbContextOptionsBuilder> configureDbContext = db => db.UseSqlite(connectionString);
                    services.AddDbContext<PortfolioAceDbContext>(configureDbContext);
                    services.AddSingleton<PortfolioAceDbContextFactory>(new PortfolioAceDbContextFactory(configureDbContext));

                    services.AddSingleton<IPortfolioAceViewModelAbstractFactory, PortfolioAceViewModelAbstractFactory>();
                    //services.AddSingleton<PortfolioAceDbContextFactory>();


                    // Add Business Services here
                    services.AddSingleton<IPortfolioService, PortfolioService>();


                    // Add UoW Serices here
                    services.AddSingleton<IFundService, FundService>();
                    services.AddSingleton<ITransactionService, TransactionService>();
                    services.AddSingleton<IAdminService, AdminService>();
                    services.AddSingleton<ITransferAgencyService, TransferAgencyService>();
                    services.AddSingleton<IPriceService, PriceService>();
                    services.AddSingleton<IStaticReferences, StaticReferences>();

                    // Add viewmodels here
                    services.AddSingleton<HomeViewModel>();
                    services.AddSingleton<AllFundsViewModel>(services => new AllFundsViewModel(
                        services.GetRequiredService<IFundService>(),
                        services.GetRequiredService<ITransactionService>(),
                         services.GetRequiredService<IPortfolioService>(),
                         services.GetRequiredService<ITransferAgencyService>(),
                         services.GetRequiredService<IStaticReferences>())); // this is how i can pass in my repositories
                    services.AddSingleton<SystemFXRatesViewModel>();
                    services.AddSingleton<SystemSecurityPricesViewModel>(services => new SystemSecurityPricesViewModel(
                        services.GetRequiredService<IPriceService>()));

                    // We register the viewmodels to be created in our dependency injection container
                    services.AddSingleton<CreateViewModel<HomeViewModel>>(services =>
                    {
                        return () => services.GetRequiredService<HomeViewModel>();
                    });
                    services.AddSingleton<CreateViewModel<AllFundsViewModel>>(services =>
                    {
                        return () => services.GetRequiredService<AllFundsViewModel>();
                    });
                    services.AddSingleton<CreateViewModel<SystemFXRatesViewModel>>(services =>
                    {
                        return () => services.GetRequiredService<SystemFXRatesViewModel>();
                    });
                    services.AddSingleton<CreateViewModel<SystemSecurityPricesViewModel>>(services =>
                    {
                        return () => services.GetRequiredService<SystemSecurityPricesViewModel>();
                    });



                    services.AddScoped<INavigator, Navigator>();
                    services.AddScoped<MainViewModel>();

                    services.AddScoped<MainWindow>(s => new MainWindow(s.GetRequiredService<MainViewModel>()));

                });
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();
            System.Windows.FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false; // allows me to put negatives in textbox

            PortfolioAceDbContextFactory contextFactory = _host.Services.GetRequiredService<PortfolioAceDbContextFactory>();
            using(PortfolioAceDbContext context = contextFactory.CreateDbContext())
            {
                context.Database.Migrate();
            }

            Window window =  _host.Services.GetRequiredService<MainWindow>();
            window.Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();

            base.OnExit(e);
        }
    }
}
