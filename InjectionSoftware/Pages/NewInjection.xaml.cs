using ControlzEx.Theming;
using InjectionSoftware.Class;
using InjectionSoftware.Enums;
using InjectionSoftware.ViewModels;
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
using System.Windows.Shapes;

namespace InjectionSoftware.Pages
{
    /// <summary>
    /// Interaction logic for NewInjection.xaml
    /// </summary>
    public partial class NewInjection
    {
        NewInjectionViewModel viewModel;

        public NewInjection(Injection Injection = null)
        {
            viewModel = new NewInjectionViewModel(this, Injection);
            DataContext = viewModel;
            InitializeComponent();

            //selected item stuff
            viewModel.RPListView = RP_injection;

            //make the focus
            RP_injection.SelectedIndex = 0;
            RadiologistList.SelectedIndex = 0;

            //select the previous selected RPs if the client is trying to modify instead of adding a new injection
            viewModel.reselectRPs();
            viewModel.reselectRadiologist();

            if(Injection == null)
            {
                ThemeManager.Current.ChangeTheme(this, "Light.Green");
                DeleteButton.Visibility = Visibility.Collapsed;
                DischargeButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                ThemeManager.Current.ChangeTheme(this, "Light.Red");
                this.Title = "Modify Injection";
            }
        }
    }
}
