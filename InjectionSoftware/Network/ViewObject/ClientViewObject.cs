using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionSoftware.Network
{
    public class ClientViewObject
    {
        public static ObservableCollection<ClientViewObject> clientViewObjects = new ObservableCollection<ClientViewObject>();

        public string IP { get; set; }

        public string MachineName { get; set; }

        public ClientViewObject(string IP, string MachineName)
        {
            this.IP = IP;
            this.MachineName = MachineName;
            clientViewObjects.Add(this);
        }

        public static void Delete(string IP)
        {
            foreach (ClientViewObject clientViewObject in clientViewObjects)
            {
                if(clientViewObject.IP == IP)
                {
                    clientViewObjects.Remove(clientViewObject);
                    return;
                }
            }
            Console.Error.WriteLine("Could not found clientviewobject with IP: {0}", IP);
        }
    }
}
