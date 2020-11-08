using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PortfolioAce.EFCore;
using PortfolioAce.EFCore.Repository;
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
            
            // Add repositories here
            services.AddSingleton<IFundRepository, FundRepository>();
            services.AddSingleton<ICashTradeRepository, CashTradeRepository>();
            services.AddSingleton<ITradeRepository, TradeRepository>();
            services.AddSingleton<IAdminRepository, AdminRepository>();


            // Add viewmodels here
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<AllFundsViewModel>(services=> new AllFundsViewModel(
                services.GetRequiredService<IFundRepository>())); // this is how i can pass in my repositories
            services.AddSingleton<SystemFXRatesViewModel>();
            services.AddSingleton<SystemSecurityPricesViewModel>();

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
