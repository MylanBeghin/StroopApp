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

            // Resolve ExperimentChartFactory from DI
            var chartFactory = _serviceProvider.GetRequiredService<ExperimentChartFactory>();

            // Create settings with injected factory
            var settings = new ExperimentSettings();
            settings.ExperimentContext = new SharedExperimentData(chartFactory);

            var expWin = new ExperimentWindow(settings, WindowManager, LanguageService);
            expWin.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Phase D1: ExperimentChartFactory for LiveCharts graphics creation
            services.AddSingleton<ExperimentChartFactory>();

            // Phase D2.1: Core application services
            // LanguageService: Singleton - manages global application culture and language state.
            // Single instance ensures consistent language across all components.
            services.AddSingleton<LanguageService>();

            // WindowManager: Singleton - manages participant window lifecycle.
            // Single instance required to track and control the unique participant window.
            services.AddSingleton<IWindowManager, WindowManager>();
        }
    }
}
