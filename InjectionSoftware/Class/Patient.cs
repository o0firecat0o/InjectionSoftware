using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionSoftware.Class
{
    //TODO: implement INotifyPropertyChanged to patient's property
    public class Patient
    {
        public string patientSurname { get; set; }
        public string patientLastname { get; set; }
        public string patientID { get; set; }

        public string PatientFullname
        {
            get
            {
                return patientSurname + " " + patientLastname;
            }
        }

        public Patient(string patientID, string patientSurname, string patientLastname)
        {
            this.patientID = patientID;
            this.patientLastname = patientLastname;
            this.patientSurname = patientSurname;
            PatientManager.AddPatient(this);
        }
    }
}
