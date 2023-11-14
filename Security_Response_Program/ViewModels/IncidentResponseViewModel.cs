using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
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
        #region Fields and Properties

        // Incident Properties
        [ObservableProperty] private Incident? _currentIncident;
        [ObservableProperty] private string _selectedIncidentType = "Malware";
        [ObservableProperty] private List<string> _incidentTypes = new List<string> { "Malware", "Phishing", "Ransomware", "Data Breach", "Other" };
        [ObservableProperty] private string _selectedIncidentSeverity = "Low";
        [ObservableProperty] private List<string> _incidentSeverities = new List<string> { "P4 - Low", "P3 - Medium", "P2 - High", "P1 - Critical" };
        [ObservableProperty] private string _incidentLocation = string.Empty;
        [ObservableProperty] private DateTime _incidentDate = DateTime.Now;
        [ObservableProperty] private string _incidentDescription = string.Empty;
        [ObservableProperty] private string _incidentAssignee = string.Empty;
        [ObservableProperty] private string _breachDescription = string.Empty;
        [ObservableProperty] private string _dataCompromised = string.Empty;
        [ObservableProperty] private string _selectedIncidentStatus = "Detected";
        [ObservableProperty] private List<string> _incidentStatuses = new List<string> { "Detected", "Contained", "Eradicated", "Prevented" };
        [ObservableProperty] private Models.System _selectedAffectedSystem = new Models.System();
        [ObservableProperty] private List<Models.System> _currentSystems = new List<Models.System>();

        // UI Properties
        [ObservableProperty] private string _buttonTestText = "Test Button";

        // Services
        private readonly INavigationService _navigationService;
        private readonly IUserService _userService;
        private readonly ISnackbarMessageService _snackbarMessageService;
        private readonly IDatabaseCommands _databaseCommands;
        private readonly PersistedUserSelectionService _persistedUserSelectionService;

        #endregion

        #region Constructor

        public IncidentResponseViewModel(INavigationService navigationService, IUserService userService, ISnackbarMessageService snackbarMessageService, PersistedUserSelectionService persistedUserSelectionService, IDatabaseCommands databaseCommands)
        {
            _navigationService = navigationService;
            _userService = userService;
            _snackbarMessageService = snackbarMessageService;
            _persistedUserSelectionService = persistedUserSelectionService;
            _databaseCommands = databaseCommands;
        }

        #endregion
        // Commands
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
                    Date = IncidentDate
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
                    IncidentId = CurrentIncident.IncidentId,
                    Date = IncidentDate
                };
                var finalizedIncident = await _databaseCommands.AddOrUpdateIncident(updatedIncident);
                CurrentIncident = finalizedIncident;
            }

            await _snackbarMessageService.ShowSuccessSnackbar("Incident saved successfully.");
        }

        [RelayCommand]
        private async Task UploadFile()
        {
            // open file dialog to select file
            // upload file to database
            // add file to incident
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var file = openFileDialog.FileName;
                var fileBytes = System.IO.File.ReadAllBytes(file);
                var fileName = System.IO.Path.GetFileName(file);
                var fileExtension = System.IO.Path.GetExtension(file);

                await _snackbarMessageService.ShowSuccessSnackbar("Uploaded File(functionality is not fully supported yet)");
            }
        }
        // Methods
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
                SelectedIncidentStatus = IncidentStatuses.FirstOrDefault(x => x == CurrentIncident.Status) ?? "Detected";
                SelectedIncidentType = IncidentTypes.FirstOrDefault(x => x == CurrentIncident.IncidentType) ?? "Malware";
                SelectedIncidentSeverity = IncidentSeverities.FirstOrDefault(x => x.Contains(CurrentIncident.Severity) ) ?? "Low";
                IncidentLocation = CurrentIncident.IncidentLocation;
                IncidentDescription = CurrentIncident.Description;
                IncidentAssignee = CurrentIncident.IncidentResponder;
                SelectedAffectedSystem = CurrentIncident.AffectedSystem;
                BreachDescription = CurrentIncident.BreachDescription;
                DataCompromised = CurrentIncident.DataCompromised;
                IncidentDate = CurrentIncident.Date.GetValueOrDefault();
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
