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
        [ObservableProperty] private DateTime _incidentDate = DateTime.Now;
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
        private readonly IDatabaseCommands _databaseCommands;
        private readonly PersistedUserSelectionService _persistedUserSelectionService;
        public IncidentResponseViewModel (INavigationService navigationService, IUserService userService, ISnackbarMessageService snackbarMessageService, PersistedUserSelectionService persistedUserSelectionService, IDatabaseCommands databaseCommands)
        {
            _navigationService = navigationService;
            _userService = userService;
            _snackbarMessageService = snackbarMessageService;
            _persistedUserSelectionService = persistedUserSelectionService;
            _databaseCommands = databaseCommands;
        }


        [RelayCommand]
        private async Task SubmitChanges()
        {
            if (CurrentIncident == null)
            {
                var newIncident = new Incident()
                {
                    IncidentType = SelectedIncidentType,
                    Severity = SelectedIncidentSeverity,
                    IncidentLocation = IncidentLocation,
                    Description = IncidentDescription,
                    IncidentResponder = IncidentAssignee,
                    Status = SelectedIncidentStatus,
                    AffectedSystem = SelectedAffectedSystem,
                    BreachDescription = BreachDescription,
                    DataCompromised = DataCompromised,
                };
                var finalizedIncident = await _databaseCommands.AddOrUpdateIncident(newIncident);
                CurrentIncident = finalizedIncident;
            }
            else
            {
                var updatedIncident = new Incident()
                {
                    IncidentType = SelectedIncidentType,
                    Severity = SelectedIncidentSeverity,
                    IncidentLocation = IncidentLocation,
                    Description = IncidentDescription,
                    IncidentResponder = IncidentAssignee,
                    Status = SelectedIncidentStatus,
                    AffectedSystem = SelectedAffectedSystem,
                    BreachDescription = BreachDescription,
                    DataCompromised = DataCompromised,
                    IncidentId = CurrentIncident.IncidentId
                };
                var finalizedIncident = await _databaseCommands.AddOrUpdateIncident(updatedIncident);
                CurrentIncident = finalizedIncident;
            }

            await _snackbarMessageService.ShowSuccessSnackbar("Incident saved successfully.");

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
                SelectedIncidentType = CurrentIncident.IncidentType;
                SelectedIncidentSeverity = CurrentIncident.Severity;
                IncidentLocation = CurrentIncident.IncidentLocation;
                IncidentDescription = CurrentIncident.Description;
                IncidentAssignee = CurrentIncident.IncidentResponder;
                SelectedAffectedSystem = CurrentIncident.AffectedSystem;
                BreachDescription = CurrentIncident.BreachDescription;
                DataCompromised = CurrentIncident.DataCompromised;


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
