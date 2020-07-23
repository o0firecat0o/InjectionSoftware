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

        private static BrushConverter converter = new System.Windows.Media.BrushConverter();

        public static ObservableCollection<RP> RPs = new ObservableCollection<RP>();

        public RP(string Name, Brush Color, float UptakeTime = 60)
        {
            this.Name = Name;
            this.Color = Color;
            RPs.Add(this);
            this.UptakeTime = UptakeTime;
            this.SeperatedName = Name.Replace('-', '\n');
        }


        //TODO: load RP from txtfile
        public static void AddDefault()
        {
            new RP("F18-FDG", (Brush)converter.ConvertFromString("#579BFF"));
            new RP("F18-PSMA", (Brush)converter.ConvertFromString("#FF57C4"));
            new RP("C11-Acetate", (Brush)converter.ConvertFromString("#FF5757"));
            new RP("C11-PIB", (Brush)converter.ConvertFromString("#71FF57"));
            new RP("F18-FLDOPA", (Brush)converter.ConvertFromString("#57FFF9"));
            new RP("Ga68-Dotatate", (Brush)converter.ConvertFromString("#FFFC57"));
            new RP("C11-Methaio-nine", (Brush)converter.ConvertFromString("#FFBA57"));
            new RP("Ga68-PSMA", (Brush)converter.ConvertFromString("#FF57C4"));
            new RP("Others", (Brush)converter.ConvertFromString("#858585"));
        }
    }
}
