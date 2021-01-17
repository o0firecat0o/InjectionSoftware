using InjectionSoftware.Class;
using InjectionSoftware.Dialogs;
using InjectionSoftware.Enums;
using InjectionSoftware.Network;
using InjectionSoftware.Pages;
using InjectionSoftware.Util;
using InjectionSoftware.ViewModels;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private NetworkPage networkPage = new NetworkPage();
        private RolePage rolePage = new RolePage();
        private ConsoleLogPage consoleLogPage = new ConsoleLogPage();

        public static MetroWindow window;
                
        public MainWindow()
        {
            window = this;

            InitializeComponent();

            var userPrefs = new WindowConfig();

            this.Height = userPrefs.WindowHeight;
            this.Width = userPrefs.WindowWidth;
            this.Top = userPrefs.WindowTop;
            this.Left = userPrefs.WindowLeft;
            this.WindowState = userPrefs.WindowState;

            DataContext = new MainWindowViewModel();

            //set the default selection to 1
            leftControlBar.SelectedIndex = 0;

            Modality.AddDefault();
            RP.AddDefault();
            Doctor.AddDefault();
            Room.AddDefault();


            InjectionsManager.Init();

            NetworkManager.Init(this);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            Console.Out.WriteLine(e.Device.ToString());
            base.OnKeyDown(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            var userPrefs = new WindowConfig();

            userPrefs.WindowHeight = this.Height;
            userPrefs.WindowWidth = this.Width;
            userPrefs.WindowTop = this.Top;
            userPrefs.WindowLeft = this.Left;
            userPrefs.WindowState = this.WindowState;

            userPrefs.Save();
            base.OnClosing(e);
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
                    Main.Content = rolePage;
                    break;
                case 3:
                    Main.Content = networkPage;
                    break;
                case 4:
                    Main.Content = consoleLogPage;
                    break;
            }            
        }
    }
}
