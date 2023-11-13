using Security_Response_Program.ViewModels;
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
using Wpf.Ui.Controls;

namespace Security_Response_Program.Views.Pages
{
    /// <summary>
    /// Interaction logic for PastIncidentPage.xaml
    /// </summary>
    public partial class PastIncidentPage : INavigableView<PastIncidentViewModel>
    {
        public PastIncidentPage( PastIncidentViewModel viewModel)
        {
            ViewModel = viewModel ;
            DataContext = ViewModel;
            InitializeComponent();
        }

        public PastIncidentViewModel ViewModel
        {
            get;
        }
    }
}
