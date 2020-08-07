using InjectionSoftware.Class;
using InjectionSoftware.Dialogs;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Linq;
using WatsonTcp;

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

        /// <summary>
        /// Click event that start the client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static async void StartClient(object sender, RoutedEventArgs e)
        {
            client = new Client();
            client.ServerFound += ServerFound;
            client.ServerConnected += ServerConnected;
            client.ServerDisconnected += ServerDisconnected;
            client.MessageReceivedFromServer += MessageReceivedFromServer;
            try
            {
                await window.HideMetroDialogAsync(twoChoiceDialog);
            }
            catch (Exception)
            {

            }

            await window.ShowMetroDialogAsync(progressingDialog);
        }

        /// <summary>
        /// Click event that start the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static async void StartServer(object sender, RoutedEventArgs e)
        {
            isServer = true;
            server = new Server();

            //TODO: add -= when server shut down?
            server.MessageReceivedFromClientEvent += MessageReceivedFromClient;
            server.ClientConnectedEvent += ClientConnected;
            server.ClientDisconnectedEvent += ClientDisconnected;

            //load all the injection after starting server, the client will load the injection via contacting with server
            InjectionsManager.loadAllInjections();

            await window.HideMetroDialogAsync(twoChoiceDialog);
        }

        /// <summary>
        /// click event that stop the client finding server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static async void GiveUpFindingServer(object sender, RoutedEventArgs e)
        {
            await window.HideMetroDialogAsync(progressingDialog);
            client.StopUDP();
            server_client_Selection();
        }

        /// <summary>
        /// Event enabled if client found a server via UDP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ServerFound(object sender, EventArgs e)
        {
            client.ConnectToServer();
            window.Dispatcher.Invoke(() =>
            {
                progressingDialog.MessageText.Content = "Server Found \n Server Name: " + client.servername + "\nServer IP:" + client.serverip;
            });
        }

        /// <summary>
        /// Event enabled if client connect to server via TCP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ServerConnected(object sender, EventArgs e)
        {
            client.TCPSendMessageToServer("ConnectionSucessful", NetworkUtil.GetMachineName());

            window.Dispatcher.Invoke(async () =>
            {
                // clear all previous injection
                client.TCPSendMessageToServer("requestInitialInjection", "");   
                await window.HideMetroDialogAsync(progressingDialog);
                await window.ShowMessageAsync("Connection to server succesful", "Server Name: " + client.servername + "\nServer IP:" + client.serverip);
            });
        }       

        /// <summary>
        /// Event enabled if lost connection to server, Automatically restart connection if server disconnected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ServerDisconnected(object sender, EventArgs e)
        {
            client.StopUDP();
            client.ServerFound -= ServerFound;
            client.ServerConnected -= ServerConnected;
            client.ServerDisconnected -= ServerDisconnected;
            client.MessageReceivedFromServer -= MessageReceivedFromServer;
            window.Dispatcher.Invoke(() =>
            {
                progressingDialog.TitleText.Content = "Lost Connection to Server";
                progressingDialog.MessageText.Content = "Trying to re-establish connection";
                StartClient(null, null);
            });
        }

        public static void ClientConnected(object sender, ClientConnectedEventArgs args)
        {
        }

        public static void ClientDisconnected(object sender, ClientDisconnectedEventArgs args)
        {
            ClientViewObject.Delete(args.IpPort);
        }


        /// <summary>
        /// Handle message if client send anything to the server
        /// 
        /// The message contain two part,
        /// first part is a identifier (string) to distinguish what type of message is it
        /// second part is the message content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void MessageReceivedFromClient(object sender, MessageReceivedFromClientEventArgs args)
        {
            string[] messages = Encoding.UTF8.GetString(args.Data).Split(new char[] { '_' }, 2);
            try
            {
                Console.Out.WriteLine("[NetworkManager-Server] Recieved message type: " + messages[0]);
                Console.Out.WriteLine("[NetworkManager-Server] The message is: " + messages[1]);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }

            switch (messages[0])
            {
                case "ConnectionSucessful":
                    Console.Out.WriteLine("[NetworkManager-Server] New connection established with client IP: {0}, Name: {1}", args.IpPort, messages[1]);
                    ClientViewObject.Add(messages[1], args.IpPort);
                    break;

                case "modInjection":
                    Console.Out.WriteLine("[NetworkManager-Server] Receiving Mod Injection Request from client, proceed to modify injection");
                    window.Dispatcher.Invoke(() =>
                    {
                        InjectionsManager.RecieveAndModInjection(XElement.Parse(messages[1]));
                    });
                    break;
                case "requestInitialInjection":
                    Console.Out.WriteLine("[NetworkManager-Server] Receiving Initial Injection Request from client, proceed to send all injection to client with ip: {0}", args.IpPort);
                    foreach (Injection injection in InjectionsManager.injections)
                    {
                        server.TCPSendMessage(args.IpPort, "modInjection", injection.toXML().ToString());
                    }
                    break;

                default:
                    Console.Error.WriteLine("[NetworkManager-Server] Unhandled message type: " + messages[0]);
                    break;
            }

            try
            {
                ClientViewObject.GetClientViaIP(args.IpPort).UpdateMessage(messages[0], messages[1]);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
            
        }

        /// <summary>
        /// Handle message if client recieved anything from server
        /// 
        /// The message contain two part,
        /// first part is a identifier (string) to distinguish what type of message is it
        /// second part is the message content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void MessageReceivedFromServer(object sender, MessageReceivedFromServerEventArgs args)
        {
            Console.WriteLine("[NetworkManager-Client] Recieved message from server");
            string[] messages = Encoding.UTF8.GetString(args.Data).Split(new char[] { '_' }, 2);
            try
            {
                Console.Out.WriteLine("[NetworkManager-Client] Recieved message type: " + messages[0]);
                Console.Out.WriteLine("[NetworkManager-Client] The message is: " + messages[1]);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }

            switch (messages[0])
            {
                case "modInjection":
                    Console.Out.WriteLine("[NetworkManager-Client] Receiving Mod Injection Request from server, proceed to modify injection");
                    window.Dispatcher.Invoke(() =>
                    {
                        InjectionsManager.RecieveAndModInjection(XElement.Parse(messages[1]));
                    });
                    break;

                default:
                    Console.Error.WriteLine("[NetworkManager-Client] Unhandled message type: " + messages[0]);
                    break;
            }
        }

        public static void CloseWindow(object sender, RoutedEventArgs e)
        {
            window.Close();
            Environment.Exit(0);
        }
    }
}
