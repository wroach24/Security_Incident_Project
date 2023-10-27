using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using NIST_Calibration_Sheet.Views.Pages;
using Microsoft.EntityFrameworkCore.Metadata;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls;
using NIST_Calibration_Sheet.ViewModels;

namespace NIST_Calibration_Sheet.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        public MainWindow(
            MainWindowViewModel viewModel,
            INavigationService navigationService,
            IServiceProvider serviceProvider,
            ISnackbarService snackbarService,
            IContentDialogService contentDialogService
        )
        {

            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            navigationService.SetNavigationControl(NavigationView);
            contentDialogService.SetContentPresenter(RootContentDialog);
            snackbarService.SetSnackbarPresenter(SnackbarPresenter);
            NavigationView.SetServiceProvider(serviceProvider);
            NavigationView.Loaded += (_, _) => NavigationView.Navigate(typeof(MainPage));
        }

        public MainWindowViewModel ViewModel { get; }

        private void OnNavigationSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (sender is not Wpf.Ui.Controls.NavigationView navigationView)
                return;

            NavigationView.HeaderVisibility =
                navigationView.SelectedItem?.TargetPageType != typeof(MainPage)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
        }
    }
}