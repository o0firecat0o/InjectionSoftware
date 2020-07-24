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

        /// <summary>
        /// Name with only the pharamceutical without the radionuclide
        /// </summary>
        public string SimplifiedName { get; }
        public Brush Color { get; }
        public float UptakeTime { get; }

        private static BrushConverter converter = new System.Windows.Media.BrushConverter();

        public static ObservableCollection<RP> RPs = new ObservableCollection<RP>();

        public RP(string Name, string SimplifiedName,Brush Color, float UptakeTime = 60)
        {
            this.Name = Name;
            this.SimplifiedName = SimplifiedName;
            this.Color = Color;
            RPs.Add(this);
            this.UptakeTime = UptakeTime;
            this.SeperatedName = Name.Replace('-', '\n');
        }


        //TODO: load RP from txtfile
        public static void AddDefault()
        {
            new RP("F18-FDG", "FDG",(Brush)converter.ConvertFromString("#579BFF"));
            new RP("F18-PSMA", "PSMA",(Brush)converter.ConvertFromString("#FF57C4"));
            new RP("C11-Acetate", "ACT",(Brush)converter.ConvertFromString("#FF5757"));
            new RP("C11-PIB", "PIB",(Brush)converter.ConvertFromString("#71FF57"));
            new RP("F18-FLDOPA", "DOPA",(Brush)converter.ConvertFromString("#57FFF9"));
            new RP("Ga68-Dotatate", "DOT",(Brush)converter.ConvertFromString("#FFFC57"));
            new RP("C11-Methaio-nine", "METH",(Brush)converter.ConvertFromString("#FFBA57"));
            new RP("Ga68-PSMA", "PSMA",(Brush)converter.ConvertFromString("#FF57C4"));
            new RP("Others", "OTHR",(Brush)converter.ConvertFromString("#858585"));
        }
    }
}
