using InjectionSoftware.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionSoftware.ViewModels
{
    internal class RoomPageViewModel
    {
        public ObservableCollection<Room> AllRoom
        {
            get
            {
                return Room.Rooms;
            }
        }
    }
}
