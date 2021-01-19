using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionSoftware.Util
{
    public class DOBconverter
    {
        public static string hl7DOBtoBarcodeDOB(string hl7DOB)
        {
            if (hl7DOB.Length == 8)
            {
                return (hl7DOB.Substring(6,2)+"-"+hl7DOB.Substring(4,2)+"-"+hl7DOB.Substring(0, 4));
            }
            else
            {
                return "";
            }
        }

        public static string BarcodeDOBtohl7DOB(string BarcodeDOB)
        {
            if (BarcodeDOB.Length == 10)
            {
                return (BarcodeDOB.Substring(6, 4) + "" + BarcodeDOB.Substring(3, 2) + "" + BarcodeDOB.Substring(0, 2));
            }
            else
            {
                return "";
            }
        }
    }
}
