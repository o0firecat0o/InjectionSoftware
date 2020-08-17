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
    internal class RoomPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Room> AllRoom
        {
            get
            {
                return Room.Rooms;
            }
        }

        public ObservableCollection<Injection> AllInjection
        {
            get
            {
                return InjectionsManager.injections;
            }
        }

        public RoomPageViewModel()
        {
            Number = 4;
        }

        private int _Number;
        public int Number
        {
            get { return _Number; }
            set { _Number = value; OnPropertyChanged("Number"); }
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
