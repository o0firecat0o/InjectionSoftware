using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionSoftware.Network
{
    public class ClientViewObject : INotifyPropertyChanged
    {
        public static ObservableCollection<ClientViewObject> clientViewObjects = new ObservableCollection<ClientViewObject>();

        private string _IP;
        public string IP { get
            {
                return _IP;
            }
            set
            {
                _IP = value;
                OnPropertyChanged("IP");
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

        private ClientViewObject(string MachineName, string IP)
        {
            this.MachineName = MachineName;
            this.IP = IP;
            clientViewObjects.Add(this);
        }

        public static void Add(string MachineName, string IP)
        {
            if (HasClient(MachineName))
            {
                GetClient(MachineName).IP = IP;
            }
            else
            {
                new ClientViewObject(MachineName, IP);
            }
        }

        public static void Delete(string IP)
        {
            foreach (ClientViewObject clientViewObject in clientViewObjects)
            {
                if (clientViewObject.IP == IP)
                {
                    clientViewObjects.Remove(clientViewObject);
                    return;
                }
            }
            Console.Error.WriteLine("Could not found clientviewobject with IP: {0}", IP);
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
