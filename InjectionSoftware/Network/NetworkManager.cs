using InjectionSoftware.Dialogs;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace InjectionSoftware.Network
{
    public static class NetworkManager
    {
        public static bool isServer = false;

        public static Client client;

        public static Server server;

        private static TwoChoiceDialog twoChoiceDialog = new TwoChoiceDialog();
        private static ProgressingDialog progressingDialog = new ProgressingDialog();

        private static MetroWindow window;

        public static void Init(MetroWindow w)
        {
            window = w;

            //set all the callback
            twoChoiceDialog.Choice1.Click += StartServer;
            twoChoiceDialog.Choice1.Content = "Start as Server";
            twoChoiceDialog.Choice2.Click += StartClient;
            twoChoiceDialog.Choice2.Content = "Start as Client";
            twoChoiceDialog.CloseWindow.Click += CloseWindow;

            progressingDialog.GiveUp.Click += GiveUpFindingServer;

            //this timer is used to deal with a bug with metro dialog, where nullreferenceexception when initizaled in constructor
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Start();
            timer.Tick += new EventHandler(delegate (object s, EventArgs a)
            {
                server_client_Selection();
                timer.Stop();
            });
        }

        private async static void server_client_Selection()
        {
            await window.ShowMetroDialogAsync(twoChoiceDialog);
        }

        public static async void StartClient(object sender, RoutedEventArgs e)
        {
            client = new Client();
            client.ServerFound += ServerFound;
            client.ServerDisconnected += ServerDisconnected;
            try
            {
                await window.HideMetroDialogAsync(twoChoiceDialog);
            }
            catch (Exception)
            {

            }
            
            await window.ShowMetroDialogAsync(progressingDialog);
        }


        public static async void StartServer(object sender, RoutedEventArgs e)
        {
            isServer = true;
            server = new Server();
            await window.HideMetroDialogAsync(twoChoiceDialog);
        }

        public static void ServerFound(object sender, EventArgs e)
        {
            client.ConnectToServer();            
            window.Dispatcher.Invoke(async() =>
            {
                await window.HideMetroDialogAsync(progressingDialog);
                await window.ShowMessageAsync("Connection to server succesful", "Server Name: " + client.servername + "\nServer IP:" + client.serverip);
            });            
        }

        public static void CloseWindow(object sender, RoutedEventArgs e)
        {
            window.Close();
            Environment.Exit(0);
        }

        /// <summary>
        /// Automatically restart connection if server disconnected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ServerDisconnected(object sender, EventArgs e)
        {
            client.StopUDP();
            client.ServerFound -= ServerFound;
            client.ServerDisconnected -= ServerDisconnected;
            window.Dispatcher.Invoke(() =>
            {
                progressingDialog.TitleText.Content = "Lost Connection to Server";
                progressingDialog.MessageText.Content = "Trying to re-establish connection";
                StartClient(null,null);
            });
        }

        public static async void GiveUpFindingServer(object sender, RoutedEventArgs e)
        {
            await window.HideMetroDialogAsync(progressingDialog);
            client.StopUDP();
            server_client_Selection();
        }
    }
}
