using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Wpf.Ui.Common;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls;

namespace Security_Response_Program.Services;

public interface ISnackbarMessageService
{
    Task ShowAsync(string title, string message = "", SymbolIcon? icon = null,
        ControlAppearance appearance = ControlAppearance.Secondary);

    Task ShowSuccessSnackbar(string message = "",
        ControlAppearance appearance = ControlAppearance.Success);
    Task ShowErrorSnackbar(string message = "",
        ControlAppearance appearance = ControlAppearance.Danger);
    void ProcessValidationErrors(IEnumerable<ValidationResult> errors);
}

public class SnackbarMessageService : ISnackbarMessageService
{
    private readonly ISnackbarService _snackbarService;

    public SnackbarMessageService(ISnackbarService snackbarService)
    {
        _snackbarService = snackbarService;
        _snackbarService.DefaultTimeOut = TimeSpan.FromSeconds(5);
    }

    public async Task ShowAsync(string title, string message = "", SymbolIcon? icon = null,
        ControlAppearance appearance = ControlAppearance.Secondary)
    {
        await Dispatcher.CurrentDispatcher.InvokeAsync(
            async () => {  _snackbarService.Show(title:title, message:message, appearance: appearance, icon: icon); },
            DispatcherPriority.Send);
    }

    public async Task ShowErrorSnackbar(string message = "",
        ControlAppearance appearance = ControlAppearance.Danger)
    {
        await Dispatcher.CurrentDispatcher.InvokeAsync(
                       async () => {  _snackbarService.Show(title:"Error:", message:message, appearance: appearance, icon: new SymbolIcon(SymbolRegular.ErrorCircle12)); },
                                  DispatcherPriority.Send);
    }

    public async Task ShowSuccessSnackbar(string message = "",
        ControlAppearance appearance = ControlAppearance.Success)
    {
        await Dispatcher.CurrentDispatcher.InvokeAsync(
                                  async () => {  _snackbarService.Show(title:"Success:", message:message, appearance: appearance, icon: new SymbolIcon(SymbolRegular.CheckmarkCircle12)); },
                                                                   DispatcherPriority.Send);
    }

    public void ProcessValidationErrors(IEnumerable<ValidationResult> errors)
    {
        var sb = new StringBuilder();
        foreach (var error in errors) sb.AppendLine(error.ErrorMessage);
        
        ShowAsync("Error", sb.ToString(), new SymbolIcon(SymbolRegular.ErrorCircle12), ControlAppearance.Danger);
    }
}