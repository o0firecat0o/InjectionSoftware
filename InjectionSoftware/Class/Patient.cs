using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionSoftware.Class
{
    public class Patient
    {
        public String patientSurname;
        public String patientLastname;
        public String patientID;

        public Patient(String patientID, String patientSurname, String patientLastname)
        {
            this.patientID = patientID;
            this.patientLastname = patientLastname;
            this.patientSurname = patientSurname;
            PatientManager.AddPatient(this);
        }
    }
}
