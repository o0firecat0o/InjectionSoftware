using InjectionSoftware.Class;
using InjectionSoftware.Dialogs;
using InjectionSoftware.Enums;
using InjectionSoftware.Network;
using InjectionSoftware.ViewModels;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace InjectionSoftware
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private InjectionPage injectionPage = new InjectionPage();
        private RoomPage roomPage = new RoomPage();

       
                
        public MainWindow()
        {
            //new Server();
            //new Client();

            InitializeComponent();

            DataContext = new MainWindowViewModel();

            //set the default selection to 1
            leftControlBar.SelectedIndex = 0;

            RP.AddDefault();
            Doctor.AddDefault();
            Room.AddDefault();

            

            NetworkManager.Init(this);
        }

        

        // switching pages
        private void leftControlBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lv = sender as ListView;
            switch (lv.SelectedIndex)
            {
                case 0:
                    Main.Content = injectionPage;
                    break;
                case 1:
                    Main.Content = roomPage;
                    break;
                case 2:

                    break;
            }            
        }
    }
}
