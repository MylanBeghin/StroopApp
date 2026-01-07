using StroopApp.Models;
using StroopApp.Services.Charts;
using StroopApp.Services.Language;
using StroopApp.Services.Window;
using StroopApp.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace StroopApp
{
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        public static IWindowManager WindowManager
        {
            get; private set;
        }
        public static ILanguageService LanguageService { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Configure DI container
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            // Initialize services (legacy pattern - to be migrated progressively)
            LanguageService = new LanguageService();
            WindowManager = new WindowManager();

            // Create settings (not injected yet - Phase D1 scope limited)
            var settings = new ExperimentSettings();

            var expWin = new ExperimentWindow(settings, WindowManager, LanguageService);
            expWin.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Phase D1: Register only ExperimentChartFactory
            services.AddSingleton<ExperimentChartFactory>();
        }
    }
}
