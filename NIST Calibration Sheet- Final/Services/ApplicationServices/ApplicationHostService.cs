﻿
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using NIST_Calibration_Sheet.Views.Windows;
using Microsoft.Extensions.Hosting;
using Wpf.Ui.Controls;


namespace NIST_Calibration_Sheet.Services.ApplicationServices;

/// <summary>
/// Managed host of the application.
/// </summary>
public class ApplicationHostService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public ApplicationHostService(IServiceProvider serviceProvider)
    {
        // If you want, you can do something with these services at the beginning of loading the application.
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Triggered when the application host is ready to start the service.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await HandleActivationAsync();
    }

    /// <summary>
    /// Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }

    /// <summary>
    /// Creates main window during activation.
    /// </summary>
    private async Task HandleActivationAsync()
    {
        await Task.CompletedTask;

        if (!Application.Current.Windows.OfType<MainWindow>().Any())
        {
            var mainWindow = _serviceProvider.GetService(typeof(FluentWindow)) as FluentWindow;
            mainWindow?.Show();
        }

        await Task.CompletedTask;
    }
}