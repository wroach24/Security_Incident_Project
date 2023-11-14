using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Security_Response_Program.Models;
using Security_Response_Program.Services;
using Security_Response_Program.Views.Pages;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls;

namespace Security_Response_Program.ViewModels
{
    public partial class HomePageViewModel : ObservableObject, INavigationAware
    {
        // Properties and fields
        [ObservableProperty] private ObservableCollection<Incident> _recentIncidents = new ObservableCollection<Incident>();
      
       // Services
       private readonly INavigationService _navigationService;
       private readonly IDatabaseCommands _databaseService;


        public HomePageViewModel(INavigationService navigationService, IDatabaseCommands databaseService)
        {
            _navigationService = navigationService;
            _databaseService = databaseService;
        }

        // Command implementations
        [RelayCommand]
        private void NavigateToActiveIncidents()
        {
            // Update the content of the ContentFrame to the active incidents view
            _navigationService.Navigate(typeof(PastIncidentPage));
            
        }

        private async Task LoadIncidents()
        {
            // Logic to load incidents from the database
            var incidents = await _databaseService.GetIncidents();
            // get incidents in the last 6 months
            var recentIncidents = incidents.Where(x => x.Date > DateTime.Now.AddMonths(-6)).ToList();
            RecentIncidents = new ObservableCollection<Incident>(recentIncidents);
        }
        public async void OnNavigatedTo()
        {
            await LoadIncidents();
        }

        public void OnNavigatedFrom()
        {
           
        }

        // Implement other navigation methods similarly...
    }

}
