using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionSoftware.Class
{
    class InjectionsManager
    {
        private List<Injection> injections = new List<Injection>();

        public Injection getInjection(String patientID)
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

        public bool hasInjection(String patientID)
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

        public bool addInjection(String patientID, String patientSurname, String patientLastname)
        {
            //check if patient with patientID already existed in the database
            if (hasInjection(patientID)){
                return false;
            }

            //add the injection
            Injection injection = new Injection();
            injection.patient.patientID = patientID;
            injection.patient.patientSurname = patientSurname;
            injection.patient.patientLastname = patientLastname;

            injections.Add(injection);

            return true;
        }

        public void loadAllInjections()
        {

        }
    }
}
