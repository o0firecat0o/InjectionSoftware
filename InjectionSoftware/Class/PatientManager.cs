using InjectionSoftware.Util.Scheduler;
using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace InjectionSoftware.Class
{
    public static class PatientManager
    {
        public static ObservableCollection<Patient> Patients = new ObservableCollection<Patient>();

        public static void AddPatient(Patient patient)
        {
            if (HasPatient(patient.PatientID))
            {
                Console.Out.WriteLine("Patient with patient ID:" + patient.PatientID + " is already presented in database, fail to add patiet");
                return;
            }
            Patients.Add(patient);
        }

        public static Patient GetPatient(string patientID)
        {
            foreach (Patient patient in Patients)
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
            foreach (Patient patient in Patients)
            {
                if (patient.PatientID == patientID){
                    return true;
                }
            }
            return false;
        }

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
                try
                {
                    string text = System.IO.File.ReadAllText(file);
                    Hl7file ff = Hl7file.load(text);
                    Patient patient = new Patient(ff);
                    AddPatient(patient);
                }
                catch(System.Exception e)
                {
                    Console.Error.WriteLine(e);
                }
            }

            InjectionsManager.recreateObservableList();
        }
    }
}
