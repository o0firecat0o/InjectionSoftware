using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace InjectionSoftware.Class
{
    public static class PatientManager
    {
        private static List<Patient> patients = new List<Patient>();

        public static void AddPatient(Patient patient)
        {
            if (HasPatient(patient.patientID))
            {
                Console.Out.WriteLine("Patient with patient ID:" + patient.patientID + " is already presented in database, fail to add patiet");
                return;
            }
            patients.Add(patient);
        }
        public static void AddPatient(string patientID, string patientSurname, string patientLastname)
        {
            if (HasPatient(patientID))
            {
                Console.Out.WriteLine("Patient with patient ID:" + patientID + " is already presented in database, fail to add patiet");
                return;
            }
            Patient patient = new Patient(patientID, patientSurname, patientLastname);
            patients.Add(patient);
        }

        public static Patient GetPatient(string patientID)
        {
            foreach (Patient patient in patients)
            {
                if (patient.patientID.Equals(patientID))
                {
                    return patient;
                }
            }
            Console.Out.WriteLine("Patient with patient ID:" + patientID + "does not exist in database");
            return null;
        }

        public static bool HasPatient(string patientID)
        {
            foreach (Patient patient in patients)
            {
                if (patient.patientID.Equals(patientID)){
                    return true;
                }
            }
            return false;
        }

       
    }
}
