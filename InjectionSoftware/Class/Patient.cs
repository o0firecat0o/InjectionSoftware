using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionSoftware.Class
{
    public class Patient
    {
        public string patientSurname;
        public string patientLastname;
        public string patientID;

        public Patient(string patientID, string patientSurname, string patientLastname)
        {
            this.patientID = patientID;
            this.patientLastname = patientLastname;
            this.patientSurname = patientSurname;
            PatientManager.AddPatient(this);
        }
    }
}
