// Copyright © 2020 Ramon Williams
// GNU General Public License
// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; 
// either version 3 of the License, or (at your option) any later version.

// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
//     See the GNU General Public License for more details. 

// You should have received a copy of the GNU General Public License along with this program;
// if not, write to the Free Software Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.

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
using PortfolioAce.EFCore.Services.FactTableServices;
using PortfolioAce.EFCore.Services.PriceServices;
using PortfolioAce.EFCore.Services.SettingServices;
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
                    // Add API Service here
                    services.AddSingleton<DataConnectionFactory>(new DataConnectionFactory());

                    string connectionString = "Data Source=PortfolioAce.db";
                    Action<DbContextOptionsBuilder> configureDbContext = db => db.UseSqlite(connectionString);
                    services.AddDbContext<PortfolioAceDbContext>(configureDbContext);
                    services.AddSingleton<PortfolioAceDbContextFactory>(new PortfolioAceDbContextFactory(configureDbContext));

                    services.AddSingleton<IPortfolioAceViewModelAbstractFactory, PortfolioAceViewModelAbstractFactory>();
                    services.AddSingleton<IWindowFactory, WindowFactory>(services => new WindowFactory(
                        services.GetRequiredService<IFundService>(),
                        services.GetRequiredService<ITransactionService>(),
                         services.GetRequiredService<IPortfolioService>(),
                         services.GetRequiredService<IAdminService>(),
                         services.GetRequiredService<ISettingService>(),
                         services.GetRequiredService<ITransferAgencyService>(),
                         services.GetRequiredService<IStaticReferences>(),
                         services.GetRequiredService<IFactTableService>(),
                         services.GetRequiredService<IPriceService>()));


                    // Add Business Services here
                    services.AddSingleton<IPortfolioService, PortfolioService>();


                    // Add UoW Serices here
                    services.AddSingleton<IFundService, FundService>();
                    services.AddSingleton<ITransactionService, TransactionService>();
                    services.AddSingleton<IAdminService, AdminService>();
                    services.AddSingleton<ITransferAgencyService, TransferAgencyService>();
                    services.AddSingleton<IPriceService, PriceService>();
                    services.AddSingleton<IStaticReferences, StaticReferences>();
                    services.AddSingleton<ISettingService, SettingService>();
                    services.AddSingleton<IFactTableService, FactTableService>();

                    // Add viewmodels here
                    services.AddSingleton<HomeViewModel>(services => new HomeViewModel(
                        services.GetRequiredService<IFactTableService>()));
                    services.AddSingleton<AllFundsViewModel>(services => new AllFundsViewModel(
                        services.GetRequiredService<IFundService>(),
                        services.GetRequiredService<ITransactionService>(),
                         services.GetRequiredService<IPortfolioService>(),
                         services.GetRequiredService<ITransferAgencyService>(),
                         services.GetRequiredService<IStaticReferences>(),
                         services.GetRequiredService<IFactTableService>(),
                         services.GetRequiredService<IPriceService>(),
                         services.GetRequiredService<IWindowFactory>())); // this is how i can pass in my repositories
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
