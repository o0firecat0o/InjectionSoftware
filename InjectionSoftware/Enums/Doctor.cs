using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace InjectionSoftware.Enums
{
    
    public class Doctor
    {
        public string Name { get; }

        /// <summary>
        /// Name without "-" , which is all converted to linebreak for better visuallization
        /// </summary>
        public string SeperatedName { get; }
        public Brush Color { get; }
        public float UptakeTime { get; }

        private static BrushConverter converter = new System.Windows.Media.BrushConverter();

        public static ObservableCollection<Doctor> Doctors = new ObservableCollection<Doctor>();

        public Doctor(string Name, Brush Color)
        {
            this.Name = Name;
            this.Color = Color;
            Doctors.Add(this);
            this.SeperatedName = Name.Replace('-', '\n');
        }


        //TODO: load doctor list from txt, instead of manual assining

        public static void AddDefault()
        {
            new Doctor("Dr-Kevin-Tse", (Brush)converter.ConvertFromString("#79CBFF"));
            new Doctor("Dr-Cheng", (Brush)converter.ConvertFromString("#7EFF79"));
            new Doctor("Dr-Antonio", (Brush)converter.ConvertFromString("#FFFF79"));
            new Doctor("Dr-Shu", (Brush)converter.ConvertFromString("#C579FF"));
            new Doctor("Dr-Fan", (Brush)converter.ConvertFromString("#83FFB9"));
            new Doctor("Dr-Lo", (Brush)converter.ConvertFromString("#B6B6B6"));
            new Doctor("Dr-Donald-Tse", (Brush)converter.ConvertFromString("#FF79E0"));
            new Doctor("Dr-Siu", (Brush)converter.ConvertFromString("#FFCE79"));
        }
    }
}

