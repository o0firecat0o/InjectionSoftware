using InjectionSoftware.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace InjectionSoftware.ViewModels
{
    internal class NetworkPageViewModel
    {
        public CompositeCollection CompositeCollection
        {
            get;
        } = new CompositeCollection();

        public NetworkPageViewModel()
        {
            CollectionContainer clientCollection = new CollectionContainer() { Collection = ClientViewObject.clientViewObjects };
            CompositeCollection.Add(clientCollection);
        }
    }
}
