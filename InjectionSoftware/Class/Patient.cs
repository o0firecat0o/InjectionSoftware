using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionSoftware.Class
{
    public class Patient
    {
        public string patientSurname { get; set; }
        public string patientLastname { get; set; }
        public string patientID { get; set; }

        public Patient(string patientID, string patientSurname, string patientLastname)
        {
            this.patientID = patientID;
            this.patientLastname = patientLastname;
            this.patientSurname = patientSurname;
            PatientManager.AddPatient(this);
        }
    }
}
