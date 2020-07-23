using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


namespace InjectionSoftware.Enums
{
    public class RP
    {
        public string Name { get; }

        /// <summary>
        /// Name without "-" , which is all converted to linebreak for better visuallization
        /// </summary>
        public string SeperatedName { get; }
        public Brush Color { get; }
        public float UptakeTime { get; }

        public int S { get; set; }

        private static BrushConverter converter = new System.Windows.Media.BrushConverter();

        public static ObservableCollection<RP> RPs = new ObservableCollection<RP>();

        public RP(string Name, Brush Color, float UptakeTime = 60)
        {
            this.Name = Name;
            this.Color = Color;
            RPs.Add(this);
            this.UptakeTime = UptakeTime;
            this.SeperatedName = Name.Replace('-', '\n');
            S = 0;
        }


        //TODO: load RP from txtfile
        public static void AddDefault()
        {
            new RP("F18-FDG", (Brush)converter.ConvertFromString("#FFA6D6FF"));
            new RP("F18-PSMA", (Brush)converter.ConvertFromString("#FFDBB1FF"));
            new RP("C11-Acetate", (Brush)converter.ConvertFromString("#FFFFE0CA"));
            new RP("C11-PIB", (Brush)converter.ConvertFromString("#FFA6D6FF"));
            new RP("F18-FLDOPA", (Brush)converter.ConvertFromString("#FFFBB5F8"));
            new RP("Ga68-Dotatate", (Brush)converter.ConvertFromString("#FFFFFFFF"));
            new RP("C11-Methaio-nine", (Brush)converter.ConvertFromString("#FFD6CD91"));
            new RP("Ga68-PSMA", (Brush)converter.ConvertFromString("#FFFCFFC8"));
            new RP("Others", (Brush)converter.ConvertFromString("#FFFDA9EE"));
        }
    }
}
