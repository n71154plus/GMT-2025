using System;
using System.Windows;
using GMT_2025.Models;
using GMT_2025.Services;
using GMT_2025.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace GMT_2025
{
    /// <summary>
    /// App.xaml 的互動邏輯
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider? _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<ProductsViewModel>();
            services.AddSingleton<MainWindow>();
            services.AddTransient<Func<Product, ProductWindow>>(sp => product => new ProductWindow(product));
        }
    }
}
