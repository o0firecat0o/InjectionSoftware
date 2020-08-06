using InjectionSoftware.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using InjectionSoftware.Network;

namespace InjectionSoftware.Class
{
    public static class InjectionsManager
    {
        public static ObservableCollection<Injection> injections = new ObservableCollection<Injection>();

        public static void Init()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(30);
            dispatcherTimer.Tick += new EventHandler(delegate (object s, EventArgs a)
            {
                foreach (Injection injection in injections)
                {
                    injection.Update();
                }
            });
            dispatcherTimer.Start();
        }

        public static Injection getInjection(string patientID)
        {
            foreach (var injection in injections)
            {
                if (injection.Patient.PatientID.Equals(patientID))
                {
                    return injection;
                }
            }
            throw new System.Exception("No patient with patient ID: " + patientID+", is registered. @InjectionManagers/getInjection()");
        }

        public static bool hasInjection(string patientID)
        {
            foreach (var injection in injections)
            {
                if (injection.Patient.PatientID.Equals(patientID))
                {
                    return true;
                }
            }
            return false;
        }

        public static Injection addInjection(string patientID, string patientSurname, string patientLastname, ObservableCollection<RP> RPs, Doctor Doctor, float UptakeTime, DateTime InjectionTime, Room SelectedRoom, bool isContrast, bool isDelay, bool isDischarge)
        {
            // find wether the patient is already registered and exist in the database
            Patient patient;
            if (PatientManager.HasPatient(patientID))
            {
                patient = PatientManager.GetPatient(patientID);
            }
            else
            {
                patient = new Patient(patientID, patientSurname, patientLastname);
            }

            Console.Out.WriteLine(patientSurname);

            // TODO: avoid duplicated adding of patient
            // add the injection
            Injection injection = new Injection(patient, RPs, Doctor, UptakeTime, InjectionTime, SelectedRoom, isContrast, isDelay, isDischarge);
            
            injections.Add(injection);

            reassignCaseNumberOfDoctor();
            reassignCaseNumber();

            saveInjection(patientID);

            if (!NetworkManager.isServer)
            {
                // for client, send add message to server after adding new injection
                NetworkManager.client.TCPSendMessageToServer("AddInjection", injection.toXML().ToString());
            }

            return injection;
        }

        public static void modInjection(Injection Injection, string patientID, string patientSurname, string patientLastname, ObservableCollection<RP> RPs, Doctor Doctor, float UptakeTime, DateTime InjectionTime, Room SelectedRoom, bool isContrast, bool isDelay, bool isDischarge)
        {
            Injection.Patient.PatientID = patientID;
            Injection.Patient.PatientSurname = patientSurname;
            Injection.Patient.PatientLastname = patientLastname;
            Injection.RPs = RPs;
            Injection.Doctor = Doctor;
            Injection.UptakeTime = UptakeTime;
            Injection.InjectionTime = InjectionTime;
            Injection.SelectedRoom = SelectedRoom;
            Injection.isContrast = isContrast;
            Injection.isDelay = isDelay;
            Injection.isDischarge = isDischarge;


            reassignCaseNumberOfDoctor();
            reassignCaseNumber();

            saveInjection(patientID);

            if (!NetworkManager.isServer)
            {
                // for client, send add message to server after adding new injection
                NetworkManager.client.TCPSendMessageToServer("ModInjection", Injection.toXML().ToString());
            }
        }

        public static void dischargeInjection(Injection Injection, bool isDischarge)
        {
            Injection.isDischarge = isDischarge;
        }

        public static void delInjection(Injection Injection)
        {
            injections.Remove(Injection);

            reassignCaseNumberOfDoctor();
            reassignCaseNumber();

            delInjection(Injection.Patient.PatientID);
        }

        /// <summary>
        /// Rassign case number of doctor
        /// Used when case injection time changed, case assigned doctor changed etc
        /// </summary>
        public static void reassignCaseNumberOfDoctor()
        {
            List<Injection> tempinjections = returnSortedInjectionByTime();

            for (int i = 0; i < Doctor.Doctors.Count; i++)
            {
                int counter = 1;
                for (int j = 0; j < tempinjections.Count; j++)
                {
                    if(tempinjections[j].Doctor == Doctor.Doctors[i])
                    {
                        tempinjections[j].CaseNumberOfDoctor = counter;
                        counter++;
                    }
                }
            }
        }

