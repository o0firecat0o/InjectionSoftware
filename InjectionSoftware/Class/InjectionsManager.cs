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
using System.Runtime.CompilerServices;
using System.Threading;

namespace InjectionSoftware.Class
{
    public static class InjectionsManager
    {
        public static ObservableCollection<Injection> injections = new ObservableCollection<Injection>();
        public static ObservableCollection<Injection> dischargedInjections = new ObservableCollection<Injection>();
        public static ObservableCollection<Patient> registeredPatients = new ObservableCollection<Patient>();

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

        public static bool hasInjection(string accessionNumber)
        {
            foreach (var injection in injections)
            {
                if (injection.AccessionNumber.Equals(accessionNumber))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool hasInjection_patientID(string patientID)
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

        public static Injection getInjection(string accessionNumber)
        {
            foreach (var injection in injections)
            {
                if (injection.AccessionNumber.Equals(accessionNumber))
                {
                    return injection;
                }
            }
            Console.Error.WriteLine();
            return null;
        }

        public static void modInjectionNetWork(string accessionNumber, Modality modality, string patientID, string patientSurname, string patientLastname, string UniqueExamIdentifier, string ExamCode, string DateOfBirth, bool Gender, bool Inpatient, string WardNumber, ObservableCollection<RP> RPs, Doctor Doctor, float UptakeTime, DateTime InjectionTime, Room SelectedRoom, bool isContrast, bool isDelay, string patientStatus)
        {
            Injection injection = modInjection(accessionNumber, modality, patientID, patientSurname, patientLastname, UniqueExamIdentifier, ExamCode, DateOfBirth, Gender, Inpatient, WardNumber, RPs, Doctor, UptakeTime, InjectionTime, SelectedRoom, isContrast, isDelay, patientStatus);
            if (!NetworkManager.isServer)
            {
                NetworkManager.client.TCPSendMessageToServer("modInjection", injection.toXML().ToString());
            }
            else
            {
                NetworkManager.server.TCPBroadcastMessage("modInjection", injection.toXML().ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessionNumber">leave accessionNumber as empty to generate new injection</param>
        /// <param name="patientID"></param>
        /// <param name="patientSurname"></param>
        /// <param name="patientLastname"></param>
        /// <param name="RPs"></param>
        /// <param name="Doctor"></param>
        /// <param name="UptakeTime"></param>
        /// <param name="InjectionTime"></param>
        /// <param name="SelectedRoom"></param>
        /// <param name="isContrast"></param>
        /// <param name="isDelay"></param>
        /// <param name="isDischarge"></param>
        /// <returns></returns>
        private static Injection modInjection(string accessionNumber, Modality modality, string patientID, string patientSurname, string patientLastname, string UniqueExamIdentifier, string ExamCode, string DateOfBirth, bool Gender, bool Inpatient, string WardNumber, ObservableCollection<RP> RPs, Doctor Doctor, float UptakeTime, DateTime InjectionTime, Room SelectedRoom, bool isContrast, bool isDelay, string patientStatus)
        {
            // find wether the patient is already registered and exist in the database
            Patient patient;
            if (PatientManager.HasPatient(patientID))
            {
                patient = PatientManager.GetPatient(patientID);
            }
            else
            {
                patient = new Patient();
                patient.PatientID = patientID;
            }

            // find whether the injection is already registered and exist in the database
            Injection injection;
            if (hasInjection(accessionNumber))
            {
                injection = getInjection(accessionNumber);
            }
            // loading previous injection from elsewhere
            else if (accessionNumber != "")
            {
                injection = new Injection();
                injection.AccessionNumber = accessionNumber;
                injections.Add(injection);
            }
            // adding completely new injection
            else
            {
                injection = new Injection();
                injection.AccessionNumber = Guid.NewGuid().ToString();
                injections.Add(injection);
            }


            // modify the patient information
            injection.Patient = patient;
            patient.PatientSurname = patientSurname;
            patient.PatientLastname = patientLastname;

            // modify the optional patient information
            // only a couple of patient information is modifiable
            patient.ExamCode = ExamCode;
            patient.UniqueExamIdentifier = UniqueExamIdentifier;
            patient.DateOfBirth = DateOfBirth;
            patient.IsMale = Gender;
            patient.IsInpatient = Inpatient;
            patient.WardNumber = WardNumber;

            // modify the injection information
            injection.Modality = modality;
            injection.RPs = RPs;
            injection.Doctor = Doctor;
            injection.UptakeTime = UptakeTime;
            injection.InjectionTime = InjectionTime;
            injection.SelectedRoom = SelectedRoom;
            injection.isContrast = isContrast;
            injection.isDelay = isDelay;
            injection.patientStatus = patientStatus;

            // add the patient to the patient manager, and save the patient information as an xml
            PatientManager.ModPatient(patient);

            // update the search string used for searching injection in InjectionPage
            // search string contained all information of the injection compressed in string format
            injection.updateSearchString();

            // recalculate the case number,
            // it is useful when the injection time is changed, since some injection may swap
            recreateObservableList();

            // save the injection in xml format
            saveInjection(injection.AccessionNumber);

            return injection;
        }

        public static void modInjection(XElement xElement)
        {
            XNamespace df = xElement.Name.Namespace;

            Modality modality = Modality.getModality(xElement.Element(df + "modality").Value);

            string accessionNumber = xElement.Element(df + "accessionNumber").Value;

            string patientID = xElement.Element(df + "patientID").Value;
            string patientSurname = xElement.Element(df + "patientSurname").Value;
            string patientLastname = xElement.Element(df + "patientLastname").Value;

            string uniqueExamIdentifier = xElement.Element(df + "uniqueExamIdentifier").Value;
            string examCode = xElement.Element(df + "examCode").Value;
            string dateOfBirth = xElement.Element(df + "dateOfBirth").Value;
            bool gender = bool.Parse(xElement.Element(df + "gender").Value);
            bool inpatient = bool.Parse(xElement.Element(df + "inpatient").Value);
            string wardNumber = xElement.Element(df + "wardNumber").Value;

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
            string patientStatus = (xElement.Element(df + "patientStatus").Value);

            modInjection(accessionNumber, modality, patientID, patientSurname, patientLastname, uniqueExamIdentifier, examCode, dateOfBirth, gender, inpatient, wardNumber, rPs, doctor, uptakeTime, injectionTime, room, isContrast, isDelay, patientStatus);
        }

        public static void changePatientStatusNetwork(string AccessionNumber, string patientStatus)
        {
            if (hasInjection(AccessionNumber))
            {
                if (NetworkManager.isServer)
                {
                    changePatientStatus(AccessionNumber, patientStatus);
                    NetworkManager.server.TCPBroadcastMessage("changePatientStatus", AccessionNumber + "^" + patientStatus);
                }
                else
                {
                    NetworkManager.client.TCPSendMessageToServer("changePatientStatus", AccessionNumber + "^" + patientStatus);
                }
            }
            else
            {
                Console.Error.WriteLine("[InjectionManager_changePatientStatusNetwork()] Error executing discharge injection command, reason: patient with accessionNumber does not exist");
            }
        }

        public static void changePatientStatus(string AccessionNumber, string patientStatus)
        {
            if (hasInjection(AccessionNumber))
            {
                getInjection(AccessionNumber).patientStatus = patientStatus;
                recreateObservableList();
                saveInjection(AccessionNumber);
            }
            else
            {
                Console.Error.WriteLine("[InjectionManager_changePatientStatus()] Error executing discharge injection command, reason: patient with accessionNumber does not exist");
            }
        }

        [Obsolete("dischargeInjectionNetwork is deprecated, please use changePatientStatusNetwork instead.")]
        public static void dischargeInjectionNetwork(string AccessionNumber)
        {
            if (hasInjection(AccessionNumber))
            {
                if (NetworkManager.isServer)
                {
                    dischargeInjection(AccessionNumber);
                    NetworkManager.server.TCPBroadcastMessage("dischargeInjection", AccessionNumber);
                }
                else
                {
                    NetworkManager.client.TCPSendMessageToServer("dischargeInjection", AccessionNumber);
                }
            }
            else
            {
                Console.Error.WriteLine("[InjectionManager] Error executing discharge injection command, reason: patient with accessionNumber does not exist");
            }
        }

        [Obsolete("dischargeInjectionNetwork is deprecated, please use changePatientStatusNetwork instead.")]
        public static void dischargeInjection(string AccessionNumber)
        {
            if (hasInjection(AccessionNumber))
            {
                getInjection(AccessionNumber).patientStatus = "Discharged";
                recreateObservableList();
                saveInjection(AccessionNumber);
            }
            else
            {
                Console.Error.WriteLine("[InjectionManager] Error executing discharge injection command, reason: patient with accessionNumber does not exist");
            }
        }

        public static void removeInjectionNetwork(string AccessionNumber)
        {
            if (hasInjection(AccessionNumber))
            {
                if (NetworkManager.isServer)
                {
                    removeInjection(AccessionNumber);
                    NetworkManager.server.TCPBroadcastMessage("removeInjection", AccessionNumber);
                }
                else
                {
                    NetworkManager.client.TCPSendMessageToServer("removeInjection", AccessionNumber);
                }
            }
            else
            {
                Console.Error.WriteLine("[InjectionManager] Error executing remove injection command, reason: patient with accessionNumber does not exist");
            }
        }

        public static void removeInjection(string accessionNumber)
        {
            if (hasInjection(accessionNumber))
            {
                foreach (Injection injection in injections)
                {
                    if (injection.AccessionNumber == accessionNumber)
                    {
                        injections.Remove(injection);
                        break;
                    }
                }

                recreateObservableList();

                delInjectionFile(accessionNumber);
            }
            else
            {
                Console.Error.WriteLine("[InjectionManager] Error executing remove injection command, reason: patient with accessionNumber does not exist");
            }
        }

        public static void recreateObservableList()
        {
            reassignCaseNumberOfDoctor();
            reassignCaseNumber();
            reassignRoom();
        }

        private static void reassignRoom()
        {
            // reassign patient (waiting for injection) to a list
            registeredPatients.Clear();
            foreach (Patient patient in PatientManager.Patients)
            {
                if (!InjectionsManager.hasInjection_patientID(patient.PatientID))
                {
                    registeredPatients.Add(patient);
                }
            }

            // assign patient with injection to corresponding room
            foreach (Room room in Room.Rooms)
            {
                //clear all previous injections of the room
                room.Injections.Clear();
                //loop through all injections one by one and assign them to room
                foreach (Injection injection in injections)
                {
                    if (injection.SelectedRoom == room && injection.patientStatus != "Discharged")
                    {
                        room.Injections.Add(injection);
                    }
                }
                //trigger the room injection changed method, it is used for changing the room page grid size
                room.InjectionsChanged();
            }

            // for the discharged row at the bottom of room page
            dischargedInjections.Clear();
            foreach (Injection injection in injections)
            {
                if (injection.patientStatus == "Discharged")
                {
                    dischargedInjections.Add(injection);
                }
            }
        }

        /// <summary>
        /// Rassign case number of doctor
        /// Used when case injection time changed, case assigned doctor changed etc
        /// </summary>
        private static void reassignCaseNumberOfDoctor()
        {
            List<Injection> tempinjections = returnSortedInjectionByTime();

            for (int i = 0; i < Doctor.Doctors.Count; i++)
            {
                int counter = 1;
                for (int j = 0; j < tempinjections.Count; j++)
                {
                    if (tempinjections[j].Doctor == Doctor.Doctors[i])
                    {
                        tempinjections[j].CaseNumberOfDoctor = counter;
                        counter++;
                    }
                }
            }
        }

        private static void reassignCaseNumber()
        {
            List<Injection> tempPETCTinjections = returnSortedInjectionByTime(Modality.getModality("PETCT"));

            for (int i = 0; i < tempPETCTinjections.Count; i++)
            {
                tempPETCTinjections[i].CaseNumber = i + 1;
            }

            List<Injection> tempPETMRinjections = returnSortedInjectionByTime(Modality.getModality("PETMR"));

            for (int i = 0; i < tempPETMRinjections.Count; i++)
            {
                tempPETMRinjections[i].CaseNumber = i + 1;
            }
        }

        /// <summary>
        /// clone the ObservableCollection(Injection) and convert it to list
        /// sort the list and return it as result
        /// </summary>
        /// <returns></returns>
        private static List<Injection> returnSortedInjectionByTime()
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

        private static List<Injection> returnSortedInjectionByTime(Modality modality)
        {
            List<Injection> returnInjection = returnSortedInjectionByTime();
            List<Injection> tempInjeciton = new List<Injection>();
            foreach (Injection injection in returnInjection)
            {
                if (injection.Modality == modality)
                {
                    tempInjeciton.Add(injection);
                }
            }

            return tempInjeciton;
        }

        public static void loadAllInjections()
        {
            Console.WriteLine("[InjectionManager] Loading previous injection");

            string date = DateTime.Now.ToString("ddMMyyyy");
            string fullpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\InjectionSoftware\" + date + @"\injection\";

            if (!Directory.Exists(fullpath))
            {
                Directory.CreateDirectory(fullpath);
            }

            foreach (var file in
                Directory.EnumerateFiles(fullpath, "*.xml"))
            {
                Console.Out.WriteLine("[InjectionManager] loading injection from location: {0}", file);
                XElement xElement = XElement.Load(file);

                modInjection(xElement);
            }
        }

        public static void saveInjection(string accessionNumber)
        {
            Injection injection;
            if (InjectionsManager.hasInjection(accessionNumber))
            {
                injection = InjectionsManager.getInjection(accessionNumber);
            }
            else
            {
                Console.Error.WriteLine("Injection with accessionNumber: {0} does not exist, @saveInjection()/InjectionManager", accessionNumber);
                return;
            }

            XElement xmlFile = injection.toXML();

            string date = DateTime.Now.ToString("ddMMyyyy");
            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "InjectionSoftware", date, "injection");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fullpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\InjectionSoftware\" + date + @"\injection\" + accessionNumber + ".xml";

            Console.WriteLine("[InjectionManager] saving injection of accessionNumber: {0}, to: {1}", accessionNumber, fullpath);
            xmlFile.Save(fullpath);
        }

        private static void delInjectionFile(string accessionNumber)
        {
            try
            {
                string date = DateTime.Now.ToString("ddMMyyyy");
                string fullpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\InjectionSoftware\" + date + @"\injection\" + accessionNumber + ".xml";
                if (File.Exists(fullpath))
                {
                    // If file found, delete it    
                    File.Delete(fullpath);
                    Console.WriteLine("Injection with accessionNumber: " + accessionNumber + " has been deleted.");
                }
                else Console.WriteLine("Injection with accessionNumber: " + accessionNumber + " has not been found.");
            }
            catch (IOException ioExp)
            {
                Console.WriteLine(ioExp.Message);
            }
        }
    }
}
