using Microsoft.Extensions.DependencyInjection;
using StroopApp.Models;
using StroopApp.Services.Charts;
using StroopApp.Services.Exportation;
using StroopApp.Services.KeyMapping;
using StroopApp.Services.Language;
using StroopApp.Services.Navigation.PageFactory;
using StroopApp.Services.Participant;
using StroopApp.Services.Profile;
using StroopApp.Services.Trial;
using StroopApp.Services.Window;
using StroopApp.Views;
using StroopApp.Views.Experiment.Experimenter;
using System.IO;
using System.Windows;

namespace StroopApp
{
    public partial class App : Application
    {
        /// <summary>
        /// Global service provider for the application's DI container.
        /// </summary>
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        public static IWindowManager WindowManager { get; private set; } = null!;
        public static ILanguageService LanguageService { get; private set; } = null!;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            LanguageService = ServiceProvider.GetRequiredService<ILanguageService>();
            WindowManager = ServiceProvider.GetRequiredService<IWindowManager>();

            var pageFactory = ServiceProvider.GetRequiredService<IPageFactory>();

            var expWin = new ExperimentWindow(pageFactory, LanguageService);
            expWin.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var configDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "StroopApp");

            services.AddSingleton(new AppConfiguration { ConfigDirectory = configDir });

            services.AddSingleton<ExperimentChartFactory>();

            services.AddSingleton<ILanguageService, LanguageService>();
            services.AddSingleton<IWindowManager, WindowManager>();

            services.AddSingleton<ExperimentSettings>(sp =>
            {
                var chartFactory = sp.GetRequiredService<ExperimentChartFactory>();
                var settings = new ExperimentSettings();
                settings.ExperimentContext = new SharedExperimentData(chartFactory);
                return settings;
            });

            services.AddSingleton<IPageFactory, PageFactory>();

            services.AddSingleton<IProfileService, ProfileService>();
            services.AddSingleton<IParticipantService, ParticipantService>();
            services.AddSingleton<IKeyMappingService, KeyMappingService>();
            services.AddSingleton<IExportationService, ExportationService>();
            services.AddTransient<ITrialGenerationService, TrialGenerationService>();

            services.AddTransient<ConfigurationPage>();
            services.AddTransient<EndExperimentPage>();
            services.AddTransient<ExperimentDashBoardPage>();
        }
    }
}
