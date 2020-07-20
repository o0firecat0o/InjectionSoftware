using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionSoftware.Class
{
    public static class InjectionsManager
    {
        public static ObservableCollection<Injection> injections = new ObservableCollection<Injection>();

        public static Injection getInjection(String patientID)
        {
            foreach (var injection in injections)
            {
                if (injection.patient.patientID.Equals(patientID))
                {
                    return injection;
                }
            }
            throw new System.Exception("No patient with patient ID: " + patientID+", is registered. @InjectionManagers/getInjection()");
        }

        public static bool hasInjection(String patientID)
        {
            foreach (var injection in injections)
            {
                if (injection.patient.patientID.Equals(patientID))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool addInjection(String patientID, String patientSurname, String patientLastname)
        {
            // TODO: change to add more
            // check if patient with patientID already existed in the database
            if (hasInjection(patientID)){
                return false;
            }

            //add the injection
            Injection injection = new Injection();
            Patient patient = new Patient(patientID, patientSurname, patientLastname);
            injection.patient = patient;

            injections.Add(injection);

            return true;
        }

        public static void loadAllInjections()
        {

        }
    }
}
