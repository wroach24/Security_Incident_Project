using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Security_Response_Program.Models;
using Security_Response_Program.Models;
using Security_Response_Program.Services;
using Security_Response_Program.Views.Pages;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls;
using MessageBox = System.Windows.MessageBox;

namespace Security_Response_Program.ViewModels
{
    public partial class IncidentResponseViewModel : ObservableObject, INavigationAware
    {
        [ObservableProperty] private string _buttonTestText = "Test Button";

        [ObservableProperty] private Incident? _currentIncident;

        [ObservableProperty] private string _selectedIncidentStatus = "Detected";
        [ObservableProperty] private List<string> _incidentStatuses = new List<string>()
        {
            "Detected",
            "Contained",
            "Eradicated",
            "Prevented"
        };



        private readonly INavigationService _navigationService;
        private readonly IUserService _userService;
        private readonly ISnackbarMessageService _snackbarMessageService;
        private readonly PersistedUserSelectionService _persistedUserSelectionService;
        public IncidentResponseViewModel (INavigationService navigationService, IUserService userService, ISnackbarMessageService snackbarMessageService, PersistedUserSelectionService persistedUserSelectionService)
        {
            _navigationService = navigationService;
            _userService = userService;
            _snackbarMessageService = snackbarMessageService;
            _persistedUserSelectionService = persistedUserSelectionService;
        }


        [RelayCommand]
        private async Task SubmitChanges()
        {
            if (CurrentIncident == null)
            {
                var newIncident = new Incident()
                {

                };
            }
        }

        private void InitializeNewIncident()
        {
            CurrentIncident = new Incident()
            {
                Status = "Detected",
                // so on so forth
            };
        }
        private void FillOutIncident()
        {
            if (CurrentIncident != null)
            {
                CurrentIncident.Status = SelectedIncidentStatus;
                // so on so forth
            }
        }
        private void InitializeViewModel()
        {
            if (_userService.CurrentUser == null)
            {
                _navigationService.GetNavigationControl().Navigate(typeof(LoginPage));
                
            }

            if (_persistedUserSelectionService.SelectedIncident != null)
            {
                CurrentIncident = _persistedUserSelectionService.SelectedIncident;
                FillOutIncident();
            }
            else
            {
                InitializeNewIncident();
            }
        }
        public void OnNavigatedFrom()
        {
            // determine if there is a global 
        }

        public void OnNavigatedTo()
        { 
            InitializeViewModel();
        }
    }
}
