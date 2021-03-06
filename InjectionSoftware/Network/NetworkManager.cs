﻿using InjectionSoftware.Class;
using InjectionSoftware.Dialogs;
using InjectionSoftware.Pages;
using InjectionSoftware.Util;
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
        public static bool isServer
        {
            get
            {
                return NetworkManager.clientNumber == 0;
            }
        }

        public static Client client;

        public static Server server;

        private static TwoChoiceDialog twoChoiceDialog = new TwoChoiceDialog();
        private static ProgressingDialog progressingDialog = new ProgressingDialog();
        private static ProgressingDialog simultaneousErrorDialog = new ProgressingDialog();

        private static MetroWindow window;

        //not connected = -1
        //server = 0
        //1st client = 1
        //2nd client = 2 and so on
        public static int clientNumber = -1;

        //for server only, to keep track of how many client is connected (include both connected AND disconnected)
        private static int clientCount = 0;

        private static bool connected = false;

        //enable the connection to server, if false => offline mode
        public static bool connectionEnabled = true;

        public static void Init(MetroWindow w, bool autostart, bool startAsServer)
        {
            window = w;

            //set all the callback
            twoChoiceDialog.Choice1.Click += StartServer;
            twoChoiceDialog.Choice1.Content = "Start as Server";
            twoChoiceDialog.Choice2.Click += StartClient;
            twoChoiceDialog.Choice2.Content = "Start as Client";
            twoChoiceDialog.CloseWindow.Click += CloseWindow;

            progressingDialog.GiveUp.Click += GiveUpFindingServer;

            simultaneousErrorDialog.GiveUp.Click += RetryFindingServer;
            simultaneousErrorDialog.GiveUp.Content = "Retry";

            //if (autostart)
            //{
            //    if (startAsServer)
            //    {
            //        DispatcherTimer timer = new DispatcherTimer();
            //        timer.Interval = TimeSpan.FromMilliseconds(10);
            //        timer.Start();
            //        timer.Tick += new EventHandler(delegate (object s, EventArgs a)
            //        {
            //            StartServer(null, null);
            //            timer.Stop();
            //        });
            //    }
            //    else
            //    {
            //        DispatcherTimer timer = new DispatcherTimer();
            //        timer.Interval = TimeSpan.FromMilliseconds(10);
            //        timer.Start();
            //        timer.Tick += new EventHandler(delegate (object s, EventArgs a)
            //        {
            //            StartClient(null, null);
            //            timer.Stop();
            //        });
            //    }
            //}
            //else
            {


                //this timer is used to deal with a bug with metro dialog, where nullreferenceexception when initizaled in constructor
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(10);
                timer.Start();
                timer.Tick += new EventHandler(delegate (object s, EventArgs a)
                {
                    StartClientThenServer();
                    timer.Stop();
                });
            }


        }





        public static async void StartClientThenServer()
        {
            //detect if a client is already started on this computer
            if (System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners().Any(p => p.Port == 14999))
            {
                if (!simultaneousErrorDialog.IsVisible)
                {
                    await window.ShowMetroDialogAsync(simultaneousErrorDialog);
                }
                simultaneousErrorDialog.MessageText.Content = "Error when starting client- UDP port already occupied. \nCheck whether there is another instance running";
                Console.Out.WriteLine("[NetworkManager] error when starting client- UDP port already occupied. Check whether there is another instance running");
                return;
            }

            if (!connectionEnabled)
            {
                return;
            }

            client = new Client();
            client.ServerFound += ServerFound;
            client.ServerConnected += ServerConnected;
            client.ServerDisconnected += ServerDisconnected;
            client.MessageReceivedFromServer += MessageReceivedFromServer;

            //Auto give up finding client after 10 second
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5 + 4 * (clientNumber + 2));
            timer.Start();
            timer.Tick += new EventHandler(async delegate (object s, EventArgs a)
            {
                timer.Stop();
                if (connected)
                {
                    return;
                }
                Console.Out.WriteLine("[NetworkManager] No server found. Terminating client. Start as Server instead");
                //close the client
                if (progressingDialog.IsVisible)
                {
                    await window.HideMetroDialogAsync(progressingDialog);
                }
                client.StopUDP();
                //start as a server
                StartServer(null, null);
            });
            if (!progressingDialog.IsVisible)
            {
                await window.ShowMetroDialogAsync(progressingDialog);
            }

            progressingDialog.MessageText.Content = "Finding Server:\nPosition in queue: " + (clientNumber + 2);
        }

        private static async void StartClient(object sender, RoutedEventArgs e)
        {
            //detect if a client is already started on this computer
            if (System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners().Any(p => p.Port == 14999))
            {
                if (!simultaneousErrorDialog.IsVisible)
                {
                    await window.ShowMetroDialogAsync(simultaneousErrorDialog);
                }
                simultaneousErrorDialog.MessageText.Content = "Error when starting client- UDP port already occupied. \nCheck whether there is another instance running";
                Console.Out.WriteLine("[NetworkManager] error when starting client- UDP port already occupied. Check whether there is another instance running");
                return;
            }
            client = new Client();
            client.ServerFound += ServerFound;
            client.ServerConnected += ServerConnected;
            client.ServerDisconnected += ServerDisconnected;
            client.MessageReceivedFromServer += MessageReceivedFromServer;

            //hide all dialog
            try
            {
                if (twoChoiceDialog.IsVisible)
                {
                    await window.HideMetroDialogAsync(twoChoiceDialog);
                }
            }
            catch (Exception)
            {

            }

            if (!progressingDialog.IsVisible)
            {
                await window.ShowMetroDialogAsync(progressingDialog);
            }

        }

        private static async void StartServer(object sender, RoutedEventArgs e)
        {
            if (System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners().Any(p => p.Port == 15000))
            {
                if (!simultaneousErrorDialog.IsVisible)
                {
                    await window.ShowMetroDialogAsync(simultaneousErrorDialog);
                }
                simultaneousErrorDialog.MessageText.Content = "Error when starting server- UDP port already occupied. Check whether there is another instance running";
                Console.Out.WriteLine("[NetworkManager] error when starting server- UDP port already occupied. Check whether there is another instance running");
                return;
            }

            server = new Server();

            connected = true;
            clientNumber = 0;
            clientCount = 0;

            //client view object for displaying in the network page
            ClientViewObject.Add(clientNumber, NetworkUtil.GetMachineName(), NetworkUtil.GetLocalIPAddress());

            //TODO: add -= when server shut down?
            server.MessageReceivedFromClientEvent += MessageReceivedFromClient;
            server.ClientConnectedEvent += ClientConnected;
            server.ClientDisconnectedEvent += ClientDisconnected;


            //load all patient in registered patient after starting server, the client will load the patient via requesting
            PatientManager.LoadAllPatientFromSchedular();
            PatientManager.LoadAllPatient();

            //load all the injection after starting server, the client will load the injection via contacting with server
            InjectionsManager.loadAllInjections();

            if (twoChoiceDialog.IsVisible)
            {
                await window.HideMetroDialogAsync(twoChoiceDialog);
            }
            await window.ShowMessageAsync("No server found. \nStarted as server instead!", "Server Name: " + NetworkUtil.GetMachineName() + "\nServer IP:" + NetworkUtil.GetLocalIPAddress());
        }

        private async static void server_client_Selection()
        {
            await window.ShowMetroDialogAsync(twoChoiceDialog);
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

        public static async void RetryFindingServer(object sender, RoutedEventArgs e)
        {
            await window.HideMetroDialogAsync(simultaneousErrorDialog);
            StartClientThenServer();
        }

        /// <summary>
        /// Event enabled if client found a server via UDP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ServerFound(object sender, EventArgs e)
        {
            connected = true;

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
                // request all the registered patient, exp from scheduler
                client.TCPSendMessageToServer("requestInitialPatient", "");

                // Injection Syncing...
                // clear all previous injection
                InjectionsManager.injections.Clear();
                // request all the injection
                client.TCPSendMessageToServer("requestInitialInjection", "");
                if (progressingDialog.IsVisible)
                {
                    await window.HideMetroDialogAsync(progressingDialog);
                }
                if (simultaneousErrorDialog.IsVisible)
                {
                    await window.HideMetroDialogAsync(simultaneousErrorDialog);
                }
                if (twoChoiceDialog.IsVisible)
                {
                    await window.HideMetroDialogAsync(twoChoiceDialog);
                }
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

            connected = false;

            window.Dispatcher.Invoke(() =>
            {
                ClientViewObject.clientViewObjects.Clear();

                // close the new injection window when lost connection to server
                if (NewInjection.window != null)
                {
                    NewInjection.window.Close();
                }

                System.Threading.Thread.Sleep(2000 + clientNumber * 500);
                
                progressingDialog.TitleText.Content = "Lost Connection to Server";
                progressingDialog.MessageText.Content = "Trying to re-establish connection";
                StartClientThenServer();
            });
        }

        public static void ClientConnected(object sender, ClientConnectedEventArgs args)
        {
        }

        public static void ClientDisconnected(object sender, ClientDisconnectedEventArgs args)
        {
            window.Dispatcher.Invoke(() =>
            {
                ClientViewObject.Delete(args.IpPort);
            });
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
                    window.Dispatcher.Invoke(() =>
                    {
                        //Send the clientNumber to the client
                        clientCount += 1;
                        Console.Out.WriteLine("[NetworkManager-Server] Setting client with IPPort: {0} as client number {1}", args.IpPort, clientCount);
                        server.TCPSendMessage(args.IpPort, "setClientNumber", clientCount.ToString());

                        //Add the client view object for networking page
                        ClientViewObject.Add(clientCount, messages[1], args.IpPort);
                    });
                    break;

                case "modInjection":
                    Console.Out.WriteLine("[NetworkManager-Server] Receiving Mod Injection Request from client, proceed to modify injection");
                    window.Dispatcher.Invoke(() =>
                    {
                        server.TCPBroadcastMessage("modInjection", messages[1]);
                        InjectionsManager.modInjection(XElement.Parse(messages[1]));
                    });
                    break;
                case "requestInitialInjection":
                    Console.Out.WriteLine("[NetworkManager-Server] Receiving Initial Injection Request from client, proceed to send all injection to client with ip: {0}", args.IpPort);
                    foreach (Injection injection in InjectionsManager.injections)
                    {
                        server.TCPSendMessage(args.IpPort, "modInjection", injection.toXML().ToString());
                    }
                    break;

                case "requestInitialPatient":
                    Console.Out.WriteLine("[NetworkManager-Server] Receiving Initial Patient Request from client, proceed to send all patient to client with ip: {0}", args.IpPort);
                    foreach (Patient patient in PatientManager.Patients)
                    {
                        server.TCPSendMessage(args.IpPort, "addPatient", patient.toXML().ToString());
                    }
                    break;

                //////////////////////////////////////////////////
                ////////    Obsolete    //////////////////////////
                case "dischargeInjection":
                    Console.Out.WriteLine("[NetworkManager-Server] Receiving discharge Injection Request from client, proceed to discharge injection");
                    window.Dispatcher.Invoke(() =>
                    {
                        server.TCPBroadcastMessage("dischargeInjection", messages[1]);
#pragma warning disable CS0618 // Type or member is obsolete
                        InjectionsManager.dischargeInjection(messages[1]);
#pragma warning restore CS0618 // Type or member is obsolete
                    });
                    break;
                //////////////////////////////////////////////////

                case "changePatientStatus":
                    Console.Out.WriteLine("[NetworkManager-Server] Receiving changePatientStatus Request from client, proceed to change PatientStatus");
                    window.Dispatcher.Invoke(() =>
                    {
                        server.TCPBroadcastMessage("changePatientStatus", messages[1]);
                        //try to splite the message back to (accessionNumber, patientStatus)
                        if (messages[1].Split('^').Length == 2)
                        {
                            InjectionsManager.changePatientStatus(messages[1].Split('^')[0], messages[1].Split('^')[1]);
                        }
                        else
                        {
                            Console.Out.WriteLine("[NetworkManager-Server_changePatientStatus()] Corrupted changePatientStatus message, require 2 input. Message: " + messages[1]);
                        }
                    });

                    break;

                case "removeInjection":
                    Console.Out.WriteLine("[NetworkManager-Server] Receiving remove Injection Request from client, proceed to remove injection");
                    window.Dispatcher.Invoke(() =>
                    {
                        server.TCPBroadcastMessage("removeInjection", messages[1]);
                        InjectionsManager.removeInjection(messages[1]);
                    });
                    break;

                default:
                    Console.Error.WriteLine("[NetworkManager-Server] Unhandled message type: " + messages[0]);
                    break;
            }

            try
            {
                window.Dispatcher.Invoke(() =>
                {
                    ClientViewObject.GetClientViaIP(args.IpPort).UpdateMessage(messages[0], messages[1]);
                });
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
                case "autoRestart":
                    Console.Out.WriteLine("[NetworkManager-Client] Receiving Auto Restart Request from server, proceed to restart client");

                    //disable connection to prevent reconnection to server before restarting
                    connectionEnabled = false;



                    window.Dispatcher.Invoke(() =>
                    {
                        Console.Out.WriteLine("[NetworkManager-Client] Start timer of 15 seconds");
                        DispatcherTimer timer = new DispatcherTimer();
                        timer.Interval = TimeSpan.FromMinutes(1);
                        timer.Start();
                        timer.Tick += new EventHandler(delegate (object s, EventArgs a)
                        {
                            //only restart if it is a different date
                            if (WindowAutoRestart.isSameDate())
                            {
                                Console.Out.WriteLine("[NetworkManager-Client] It is still yesterday. Wait until auto restart.");
                            }
                            else
                            {
                                Console.Out.WriteLine("[NetworkManager-Client] AutoRestarting...");
                                WindowAutoRestart.AutoRestart();
                                timer.Stop();
                            }
                        });
                    });
                    break;

                case "setClientNumber":
                    Console.Out.WriteLine("[NetworkManager-Client] Receiving Set ClientNumber Request from server, proceed to set self ClientNumber");
                    window.Dispatcher.Invoke(() =>
                    {
                        clientNumber = int.Parse(messages[1]);
                        Console.Out.WriteLine("[NetworkManager-Client] My client number is:" + clientNumber);
                    });
                    break;

                case "addAllClientInfo":
                    Console.Out.WriteLine("[NetworkManager-Client] Receiving Add All ClientInfo Request from server, proceed to add client view objects for network page");
                    window.Dispatcher.Invoke(() =>
                    {
                        ClientViewObject.XMLtoClient(XElement.Parse(messages[1]));
                    });
                    break;

                case "modInjection":
                    Console.Out.WriteLine("[NetworkManager-Client] Receiving Mod Injection Request from server, proceed to modify injection");
                    window.Dispatcher.Invoke(() =>
                    {
                        InjectionsManager.modInjection(XElement.Parse(messages[1]));
                    });
                    break;

                case "changePatientStatus":
                    Console.Out.WriteLine("[NetworkManager-Client] Receiving change PatientStatus Request from server, proceed to change PatientStatus");
                    window.Dispatcher.Invoke(() =>
                    {
                        //try to splite the message back to (accessionNumber, patientStatus)
                        if (messages[1].Split('^').Length == 2)
                        {
                            InjectionsManager.changePatientStatus(messages[1].Split('^')[0], messages[1].Split('^')[1]);
                        }
                        else
                        {
                            Console.Out.WriteLine("[NetworkManager-Client_changePatientStatus()] Corrupted changePatientStatus message, require 2 input. Message: " + messages[1]);
                        }
                    });
                    break;

                //////////////////////////////////////////////////
                ////////    Obsolete    //////////////////////////
                case "dischargeInjection":
                    Console.Out.WriteLine("[NetworkManager-Client] Receiving Discharge Injection Request from server, proceed to discharge injection");
                    window.Dispatcher.Invoke(() =>
                    {
#pragma warning disable CS0618 // Type or member is obsolete
                        InjectionsManager.dischargeInjection(messages[1]);
#pragma warning restore CS0618 // Type or member is obsolete
                    });
                    break;
                //////////////////////////////////////////////////

                case "removeInjection":
                    Console.Out.WriteLine("[NetworkManager-Client] Receiving remove Injection Request from server, proceed to remove injection");
                    window.Dispatcher.Invoke(() =>
                    {
                        InjectionsManager.removeInjection(messages[1]);
                    });
                    break;

                case "addPatient":
                    Console.Out.WriteLine("[NetworkManager-Client] Receiving Add Patient Request from server, proceed to add Patient");
                    window.Dispatcher.Invoke(() =>
                    {
                        PatientManager.ModPatient(new Patient(XElement.Parse(messages[1])));
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
