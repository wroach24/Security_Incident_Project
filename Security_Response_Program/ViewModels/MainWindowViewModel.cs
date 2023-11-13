﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Wpf.Ui.Common;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls;
using Wpf.Ui.Converters;
using MenuItem = Wpf.Ui.Controls.MenuItem;
using Thickness = System.Windows.Thickness;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Threading.Tasks;
using Security_Response_Program.Services;

namespace Security_Response_Program.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private bool _isInitialized = false;

        private readonly ISnackbarMessageService _snackbarMessageService;
        private readonly INavigationService _navigationService;
        private readonly IUserService _userService;
        [ObservableProperty]
        private string _applicationTitle = String.Empty;

        [ObservableProperty]
        private bool _isAdmin = false;

        [ObservableProperty]
        private ICollection<object> _navigationItems;

        [ObservableProperty]
        private ICollection<object> _navigationFooter;



        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new();

        public MainWindowViewModel(INavigationService navigationService, ISnackbarMessageService snackbarMessageService, IUserService userService)
        {
            if (!_isInitialized)
                InitializeViewModel();
            
            _snackbarMessageService = snackbarMessageService;
            _navigationService = navigationService;
            _userService = userService;

        }

        [RelayCommand]
        private async Task NavigateToLogin()
        {
            _navigationService.Navigate(typeof(Views.Pages.LoginPage));
        }

        [RelayCommand]
        private async Task NavigateToIncidentResponseEntry()
        {
            if (_userService.CurrentUser == null)
            {
                await _snackbarMessageService.ShowErrorSnackbar("You must be logged in to access this page.");
                _navigationService.Navigate(typeof(Views.Pages.LoginPage));
                return;
            }
            _navigationService.Navigate(typeof(Views.Pages.IncidentResponsePage));
        }
        private void InitializeViewModel()
        {
            ApplicationTitle = "Security Incident Response System";

            NavigationItems = new ObservableCollection<object>
            {
                new NavigationViewItemSeparator(),
                new NavigationViewItem()
                        {
                        Content = "Log In",
                        Tag = "home",
                        Icon = new SymbolIcon(SymbolRegular.DocumentOnePage20),
                        TargetPageType = typeof(Views.Pages.LoginPage),
                        Command = NavigateToLoginCommand,
                        Visibility = Visibility.Collapsed

                        },
                    new NavigationViewItem()
                    {
                        Content = "Incident Entry",
                        Tag = "doa",
                        Icon =  new SymbolIcon(SymbolRegular.BookDatabase20),
                        Command = NavigateToIncidentResponseEntryCommand,
                        TargetPageType = typeof(Views.Pages.IncidentResponsePage)
                    },
                    new NavigationViewItem()
                    {
                        Content = "Past Incidents",
                        Tag = "search",
                        Icon =  new SymbolIcon(SymbolRegular.Search28),
                        TargetPageType = typeof(Views.Pages.HomePage)
                    },

                    //new NavigationViewItem()
                    //{
                    //    Content = "DOA Charts",
                    //    Tag = "charts",
                    //    Icon =  new SymbolIcon(SymbolRegular.ChartMultiple20),
                    //    TargetPageType = typeof(Views.Pages.ChartDisplayPage)
                    //},
                    //new NavigationViewItemSeparator()
                    //{
                    //    Margin = new Thickness(0),
                    //    Visibility = Visibility.Collapsed
                    //},
                    //new NavigationViewItemHeader()
                    //{
                    //Text = "Admin Functions",
                    //Icon = new SymbolIcon(SymbolRegular.Shield12),
                    //Visibility = Visibility.Collapsed
                    //},

                    //new NavigationViewItem()
                    //{
                    //    Content = "Table Management",
                    //    Tag = "table",
                    //    Icon = new SymbolIcon(SymbolRegular.Table24),
                    //    TargetPageType = typeof(Views.Pages.TableManagementPage),
                    //    Visibility = Visibility.Collapsed
                    //},
                    //new NavigationViewItem
                    //{
                    //    Content = "Modify Server Pathway",
                    //    Tag = "pathway",
                    //    Icon = new SymbolIcon(SymbolRegular.Server20),
                    //    TargetPageType = typeof(Views.Pages.AdjustServerRootPathPage),
                    //    Visibility = Visibility.Collapsed
                    //},
                    //new NavigationViewItem()
                    //{
                    //    Content = "Admin Help",
                    //    Tag = "help",
                    //    Icon = new SymbolIcon(SymbolRegular.ChatHelp20),
                    //    Command = OpenHelpPdfCommand,
                    //    Visibility = Visibility.Collapsed


                    //}

        };

            TrayMenuItems = new ObservableCollection<MenuItem>
            {
                new MenuItem
                {
                    Header = "Home",
                    Tag = "tray_home"
                }
            };

            _isInitialized = true;
        }

       
    }
}
