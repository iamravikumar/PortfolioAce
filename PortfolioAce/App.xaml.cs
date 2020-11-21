using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PortfolioAce.DataCentre.APIConnections;
using PortfolioAce.Domain.BusinessServices;
using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore;
using PortfolioAce.EFCore.Services;
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
        protected override void OnStartup(StartupEventArgs e)
        {
            IServiceProvider serviceProvider = CreateServiceProvider();
            System.Windows.FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false; // allows me to put negatives in textbox

            Window window =  serviceProvider.GetRequiredService<MainWindow>();
            window.Show();
            base.OnStartup(e);
        }

        private IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            string connectionString = "Data Source=PortfolioAce.db";
            services.AddDbContext<PortfolioAceDbContext>(db => db.UseSqlite(connectionString));
            services.AddSingleton<PortfolioAceDbContextFactory>(new PortfolioAceDbContextFactory());


            services.AddSingleton<IPortfolioAceViewModelAbstractFactory, PortfolioAceViewModelAbstractFactory>();
            services.AddSingleton<PortfolioAceDbContextFactory>();

            // Add API Services here
            string apiKeyAV = "demo";
            string apiKeyFMP = "demo";
            services.AddSingleton<DataConnectionFactory>(new DataConnectionFactory(apiKeyAV, apiKeyFMP));

            // Add Business Services here
            services.AddSingleton<IPortfolioService, PortfolioService>();


            // Add UoW Serices here
            services.AddSingleton<IFundService, FundService>();
            services.AddSingleton<ICashTradeService, CashTradeService>();
            services.AddSingleton<ITradeService, TradeService>();
            services.AddSingleton<IAdminService, AdminService>();
            services.AddSingleton<ITransferAgencyService, TransferAgencyService>();
            services.AddSingleton<IPriceService, PriceService>();

            // Add viewmodels here
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<AllFundsViewModel>(services=> new AllFundsViewModel(
                services.GetRequiredService<IFundService>(), 
                services.GetRequiredService<ITradeService>(),
                 services.GetRequiredService<ICashTradeService>(),
                 services.GetRequiredService<IPortfolioService>(),
                 services.GetRequiredService<ITransferAgencyService>())); // this is how i can pass in my repositories
            services.AddSingleton<SystemFXRatesViewModel>();
            services.AddSingleton<SystemSecurityPricesViewModel>(services=> new SystemSecurityPricesViewModel(
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

            return services.BuildServiceProvider();
        }
    }
}
