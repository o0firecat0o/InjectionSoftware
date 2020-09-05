using InjectionSoftware.Class;
using InjectionSoftware.Enums;
using InjectionSoftware.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        public Command<Injection> Command1 { get; set; }

        public RoomPageViewModel()
        {
            Command1 = new Command<Injection>(ExecuteCommand1);
        }

        private void ExecuteCommand1(Injection injection)
        {
            Window newInjectionWindow = new NewInjection(injection);
            newInjectionWindow.ShowDialog();
        }
    }
}
