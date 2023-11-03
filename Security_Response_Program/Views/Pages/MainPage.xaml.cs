using System;
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static OfficeOpenXml.ExcelErrorValue;
using Excel = Microsoft.Office.Interop.Excel;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

using OfficeOpenXml;
using Security_Response_Program.ViewModels;
using Wpf.Ui.Controls;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace Security_Response_Program.Views.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : INavigableView<HomePageViewModel>
    {
        public MainPage(HomePageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();
        }

        public HomePageViewModel ViewModel
        {
            get;
        }
    }
}

