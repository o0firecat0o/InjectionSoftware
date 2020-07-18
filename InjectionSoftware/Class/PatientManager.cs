using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionSoftware.Class
{
    public static class PatientManager
    {
        private static List<Patient> patients = new List<Patient>();

        public static void AddPatient(Patient patient)
        {
            patients.Add(patient);
        }
    }
}
