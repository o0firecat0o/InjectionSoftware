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
                if (injection.Patient.patientID.Equals(patientID))
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
                if (injection.Patient.patientID.Equals(patientID))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool addInjection(string patientID, string patientSurname, string patientLastname, ObservableCollection<RP> RPs, Doctor Doctor, float UptakeTime, DateTime InjectionTime, Room SelectedRoom)
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
            Injection injection = new Injection(patient, injections.Count + 1, RPs, Doctor, UptakeTime, InjectionTime, SelectedRoom);
            
            injections.Add(injection);

            return true;
        }

        public static void loadAllInjections()
        {

        }
    }
}
