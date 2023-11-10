﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Security_Response_Program.Models;
using Wpf.Ui.Controls;
using MessageBox = System.Windows.MessageBox;

namespace Security_Response_Program.ViewModels
{
    public partial class IncidentResponseViewModel : ObservableObject, INavigationAware
    {
        [ObservableProperty] private string _buttonTestText = "Test Button";

        [ObservableProperty] private Incident _currentIncident;

        public IncidentResponseViewModel()
        {

        }

        [RelayCommand]
        // will generate TestCommand which can be binded to a button
        private async Task Test()
        {
            ButtonTestText = "Test Button Clicked";
        }

        public void OnNavigatedFrom()
        {
            // determine if there is a global 
        }

        public void OnNavigatedTo()
        {
            
        }
    }
}
