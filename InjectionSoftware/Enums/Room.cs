using InjectionSoftware.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace InjectionSoftware.Enums
{
    public class Room
    {
        public string Name { get; }
        public Brush Color { get; }

        public float RowSpan { get; }
        public float ColumnSpan { get; }
        public float Row { get; }
        public float Column { get; }

        private static BrushConverter converter = new System.Windows.Media.BrushConverter();

        public static ObservableCollection<Room> Rooms = new ObservableCollection<Room>();

        public ObservableCollection<Injection> Injections
        {
            get
            {
                return InjectionsManager.injections;
            }
        }

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

        public Room(string Name, Brush Color, float RowSpan, float ColumnSpan, float Row, float Column)
        {
            this.Name = Name;
            this.Color = Color;
            this.RowSpan = RowSpan;
            this.ColumnSpan = ColumnSpan;
            this.Row = Row;
            this.Column = Column;
            Rooms.Add(this);
        }

        public static void AddDefault()
        {
            new Room("NM", (Brush)converter.ConvertFromString("#FFE4C4"), 3, 5, 0, 0);
            new Room("Treadmill", (Brush)converter.ConvertFromString("#C4F9FF"), 3, 5, 3, 0);
            new Room("Clothes\nChange", (Brush)converter.ConvertFromString("#C4F9FF"), 9, 5, 6, 0);
            new Room("Lobby", (Brush)converter.ConvertFromString("#F0C4FF"), 3, 10, 12, 5);
            new Room("Mct", (Brush)converter.ConvertFromString("#FFE4C4"), 3, 5, 0, 5);
            new Room("7", (Brush)converter.ConvertFromString("#C4FFCE"), 5, 13, 0, 10);
            new Room("Bio", (Brush)converter.ConvertFromString("#FFE4C4"), 3, 10, 0, 23);

            new Room("8", (Brush)converter.ConvertFromString("#C4FFCE"), 3, 10, 3, 23);

            new Room("9", (Brush)converter.ConvertFromString("#C4FFCE"), 3, 5, 6, 23);
            new Room("3", (Brush)converter.ConvertFromString("#C4FFCE"), 3, 5, 9, 23);
            new Room("6", (Brush)converter.ConvertFromString("#C4FFCE"), 3, 5, 12, 23); 

            new Room("1", (Brush)converter.ConvertFromString("#C4FFCE"), 3, 5, 6, 28);
            new Room("4", (Brush)converter.ConvertFromString("#C4FFCE"), 3, 5, 9, 28);
            new Room("5", (Brush)converter.ConvertFromString("#C4FFCE"), 3, 5, 12, 28);

            new Room("11", (Brush)converter.ConvertFromString("#C4FFCE"), 6, 10, 6, 6);
            new Room("10", (Brush)converter.ConvertFromString("#C4FFCE"), 3, 5, 9, 16);
            new Room("2", (Brush)converter.ConvertFromString("#C4FFCE"), 3, 5, 6, 16);
            new Room("PetMR", (Brush)converter.ConvertFromString("#FFE4C4"), 3, 8, 12, 15);            
        }
    }
}
