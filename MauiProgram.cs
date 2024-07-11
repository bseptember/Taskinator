using Microsoft.Extensions.Logging;
using Realms;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Taskinator.Services;
using Taskinator.ViewModels;
using Taskinator.Views;
using Mopups.Hosting;
using Taskinator.Helper;
using Taskinator.Models;
using System.Text.Json;

namespace Taskinator
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            ConfigureBuilder(builder);
            ConfigureLogging(builder);
            ConfigureSecrets();
            RegisterServices(builder.Services);
            RegisterViewModels(builder.Services);
            RegisterViews(builder.Services);

            return builder.Build();
        }

        private static void ConfigureBuilder(MauiAppBuilder builder)
        {
            builder
                .UseMauiApp<App>()
                .ConfigureMopups()
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("font-awesome-5-free-solid.otf", "FontAwesomeSolid");
                    fonts.AddFont("font-awesome-5-free-regular.otf", "FontAwesomeRegular");
                });
        }

        private static void ConfigureLogging(MauiAppBuilder builder)
        {
#if DEBUG
            builder.Logging.AddDebug();
#endif
        }

        private static async void ConfigureSecrets()
        {
            // Can only use SetAsync once on windows, otherwise it freezes. So a serializer is used.
            AuthCredentials credentials = new AuthCredentials
            {
               domain = "add_your_domain_here",
               clientId = "add_your_client_id_here"
            };

            // Serialize credentials to JSON
            string jsonCredentials = JsonSerializer.Serialize(credentials);

            // Store the JSON string in SecureStorage
            await SecureStorage.SetAsync("auth_credentials", jsonCredentials);
        }

        private static void RegisterServices(IServiceCollection services)
        {

            var realmConfig = new RealmConfiguration("taskinator.db")
            {
                SchemaVersion = 1,
                MigrationCallback = (migration, oldSchemaVersion) =>
                {
                    new RealmMigrationHelper().OnMigration(migration, oldSchemaVersion);
                },
                IsDynamic = false,
                ShouldDeleteIfMigrationNeeded = true
            };

            // Register RealmConfiguration as singleton
            services.AddSingleton(realmConfig);

            // Register Realm as scoped or transient
            services.AddScoped<Realm>(provider => Realm.GetInstance(realmConfig));

            services.AddSingleton<IConnectivity>(Connectivity.Current);
            services.AddScoped<IDatabaseService, DatabaseService>();
            services.AddTransient<IDialogService, DialogService>();
            services.AddSingleton<IEventService, EventService>();
            services.AddTransient<IEventParser, EventParser>();
        }

        private static void RegisterViewModels(IServiceCollection services)
        {
            services.AddTransient<AccountPageViewModel>();
            services.AddTransient<AddCustomEventPageViewModel>();
            services.AddTransient<DayPageViewModel>();
            services.AddTransient<MonthPageViewModel>();
            services.AddTransient<YearPageViewModel>();
            services.AddTransient<SearchPageViewModel>();
            services.AddTransient<SettingsPageViewModel>();
            services.AddTransient<EditEventPageViewModel>();
        }

        private static void RegisterViews(IServiceCollection services)
        {
            services.AddTransient<AccountPageView>();
            services.AddTransient<AddCustomEventPageView>();
            services.AddTransient<DayPageView>();
            services.AddTransient<MonthPageView>();
            services.AddTransient<YearPageView>();
            services.AddTransient<SearchPageView>();
            services.AddTransient<SettingsPageView>();
            services.AddTransient<EditEventPageView>();
        }
    }
}
