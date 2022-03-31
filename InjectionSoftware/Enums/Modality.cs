using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace InjectionSoftware.Enums
{
    public class Modality
    {
        public string Name { get; }

        public Brush Color { get; }

        private static BrushConverter converter = new System.Windows.Media.BrushConverter();

        public static ObservableCollection<Modality> Modalities = new ObservableCollection<Modality>();

        public static Modality getModality(string Name)
        {
            foreach (Modality modality in Modalities)
            {
                if (modality.Name == Name)
                {
                    return modality;
                }
            }
            return Modalities[0];
        }

        public Modality(string Name, Brush Color)
        {
            this.Name = Name;
            this.Color = Color;
            Modalities.Add(this);
        }

        public static void AddDefault()
        {
            new Modality("PETCT", (Brush)converter.ConvertFromString("#83FFB9"));
            new Modality("PETMR", (Brush)converter.ConvertFromString("#FF79E0"));
            //TODO: handle NM and other
            new Modality("NM", (Brush)converter.ConvertFromString("#79CBFF"));
            //new Modality("OTHER", (Brush)converter.ConvertFromString("#FFCE79"));
        }
    }
}
