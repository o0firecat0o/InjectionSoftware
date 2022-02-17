using InjectionSoftware.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace InjectionSoftware.Enums
{
    public class Room : INotifyPropertyChanged
    {
        public string Name { get; }
        public Brush Color { get; }

        public bool MultiplePatientAllowed { get; set; }

        public float RowSpan { get; set; }
        public float ColumnSpan { get; set; }

        /// <summary>
        /// Used for grid inside grid, the larger the cell, the more child it could handle, for ex, room 11
        /// </summary>
        public int NumberOfChildColumn { get { return (int)(ColumnSpan / 5); } }
        public int NumberOfChildRow { get { return Math.Max((int)(RowSpan / 3), (Injections.Count + NumberOfChildColumn - 1) / NumberOfChildColumn); } }

        public float Row { get; set; }
        public float Column { get; set; }

        private static BrushConverter converter = new System.Windows.Media.BrushConverter();

        public static ObservableCollection<Room> Rooms { get; set; } = new ObservableCollection<Room>();

        /// <summary>
        /// reset the grid width and height each time a new injection is added
        /// </summary>
        public void InjectionsChanged()
        {
            OnPropertyChanged("NumberOfChildRow");
        }

        public ObservableCollection<Injection> Injections
        {
            get; set;
        } = new ObservableCollection<Injection>();

        public static Room getRoom(string Name)
        {
            foreach (Room room in Rooms)
            {
                if (room.Name == Name)
                {
                    return room;
                }
            }
            return Rooms[0];
        }

        public Room(string Name, Brush Color, float RowSpan, float ColumnSpan, float Row, float Column, bool MultiplePatientAllowed = false)
        {
            this.Name = Name;
            this.Color = Color;
            this.RowSpan = RowSpan;
            this.ColumnSpan = ColumnSpan;
            this.Row = Row;
            this.Column = Column;
            this.MultiplePatientAllowed = MultiplePatientAllowed;
            Rooms.Add(this);
        }

        public static void AddDefault()
        {
            new Room("NM", (Brush)converter.ConvertFromString("#FFE4C4"), 3, 5, 0, 0);
            new Room("Treadmill", (Brush)converter.ConvertFromString("#C4F9FF"), 3, 5, 3, 0);
            new Room("Changing\nRooms", (Brush)converter.ConvertFromString("#C4F9FF"), 9, 5, 6, 0, true);
            new Room("Lobby", (Brush)converter.ConvertFromString("#F0C4FF"), 3, 10, 12, 5, true);
            new Room("MCT", (Brush)converter.ConvertFromString("#FFE4C4"), 3, 5, 0, 5);
            new Room("7", (Brush)converter.ConvertFromString("#C4FFCE"), 5, 13, 0, 10, true);
            new Room("BIO", (Brush)converter.ConvertFromString("#FFE4C4"), 3, 10, 0, 23);

            new Room("8", (Brush)converter.ConvertFromString("#C4FFCE"), 3, 10, 3, 23, true);

            new Room("9", (Brush)converter.ConvertFromString("#C4FFCE"), 3, 5, 6, 23);
            new Room("3", (Brush)converter.ConvertFromString("#C4FFCE"), 3, 5, 9, 23);
            new Room("6", (Brush)converter.ConvertFromString("#C4FFCE"), 3, 5, 12, 23);

            new Room("1", (Brush)converter.ConvertFromString("#C4FFCE"), 3, 5, 6, 28);
            new Room("4", (Brush)converter.ConvertFromString("#C4FFCE"), 3, 5, 9, 28);
            new Room("5", (Brush)converter.ConvertFromString("#C4FFCE"), 3, 5, 12, 28);

            new Room("11", (Brush)converter.ConvertFromString("#C4FFCE"), 6, 10, 6, 6, true);
            new Room("2", (Brush)converter.ConvertFromString("#C4FFCE"), 3, 5, 9, 16);
            new Room("10", (Brush)converter.ConvertFromString("#C4FFCE"), 3, 5, 6, 16);
            new Room("PetMR", (Brush)converter.ConvertFromString("#FFE4C4"), 3, 8, 12, 15);
        }

        public bool hasPatient(string PatientID)
        {
            foreach (Injection injection in Injections)
            {
                if(injection.Patient.PatientID == PatientID)
                {
                    return true;
                }
            }
            return false;
        }

        public int getNumberOfPatient()
        {
            return Injections.Count;
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
