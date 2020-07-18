using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionSoftware.Class
{
    public class Injection 
    {
        public Patient patient;
        private string accessionNumber;
        public int caseNumber;                

        public string AccessionNumber
        {
            get
            {
                return accessionNumber;
            }
            set
            {
                accessionNumber = value;                
            }
        }
    }
}
