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

        /// <summary>
        /// Name with max 5 chars
        /// </summary>
        public string SimplifiedName { get; }
        public Brush Color { get; }
        
        private static BrushConverter converter = new System.Windows.Media.BrushConverter();

        public static ObservableCollection<Doctor> Doctors = new ObservableCollection<Doctor>();

        public static Doctor getDoctor(string Name)
        {
            foreach (Doctor doctor in Doctors)
            {
                if(doctor.Name == Name)
                {
                    return doctor;
                }
            }
            return Doctors[0];
        }


        public Doctor(string Name, string SimplifiedName, Brush Color)
        {
            this.Name = Name;
            this.SeperatedName = Name.Replace('-', '\n');
            this.SimplifiedName = SimplifiedName;
            this.Color = Color;
            Doctors.Add(this);            
        }


        //TODO: load doctor list from txt, instead of manual assining

        public static void AddDefault()
        {
            new Doctor("Dr-Kevin-Tse", "KM\nTSE", (Brush)converter.ConvertFromString("#79CBFF"));
            new Doctor("Dr-Cheng", "CHENG", (Brush)converter.ConvertFromString("#7EFF79"));
            new Doctor("Dr-Antonio", "ON", (Brush)converter.ConvertFromString("#FFFF79"));
            new Doctor("Dr-Shu", "SHU", (Brush)converter.ConvertFromString("#C579FF"));
            new Doctor("Dr-Fan", "FAN", (Brush)converter.ConvertFromString("#83FFB9"));
            new Doctor("Dr-Lo", "LO", (Brush)converter.ConvertFromString("#B6B6B6"));
            new Doctor("Dr-Donald-Tse", "D\nTSE", (Brush)converter.ConvertFromString("#FF79E0"));
            new Doctor("Dr-Siu", "SIU", (Brush)converter.ConvertFromString("#FFCE79"));
            new Doctor("Dr-Wong", "WONG", (Brush)converter.ConvertFromString("#00cc99"));
        }
    }
}

