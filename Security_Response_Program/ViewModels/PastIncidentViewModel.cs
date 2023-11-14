using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Security_Response_Program.Models;
using Security_Response_Program.Services;
using Security_Response_Program.Views.Pages;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls;

namespace Security_Response_Program.ViewModels
{
    public partial class PastIncidentViewModel : ObservableValidator, INavigationAware
    {
        #region Fields and Properties

        // List of incidents to be displayed

        [ObservableProperty] private ObservableCollection<Incident> _p1Incidents;
        [ObservableProperty] private ObservableCollection<Incident> _p2Incidents;
        [ObservableProperty] private ObservableCollection<Incident> _p3Incidents;
        [ObservableProperty] private ObservableCollection<Incident> _p4Incidents;

        // Services
        private readonly IDatabaseCommands _databaseService;
        private readonly ISnackbarMessageService _snackbarMessageService;
        private readonly PersistedUserSelectionService _persistedUserSelectionService;
        private readonly INavigationService _navigationService;

        #endregion

        #region Constructor

        public PastIncidentViewModel(IDatabaseCommands databaseService, ISnackbarMessageService snackbarMessageService, PersistedUserSelectionService persistedUserSelectionService, INavigationService navigationService)
        {
            _databaseService = databaseService;
            _snackbarMessageService = snackbarMessageService;
            _persistedUserSelectionService = persistedUserSelectionService;
            _navigationService = navigationService;
        }

        #endregion

        #region Commands

        [RelayCommand]
        private async Task LoadIncidents()
        {
            // Logic to load incidents from the database
            var incidents = await _databaseService.GetIncidents();
            P1Incidents = new ObservableCollection<Incident>(incidents.Where(x => x.Severity.Contains("P1")).ToList());
            P2Incidents = new ObservableCollection<Incident>(incidents.Where(x => x.Severity.Contains("P2")).ToList());
            P3Incidents = new ObservableCollection<Incident>(incidents.Where(x => x.Severity.Contains("P3")).ToList());
            P4Incidents = new ObservableCollection<Incident>(incidents.Where(x => x.Severity.Contains("P4")).ToList());
        }

        [RelayCommand]
        private async Task DeleteIncident(Incident incident)
        {
            // Logic to delete an incident from the database
            await _databaseService.RemoveIncident(incident);
            await LoadIncidents();
            await _snackbarMessageService.ShowSuccessSnackbar("Incident deleted successfully.");
        }

        [RelayCommand]
        private async Task EditIncident(Incident incident)
        {
            // Logic to edit an incident
            _persistedUserSelectionService.SelectedIncident = incident;
            await LoadIncidents();
            _navigationService.Navigate(typeof(IncidentResponsePage));
            await _snackbarMessageService.ShowSuccessSnackbar("Incident opened for editing successfully.");

        }

        #endregion

        #region Navigation Methods

        public async void OnNavigatedFrom()
        {
            // Logic for when navigating from this view
        }

        public async void OnNavigatedTo()
        {
            // Reload incidents when navigated to this page
            await LoadIncidents();
        }

        #endregion
    }
}