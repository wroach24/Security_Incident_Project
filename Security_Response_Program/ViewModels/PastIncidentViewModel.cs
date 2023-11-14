using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Security_Response_Program.Models;
using Security_Response_Program.Services;
using Wpf.Ui.Controls;

namespace Security_Response_Program.ViewModels
{
    public partial class PastIncidentViewModel : ObservableValidator, INavigationAware
    {
        #region Fields and Properties

        // List of incidents to be displayed

        [ObservableProperty] private ObservableCollection<Incident> _incidents;

        // Services
        private readonly IDatabaseCommands _databaseService;

        #endregion

        #region Constructor

        public PastIncidentViewModel(IDatabaseCommands databaseService)
        {
            _databaseService = databaseService;
        }

        #endregion

        #region Commands

        [RelayCommand]
        private async Task LoadIncidents()
        {
            // Logic to load incidents from the database
            Incidents = new ObservableCollection<Incident>(await _databaseService.GetIncidents());
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