using InjectionSoftware.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionSoftware.Class
{
    public static class InjectionsManager
    {
        public static ObservableCollection<Injection> injections = new ObservableCollection<Injection>();


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

        public static Injection addInjection(string patientID, string patientSurname, string patientLastname, ObservableCollection<RP> RPs, Doctor Doctor, float UptakeTime, DateTime InjectionTime, Room SelectedRoom)
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
            // TODO: correct count after delete or adding <= sort by injection time
            // add the injection
            Injection injection = new Injection(patient, RPs, Doctor, UptakeTime, InjectionTime, SelectedRoom);
            
            injections.Add(injection);

            reassignCaseNumberOfDoctor();
            reassignCaseNumber();

            return injection;
        }

        public static void modInjection(Injection Injection, string patientID, string patientSurname, string patientLastname, ObservableCollection<RP> RPs, Doctor Doctor, float UptakeTime, DateTime InjectionTime, Room SelectedRoom)
        {
            Injection.Patient.PatientID = patientID;
            Injection.Patient.PatientSurname = patientSurname;
            Injection.Patient.PatientLastname = patientLastname;
            Injection.RPs = RPs;
            Injection.Doctor = Doctor;
            Injection.UptakeTime = UptakeTime;
            Injection.InjectionTime = InjectionTime;
            Injection.SelectedRoom = SelectedRoom;

            reassignCaseNumberOfDoctor();
            reassignCaseNumber();
        }

        public static void delInjection(Injection Injection)
        {
            injections.Remove(Injection);

            reassignCaseNumberOfDoctor();
            reassignCaseNumber();
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

        }
    }
}
