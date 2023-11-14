using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Security_Response_Program.Models;
using Security_Response_Program.Services;
using Security_Response_Program.Services.ApplicationServices;
using Security_Response_Program.ViewModels;
using Security_Response_Program.Views.Pages;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls;
using Wpf.Ui.Services;

namespace Security_Response_Program
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private static readonly IHost _host = CreateHost();

        public static T GetService<T>()
            where T : class
        {
            return _host.Services.GetService(typeof(T)) as T;
        }

        private async void OnStartup(object sender, StartupEventArgs e)
        {
            try
            {
                await _host.StartAsync();
            }
            catch (System.Exception exception)
            {
                HandleStartupException(exception);
            }
        }

        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
        }

        private static IHost CreateHost()
        {
            return Host
                .CreateDefaultBuilder()
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .ConfigureServices(ConfigureServices)
                .Build();
        }

        private static void ConfigureAppConfiguration(HostBuilderContext hostContext, IConfigurationBuilder configBuilder)
        {
            configBuilder.SetBasePath(Path.GetDirectoryName(AppContext.BaseDirectory));
        }

        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            // App Host
            services.AddHostedService<ApplicationHostService>();

            // Database Context Creator
            services.AddDbContextFactory<IncidentDbContext>();

            // Theme manipulation
            services.AddSingleton<IThemeService, ThemeService>();

            services.AddSingleton<ISnackbarService, SnackbarService>();
            services.AddSingleton<ISnackbarMessageService, SnackbarMessageService>();
            services.AddSingleton<IContentDialogService, ContentDialogService>();
            services.AddScoped<IPasswordHashingService, PasswordHashingService>();
            services.AddSingleton<IUserService ,UserService>();
            services.AddSingleton<PersistedUserSelectionService>();
            services.AddSingleton<IDatabaseCommands, DatabaseCommands>();

            // Service containing navigation, same as INavigationService... but without window
            services.AddSingleton<INavigationService, Wpf.Ui.Services.NavigationService>();


            // Views and ViewModels
            services.AddScoped<SignUpPage>();
            services.AddScoped<SignUpPageViewModel>();
            services.AddScoped<LoginPage>();
            services.AddScoped<LoginPageViewModel>();
            services.AddScoped<HomePage>();
            services.AddScoped<HomePageViewModel>();
            services.AddScoped<IncidentResponsePage>();
            services.AddScoped<IncidentResponseViewModel>();
            services.AddScoped<IncidentResponsePage>();
            services.AddScoped<PastIncidentPage>();
            services.AddScoped<PastIncidentViewModel>();

    


            // Main window with navigation
            services.AddScoped<FluentWindow, Views.Windows.MainWindow>();
            services.AddScoped<ViewModels.MainWindowViewModel>();
        }

       
        private void HandleStartupException(Exception exception)
        {
            if (exception is not FileNotFoundException)
            {
                // Handle the exception, show an error message, or log the error
                System.Windows.MessageBox.Show($"An error occurred during application startup: {exception.Message} {exception.Source}");
                throw exception;
            }


        }
    }
}
