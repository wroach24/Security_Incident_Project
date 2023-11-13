﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Security_Response_Program.ViewModels;
using Wpf.Ui.Controls;

namespace Security_Response_Program.Views.Pages
{
    /// <summary>
    /// Interaction logic for SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : INavigableView<SignUpPageViewModel>
    {
        public SignUpPage(SignUpPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();
        }

        public SignUpPageViewModel ViewModel { get; }

        private void PasswordBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.Password = PasswordBox.Password;
        }

        private void ConfirmPasswordBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.ConfirmPassword = ConfirmPasswordBox.Password;
        }
    }
}
