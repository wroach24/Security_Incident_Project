using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Office.Interop.Excel;
using NIST_Calibration_Sheet.Models;
using NIST_Calibration_Sheet.Services;
using NIST_Calibration_Sheet.Services.ApplicationServices;
using NIST_Calibration_Sheet.ViewModels;
using NIST_Calibration_Sheet.Views.Pages;
using NIST_Calibration_Sheet__Final.Properties;
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
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls;
using Wpf.Ui.Services;

namespace NIST_Calibration_Sheet
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

            // Register the factory method for creating DOAContext
            services.AddDbContextFactory<GasCheckTDLSContext>(options =>
                options.UseSqlServer(ConnectionSettings.Default.GasCheckTDLSConnectionString));

            // Theme manipulation
            services.AddSingleton<IThemeService, ThemeService>();

            services.AddSingleton<ISnackbarService, SnackbarService>();
            services.AddSingleton<ISnackbarMessageService, SnackbarMessageService>();
            services.AddSingleton<IContentDialogService, ContentDialogService>();

            // Service containing navigation, same as INavigationService... but without window
            services.AddSingleton<INavigationService, Wpf.Ui.Services.NavigationService>();

            // Main window with navigation
            services.AddScoped<FluentWindow, Views.Windows.MainWindow>();
            services.AddScoped<ViewModels.MainWindowViewModel>();

            // Views and ViewModels
            services.AddScoped<MainPage>();
            services.AddScoped<HomePageViewModel>();
            services.AddScoped<SamplePage>();
            services.AddScoped<SamplePage2>();


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
