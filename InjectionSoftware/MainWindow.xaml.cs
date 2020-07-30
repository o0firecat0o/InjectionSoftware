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

        private TwoChoiceDialog twoChoiceDialog = new TwoChoiceDialog();
                
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

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Start();
            timer.Tick += new EventHandler(delegate (object s, EventArgs a)
            {
                server_client_Selection();
                timer.Stop();
            });
        }

        private async void server_client_Selection()
        {
            twoChoiceDialog.Choice1.Click += createServer;
            twoChoiceDialog.Choice1.Content = "Start as Server";
            twoChoiceDialog.Choice2.Click += createClient;
            twoChoiceDialog.Choice2.Content = "Start as Client";
            await this.ShowMetroDialogAsync(twoChoiceDialog);
        }

        private async void createServer(object sender, RoutedEventArgs e)
        {
            new Server();
            await this.HideMetroDialogAsync(twoChoiceDialog);
        }

        private async void createClient(object sender, RoutedEventArgs e)
        {
            new Client();
            await this.HideMetroDialogAsync(twoChoiceDialog);
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
