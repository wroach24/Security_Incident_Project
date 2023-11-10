using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Controls;
using Application = System.Windows.Application;
using Wpf.Ui.Contracts;
using System.Diagnostics;
using Security_Response_Program.Models;
using Security_Response_Program.Services;

namespace Security_Response_Program.ViewModels
{
    public partial class HomePageViewModel : ObservableObject, INavigationAware
    {
        private readonly INavigationService _navigationWindow;
        private readonly ISnackbarMessageService _snackbarMessageService;

        [ObservableProperty]
        private Visibility _isLoading;
     
      
        public HomePageViewModel(ISnackbarMessageService snackbarService, INavigationService navigationWindow)
        {
   
            _navigationWindow = navigationWindow;
    
            _snackbarMessageService = snackbarService;
        }




        public async void OnNavigatedTo()
        {

            try
            {
                using (var context = new IncidentDbContext())
                {
                    // Create the database if it doesn't exist.
                    if (await context.Database.CanConnectAsync())
                    {
                        await _snackbarMessageService.ShowSuccessSnackbar("Database accessed successfully.");
                    }
                }
                // Connection successful
                // Proceed with any other operations you need to perform on navigation.
            }
            catch (Exception ex)
            {
                // Handle any exceptions here, such as logging the error or informing the user.
                System.Windows.MessageBox.Show($"An error occurred while trying to open the SQLite connection: {ex.Message}");
            }
        }


        public void OnNavigatedFrom()
        {
        }

    


        private Wpf.Ui.Controls.MessageBox CreateMessageBox()
        {
            var msgBox = new Wpf.Ui.Controls.MessageBox
            {
                PrimaryButtonText = "Yes",
                CloseButtonText = "Cancel",
                Title = "Edit Confirmation",
                Content = "Are you sure you want to edit this entry?"


            };

            return msgBox;
        }

        private async Task ConfirmEditSelection()
        {
            Wpf.Ui.Controls.MessageBox msgBox = new Wpf.Ui.Controls.MessageBox();
            msgBox = CreateMessageBox();
            var rsult = msgBox.ShowDialogAsync();

            if (rsult is not null)
            {
                var result = await rsult;
                if (result == Wpf.Ui.Controls.MessageBoxResult.Primary)
                {
                   //do action
                }
            }
            
        }
    }
}
