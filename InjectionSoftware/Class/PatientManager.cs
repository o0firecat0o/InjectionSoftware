using InjectionSoftware.Network;
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
using System.Xml.Linq;

namespace InjectionSoftware.Class
{
    public static class PatientManager
    {
        public static ObservableCollection<Patient> Patients = new ObservableCollection<Patient>();

        public static void ModPatient(Patient patient)
        {
            if (HasPatient(patient.PatientID))
            {
                Console.Out.WriteLine("[PatientManager.AddPatient()] Patient with patient ID:" + patient.PatientID + " is already presented in database, fail to add patiet");
            }
            else
            {
                Patients.Add(patient);
            }

            // save the patient to a xml file
            // if the program is terminated, the next reboot will load the xml file and add the patient back
            SavePatient(patient);

            // forward the patient information to all clients
            // this is actually redundunt before adding auto load patient from schedular
            //if (NetworkManager.isServer)
            //{
            //   NetworkManager.server.TCPBroadcastMessage("addPatient", patient.toXML().ToString());
            //}

            InjectionsManager.recreateObservableList();
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
                if (patient.PatientID == patientID)
                {
                    return true;
                }
            }
            return false;
        }

        [Obsolete("The method has been replaced by SchedularSyncManager/LoadInitial()")]
        public static void LoadAllPatientFromSchedular()
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
                    Console.Out.WriteLine("[PatientManager] Loading patient information from HL7 file with patientID:" + patient.PatientID);
                    if (patient.PatientID == "")
                    {
                        Console.WriteLine("[PatientManager.LoadAllPatient()] " + file + " path contain corrupted information, the patient information has failed to load");
                        continue;
                    }
                    ModPatient(patient);

                }
                catch (System.Exception e)
                {
                    Console.Error.WriteLine(e);
                }
            }

            InjectionsManager.recreateObservableList();
        }

        [Obsolete("The method has been replaced by SchedularSyncManager/LoadInitial()")]
        public static void LoadAllPatient()
        {
            Console.WriteLine("[InjectionManager] Loading previous logged patients");

            string date = DateTime.Now.ToString("ddMMyyyy");
            string fullpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\InjectionSoftware\" + date + @"\patient\";

            if (!Directory.Exists(fullpath))
            {
                Directory.CreateDirectory(fullpath);
            }

            foreach (var file in
                Directory.EnumerateFiles(fullpath, "*.xml"))
            {
                Console.Out.WriteLine("[PatientManager] loading patient from location: {0}", file);
                XElement xElement = XElement.Load(file);

                Patient patient = new Patient(xElement);
                ModPatient(patient);
            }
        }

        public static void SavePatient(Patient patient)
        {

            XElement xmlFile = patient.toXML();

            string date = DateTime.Now.ToString("ddMMyyyy");
            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "InjectionSoftware", date, "patient");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fullpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\InjectionSoftware\" + date + @"\patient\" + patient.PatientID + ".xml";

            Console.WriteLine("[PatientManager] saving patient of accessionNumber: {0}, to: {1}", patient.PatientID, fullpath);
            xmlFile.Save(fullpath);
        }
    }
}
