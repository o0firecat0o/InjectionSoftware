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


        public int CaseNumber 
        { 
            get; set; 
        }

        public string AccessionNumber
        {
            get; set;
        }

        public int Row
        {
            get
            {
                return CaseNumber - 1;
            }
        }
    }
}
