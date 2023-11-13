using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Security_Response_Program.Services;
using Security_Response_Program.Views.Pages;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf.Ui.Contracts;

namespace Security_Response_Program.ViewModels
{
    public partial class SignUpPageViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly INavigationService _navigationService;
        private readonly ISnackbarMessageService _snackbarMessageService;

        [ObservableProperty]
        private string? _username;

        [ObservableProperty]
        private string? _email;

        [ObservableProperty]
        private string? _password;

        [ObservableProperty]
        private string? _confirmPassword;

        public SignUpPageViewModel(IUserService userService, INavigationService navigationService, ISnackbarMessageService snackbarMessageService)
        {
            _userService = userService;
            _navigationService = navigationService;
            _snackbarMessageService = snackbarMessageService;
        }

        [RelayCommand]
        private async Task SignUp()
        {
            if (string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                await _snackbarMessageService.ShowErrorSnackbar("Please fill in all fields.");
                return;
            }

            if (Password != ConfirmPassword)
            {
                await _snackbarMessageService.ShowErrorSnackbar("Passwords do not match.");
                return;
            }

            try
            {
                var signUpSuccess = await _userService.SignUp(Username, Password);
                if (signUpSuccess)
                {
                    await _snackbarMessageService.ShowSuccessSnackbar("Sign-up successful.");
                    _navigationService.Navigate(typeof(LoginPage));
                }
                else
                {
                    await _snackbarMessageService.ShowErrorSnackbar("Sign-up failed.");
                }
            }
            catch (System.Exception ex)
            {
                await _snackbarMessageService.ShowErrorSnackbar($"An error occurred during sign-up: {ex.Message}");
            }
        }

        [RelayCommand]
        private void NavigateLogin()
        {
            _navigationService.Navigate(typeof(LoginPage));
        }
    }
}
