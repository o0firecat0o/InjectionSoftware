using InjectionSoftware.Class;
using InjectionSoftware.Dialogs;
using InjectionSoftware.Enums;
using InjectionSoftware.FileSync;
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
using System.IO;
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
            ConsoleLogger.Init();

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            window = this;

            InitializeComponent();

            WindowConfig.Init();

            //the timer is used to let the window initialize first, then move the screen
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Start();
            timer.Tick += new EventHandler(delegate (object s, EventArgs a)
            {
                if (WindowConfig.IsAutoRestart == 1)
                {
                    this.Height = WindowConfig.WindowHeight;
                    this.Width = WindowConfig.WindowWidth;
                    this.Top = WindowConfig.WindowTop;
                    this.Left = WindowConfig.WindowLeft;
                    this.WindowState = WindowConfig.WindowState;
                }
                //reset the window config
                WindowConfig.IsAutoRestart = 0;
                WindowConfig.Save();

                timer.Stop();
            });




            DataContext = new MainWindowViewModel();

            //set the default selection to 1
            leftControlBar.SelectedIndex = 0;

            Modality.AddDefault();
            RP.AddDefault();
            Doctor.AddDefault();
            Room.AddDefault();

            //Start the timer, restart the program automatically every day
            WindowAutoRestart.Init();

            InjectionsManager.Init();

            ///////////////////////////////////////
            //Obsolete
            //Previously the networkManager is used to sync between different computers
            //Now we use Temp (T drive) to sync across multiple different computers

            //NetworkManager.Init(this, WindowConfig.IsAutoRestart == 1, WindowConfig.IsServer == 1);
            ///////////////////////////////////////
            

            //This should be run on only server
            //For copying schedular information to T drive for other computers' access
            if (WindowConfig.IsFileSyncServer == 1)
            {
                SchedularCopyManager.Init();
            }

            //This should be run on both server and client
            //For loading patient information from T drive
            SchedularSyncManager.Init();

            //This should be run on both server and client
            FileSyncManager.Init();
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            Console.Out.WriteLine(e.Device.ToString());
            base.OnKeyDown(e);
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            Console.Out.WriteLine("[MainWindow] Exitting Program");

            Console.Out.WriteLine("[MainWindow] Writing file into ConsoleLog.txt");

            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\InjectionSoftware\";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fullpath = path + @"consoleLog.txt";

            try
            {
                using (StreamWriter writetext = new StreamWriter(fullpath))
                {
                    writetext.WriteLine(ConsoleLogger.sw.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot open Redirect.txt for writing");
                Console.WriteLine(ex.Message);
                return;
            }
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

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            ToggleButtonText.Text = "Simplified View";
            detailedViewIcon.Visibility = Visibility.Hidden;
            simplifiedViewIcon.Visibility = Visibility.Visible;

            foreach (Injection injection in InjectionsManager.injections)
            {
                injection.IsDetailed = false;
            }
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ToggleButtonText.Text = "Detailed View";
            detailedViewIcon.Visibility = Visibility.Visible;
            simplifiedViewIcon.Visibility = Visibility.Hidden;

            foreach (Injection injection in InjectionsManager.injections)
            {
                injection.IsDetailed = true;
            }
        }
    }
}
