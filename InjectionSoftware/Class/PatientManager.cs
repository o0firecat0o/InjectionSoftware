using InjectionSoftware.Util.Scheduler;
using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.IO;
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
            if (HasPatient(patient.PatientID))
            {
                Console.Out.WriteLine("Patient with patient ID:" + patient.PatientID + " is already presented in database, fail to add patiet");
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
                if (patient.PatientID.Equals(patientID))
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
                if (patient.PatientID.Equals(patientID)){
                    return true;
                }
            }
            return false;
        }

        // TODO: change to listening folder
        // TODO: network capability
        // TODO: add page to show patient info only
        public static void LoadAllPatient()
        {
            string fullpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\InjectionSoftware\" + @"\Schedular";

            if (!Directory.Exists(fullpath))
            {
                Directory.CreateDirectory(fullpath);
            }

            foreach (var file in
                Directory.EnumerateFiles(fullpath, "*.hl7"))
            {
                string text = System.IO.File.ReadAllText(file);
                Hl7file ff =  Hl7file.load(text);
                Console.Out.WriteLine(ff.getSegment("OBR").getString(3));
            }
        }
    }
}
