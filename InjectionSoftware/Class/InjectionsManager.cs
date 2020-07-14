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
                if (injection.patientID.Equals(patientID))
                {
                    return injection;
                }
            }
            throw new System.Exception("No patient with patient ID: " + patientID+", is registered. @InjectionManagers/getInjection()");
        }

        public void addInjection(String patientID = null)
        {

        }

        public void loadAllInjections()
        {

        }
    }
}
