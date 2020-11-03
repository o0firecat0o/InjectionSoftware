using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace InjectionSoftware.Enums
{
    public class PatientStatus
    {
        public string Name;

        public Brush Color { get; }

        private static BrushConverter converter = new System.Windows.Media.BrushConverter();

        public static ObservableCollection<PatientStatus> PatientStatuses = new ObservableCollection<PatientStatus>();

        public static PatientStatus getPatientStatus(string Name)
        {
            foreach (PatientStatus PatientStatus in PatientStatuses)
            {
                if (PatientStatus.Name == Name)
                {
                    return PatientStatus;
                }
            }
            Console.Error.WriteLine("There is no patientStatus with name: " + Name);
            return PatientStatuses[0];
        }

        public PatientStatus(string Name, Brush Color)
        {
            this.Name = Name;
            this.Color = Color;
            PatientStatuses.Add(this);
        }

        public static void AddDefault()
        {
            new PatientStatus("Registered", (Brush)converter.ConvertFromString("#83FFB9"));
            new PatientStatus("Injected", (Brush)converter.ConvertFromString("#FF79E0"));
            new PatientStatus("Discharged", (Brush)converter.ConvertFromString("#FF79E0"));
            //TODO: handle NM and other
            //new PatientStatus("NM", (Brush)converter.ConvertFromString("#79CBFF"));
            //new PatientStatus("OTHER", (Brush)converter.ConvertFromString("#FFCE79"));
        }
    }
}
