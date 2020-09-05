using InjectionSoftware.Class;
using InjectionSoftware.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        public ObservableCollection<Patient> AllRegisteredPatient
        {
            get
            {
                return InjectionsManager.registeredPatients;
            }
        }

        public ObservableCollection<Injection> AllInjection
        {
            get
            {
                return InjectionsManager.injections;
            }
        }

        public ObservableCollection<Injection> AllDischargedInjection
        {
            get
            {
                return InjectionsManager.dischargedInjections;
            }
        }
    }
}
