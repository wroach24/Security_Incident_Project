using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        // List all of the incident properties here
        [ObservableProperty] private string _selectedIncidentType = "Malware";
        [ObservableProperty] private List<string> _incidentTypes = new List<string>()
            {
            "Malware",
            "Phishing",
            "Ransomware",
            "Data Breach",
            "Other"
        };
        [ObservableProperty] private string _selectedIncidentSeverity = "Low";
        [ObservableProperty] private List<string> _incidentSeverities = new List<string>()
            {
            "Low",
            "Medium",
            "High",
            "Critical"
        };
        [ObservableProperty] private string _incidentLocation = string.Empty;
        [ObservableProperty] private string _incidentDescription = string.Empty;
        [ObservableProperty] private string _incidentAssignee = string.Empty;
        [ObservableProperty] private string _breachDescription = string.Empty;
        [ObservableProperty] private string _dataCompromised = string.Empty;
        [ObservableProperty] private string _selectedIncidentStatus = "Detected";
        [ObservableProperty] private List<string> _incidentStatuses = new List<string>()
        {
            "Detected",
            "Contained",
            "Eradicated",
            "Prevented"
        };

        [ObservableProperty] private Models.System _selectedAffectedSystem = new Models.System();

        [ObservableProperty] private List<Models.System> _currentSystems = new List<Models.System>();

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
            //if (CurrentIncident == null)
            //{
            //    var newIncident = new Incident()
            //    {
            //        Type = SelectedIncidentType,
            //        Severity = SelectedIncidentSeverity,
            //        Location = IncidentLocation,
            //        Description = IncidentDescription,
            //        Assignee = IncidentAssignee,
            //        Status = SelectedIncidentStatus,
            //        AffectedSystem = SelectedAffectedSystem,
            //        BreachDescription = BreachDescription,
            //        DataCompromised = DataCompromised
            //    };
            //}
        }

        private void InitializeNewIncident()
        {
            CurrentIncident = new Incident()
            {
                Status = "Detected",

            };
        }
        private void FillOutIncident()
        {
            if (CurrentIncident != null)
            {
                SelectedIncidentStatus = CurrentIncident.Status;

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
