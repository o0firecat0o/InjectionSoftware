using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Media;
using System.Xml.Linq;

namespace InjectionSoftware.Network
{
    public class ClientViewObject : INotifyPropertyChanged
    {
        public static ObservableCollection<ClientViewObject> clientViewObjects = new ObservableCollection<ClientViewObject>();

        private string _IP;
        public string IP
        {
            get
            {
                return _IP;
            }
            set
            {
                _IP = value;
                OnPropertyChanged("IP");
            }
        }

        public string fullIP { get; set; }

        private string _Port;
        public string Port
        {
            get
            {
                return _Port;
            }
            set
            {
                _Port = value;
                OnPropertyChanged("Port");
            }
        }


        public string MachineName { get; set; }

        private string _PreviousMessageType = "";
        public string PreviousMessageType
        {
            get
            {
                return _PreviousMessageType;
            }
            set
            {
                _PreviousMessageType = value;
                OnPropertyChanged("PreviousMessageType");
            }
        }

        private string _PreviousMessage = "";
        public string PreviousMessage
        {
            get
            {
                return _PreviousMessage;
            }
            set
            {
                _PreviousMessage = value;
                OnPropertyChanged("PreviousMessage");
            }
        }


        public int Row { get; set; }
        public int Column { get; set; }

        public int ClientNumber { get; set; }

        //if ClientViewObject = self => GoldenYellow
        //else White
        public Brush BackgroundBrush
        {
            get
            {
                if (ClientNumber == NetworkManager.clientNumber)
                {
                    return Brushes.PaleGoldenrod;
                }
                else
                {
                    return Brushes.White;
                }
            }
        }

        private ClientViewObject(int ClientNumber, string MachineName, string fullIP, string IP, string Port)
        {
            this.ClientNumber = ClientNumber;
            this.MachineName = MachineName;
            this.fullIP = fullIP;
            this.IP = IP;
            this.Port = Port;
            Row = ClientNumber / 5;
            Column = ClientNumber % 5;
            MainWindow.window.Dispatcher.Invoke(() =>
            {
                clientViewObjects.Add(this);
            });
        }

        private static void reAssignRowAndColumn()
        {
            foreach(ClientViewObject clientViewObject in clientViewObjects)
            {
                clientViewObject.Row = clientViewObject.ClientNumber / 5;
                clientViewObject.Column = clientViewObject.ClientNumber % 5;
            }
        }

        public static void Add(int ClientNumber, string MachineName, string fullIP)
        {
            string IP = fullIP.Split(':')[0];
            string Port = "00000";
            if (fullIP.Split(':').Length >= 2)
            {
                Port = fullIP.Split(':')[1];
            }

            if (HasClient(ClientNumber))
            {
                GetClient(MachineName).fullIP = fullIP;
                GetClient(MachineName).IP = IP;
                GetClient(MachineName).Port = Port;
            }
            else
            {
                new ClientViewObject(ClientNumber, MachineName, fullIP, IP, Port);

                reAssignRowAndColumn();

                if (NetworkManager.isServer)
                {
                    NetworkManager.server.TCPBroadcastMessage("addAllClientInfo", ClientViewObject.allClientToXML().ToString());
                }
            }
        }

        public static void Delete(string fullIP)
        {

            foreach (ClientViewObject clientViewObject in clientViewObjects)
            {
                if (clientViewObject.fullIP == fullIP)
                {
                    clientViewObjects.Remove(clientViewObject);

                    reAssignRowAndColumn();

                    if (NetworkManager.isServer)
                    {
                        NetworkManager.server.TCPBroadcastMessage("addAllClientInfo", ClientViewObject.allClientToXML().ToString());
                    }

                    return;
                }
            }
            Console.Error.WriteLine("[ClientViewObject] Could not found clientviewobject with IP: {0}", fullIP);
        }

        public void UpdateMessage(string messageType, string message)
        {
            PreviousMessageType = messageType;
            PreviousMessage = message;
        }

        [Obsolete("HasClient(string MachineName) is Obsolete, please use HasClient(int ClientNumber) instead")]
        public static bool HasClient(string MachineName)
        {
            foreach (ClientViewObject clientViewObject in clientViewObjects)
            {
                if (clientViewObject.MachineName == MachineName)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool HasClient(int ClientNumber)
        {
            foreach (ClientViewObject clientViewObject in clientViewObjects)
            {
                if (clientViewObject.ClientNumber == ClientNumber)
                {
                    return true;
                }
            }
            return false;
        }

        public static ClientViewObject GetClient(string MachineName)
        {
            foreach (ClientViewObject clientViewObject in clientViewObjects)
            {
                if (clientViewObject.MachineName == MachineName)
                {
                    return clientViewObject;
                }
            }
            Console.Error.WriteLine("Could not found clientviewobject with MachineName: {0}", MachineName);
            return null;
        }

        public static ClientViewObject GetClientViaIP(string fullIP)
        {
            foreach (ClientViewObject clientViewObject in clientViewObjects)
            {
                if (clientViewObject.fullIP == fullIP)
                {
                    return clientViewObject;
                }
            }
            Console.Error.WriteLine("Could not found clientviewobject with MachineName: {0}", fullIP);
            return null;
        }

        public static void XMLtoClient(XElement xElement)
        {
            clientViewObjects.Clear();
            foreach (XElement singleClient in xElement.Elements())
            {
                int clientNumber = int.Parse(singleClient.Element("clientNumber").Value);
                string machineName = singleClient.Element("machineName").Value;
                string fullIP = singleClient.Element("_fullIP").Value;
                string iP = singleClient.Element("iP").Value;
                string port = singleClient.Element("port").Value;

                Add(clientNumber, machineName, fullIP);
            }
        }

        public static XElement allClientToXML()
        {
            XElement allClient = new XElement("allClient");

            foreach (ClientViewObject clientViewObject in clientViewObjects)
            {
                XElement singleClient = new XElement("singleClient");

                XElement clientNumber = new XElement("clientNumber", clientViewObject.ClientNumber);
                XElement machineName = new XElement("machineName", clientViewObject.MachineName);
                XElement _fullIP = new XElement("_fullIP", clientViewObject.fullIP);
                XElement iP = new XElement("iP", clientViewObject.IP);
                XElement port = new XElement("port", clientViewObject.Port);

                singleClient.Add(clientNumber);
                singleClient.Add(machineName);
                singleClient.Add(_fullIP);
                singleClient.Add(iP);
                singleClient.Add(port);

                allClient.Add(singleClient);
            }

            return allClient;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