        public static void reassignCaseNumber()
        {
            List<Injection> tempinjections = returnSortedInjectionByTime();

            for (int i = 0; i < tempinjections.Count; i++)
            {
                tempinjections[i].CaseNumber = i + 1;
            }
        }

        /// <summary>
        /// clone the ObservableCollection(Injection) and convert it to list
        /// sort the list and return it as result
        /// </summary>
        /// <returns></returns>
        public static List<Injection> returnSortedInjectionByTime()
        {

            //Clone a Observable collection and convert it to list
            List<Injection> tempinjections = new List<Injection>();
            foreach (var injection in injections)
            {
                tempinjections.Add(injection);
            }

            //sort the list according to the injection time
            tempinjections.Sort((x, y) => (DateTime.Compare(x.InjectionTime, y.InjectionTime)));

            return tempinjections;
        }

        public static void loadAllInjections()
        {
            Console.WriteLine("Loading previous injection");

            string date = DateTime.Now.ToString("ddMMyyyy");
            string fullpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\InjectionSoftware\" + date;

            if (!Directory.Exists(fullpath))
            {
                Directory.CreateDirectory(fullpath);
            }

            foreach (var file in
                Directory.EnumerateFiles(fullpath, "*.xml"))
            {
                Console.Out.WriteLine(file);
                XElement xElement = XElement.Load(file);
                XNamespace df = xElement.Name.Namespace;
                string patientID = xElement.Element(df + "patientID").Value;
                string patientSurname = xElement.Element(df + "patientSurname").Value;
                string patientLastname = xElement.Element(df + "patientLastname").Value;

                ObservableCollection<RP> rPs = new ObservableCollection<RP>();
                if (xElement.Element(df + "rp1").Value != "")
                {
                    rPs.Add(RP.getRP(xElement.Element(df + "rp1").Value));
                }
                if (xElement.Element(df + "rp2").Value != "")
                {
                    rPs.Add(RP.getRP(xElement.Element(df + "rp2").Value));
                }

                Doctor doctor = Doctor.getDoctor(xElement.Element(df + "doctor").Value);

                float uptakeTime = float.Parse(xElement.Element(df + "uptakeTime").Value);
                DateTime injectionTime = Convert.ToDateTime(xElement.Element(df + "injectionTime").Value);

                Room room = Room.getRoom(xElement.Element(df + "selectedRoom").Value);

                bool isContrast = bool.Parse(xElement.Element(df + "isContrast").Value);
                bool isDelay = bool.Parse(xElement.Element(df + "isDelay").Value);
                bool isDischarge = bool.Parse(xElement.Element(df + "isDischarge").Value);

                addInjection(patientID, patientSurname, patientLastname, rPs, doctor, uptakeTime, injectionTime, room, isContrast, isDelay, isDischarge);
            }
        }

        public static void saveInjection(string patientID)
        {
            Injection injection;
            if (InjectionsManager.hasInjection(patientID))
            {
                injection = InjectionsManager.getInjection(patientID);
            }
            else
            {
                Console.Error.WriteLine("Injection with patientID does not exist, @saveInjection()/InjectionManager");
                return;
            }

            XElement xmlFile = injection.toXML();

            string date = DateTime.Now.ToString("ddMMyyyy");
            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "InjectionSoftware", date);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fullpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\InjectionSoftware\" + date + @"\" + patientID + ".xml";

            Console.WriteLine("[InjectionManager] saving injection of patientID: " + patientID +",to: "+fullpath);
            xmlFile.Save(fullpath);
        }

        public static void delInjection(string patientID)
        {
            try
            {
                string date = DateTime.Now.ToString("ddMMyyyy");
                string fullpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\InjectionSoftware\" + date + @"\" + patientID + ".xml";
                if (File.Exists(fullpath))
                {
                    // If file found, delete it    
                    File.Delete(fullpath);
                    Console.WriteLine("Injection with patient ID: "+patientID +" has been deleted.");
                }
                else Console.WriteLine("Injection with patient ID: " + patientID + " has not been found.");
            }
            catch (IOException ioExp)
            {
                Console.WriteLine(ioExp.Message);
            }
        }
    }
}
