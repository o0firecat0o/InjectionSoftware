using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;

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

        private ClientViewObject(string MachineName, string fullIP, string IP, string Port)
        {
            this.MachineName = MachineName;
            this.fullIP = fullIP;
            this.IP = IP;
            this.Port = Port;
            Row = clientViewObjects.Count / 5;
            Column = clientViewObjects.Count % 5;
            clientViewObjects.Add(this);
            Console.Out.WriteLine(fullIP);
        }

        public static void Add(string MachineName, string fullIP)
        {
            string IP = fullIP.Split(':')[0];
            string Port = "00000";
            if (fullIP.Split(':').Length >= 2)
            {
                Port = fullIP.Split(':')[1];
            }

            if (HasClient(MachineName))
            {
                GetClient(MachineName).fullIP = fullIP;
                GetClient(MachineName).IP = IP;
                GetClient(MachineName).Port = Port;
            }
            else
            {
                new ClientViewObject(MachineName, fullIP, IP, Port);
            }
        }

        public static void Delete(string fullIP)
        {
            foreach (ClientViewObject clientViewObject in clientViewObjects)
            {
                if (clientViewObject.fullIP == fullIP)
                {
                    clientViewObjects.Remove(clientViewObject);
                    return;
                }
            }
            Console.Error.WriteLine("Could not found clientviewobject with IP: {0}", fullIP);
        }

        public void UpdateMessage(string messageType, string message)
        {
            PreviousMessageType = messageType;
            PreviousMessage = message;
        }

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
