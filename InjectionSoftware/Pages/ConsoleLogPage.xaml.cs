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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InjectionSoftware.Pages
{
    /// <summary>
    /// Interaction logic for ConsoleLogPage.xaml
    /// </summary>
    public partial class ConsoleLogPage : Page
    {
        public ConsoleLogPage()
        {
            InitializeComponent();
            ConsoleLogPageViewModel vm = new ConsoleLogPageViewModel();
            vm.Init();
            DataContext = vm;
        }
    }
}
