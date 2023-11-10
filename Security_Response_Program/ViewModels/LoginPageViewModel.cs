using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Security_Response_Program.Models;
using Security_Response_Program.Services;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Security_Response_Program.Views.Pages;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls;

namespace Security_Response_Program.ViewModels
{
    public partial class LoginPageViewModel : ObservableObject, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly IUserService _userService;
        private readonly ISnackbarMessageService _snackbarMessageService;

        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private string _password;

        public LoginPageViewModel(IUserService userService, INavigationService navigationService, ISnackbarMessageService snackbarMessageService)
        {
            _userService = userService;
            _navigationService = navigationService;
            _snackbarMessageService = snackbarMessageService;
        }

        [RelayCommand]
        private async void Login()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                await _snackbarMessageService.ShowErrorSnackbar("Username or password cannot be empty.");
                return;
            }

            try
            {
                var loginSuccess = await _userService.Login(Username, Password);
                if (loginSuccess)
                {
                    await _snackbarMessageService.ShowSuccessSnackbar("Login successful.");
                    _navigationService.Navigate(typeof(IncidentResponsePage)); 
                }
                else
                {
                    await _snackbarMessageService.ShowErrorSnackbar("Invalid username or password.");
                }
            }
            catch (Exception ex)
            {
               await _snackbarMessageService.ShowErrorSnackbar( $"An error occurred during login: {ex.Message}");
            }
        }

        public void OnNavigatedTo()
        {
            // if the user is already logged in, navigate to the main page
            if ( _userService.CurrentUser != null)
            {
                _navigationService.Navigate(typeof(IncidentResponsePage));
            }
        }

        public void OnNavigatedFrom()
        {
            
        }
    }
}
