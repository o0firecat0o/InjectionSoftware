using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionSoftware.Util.Scheduler
{
    public class Hl7segment
    {
        public readonly string header;
        List<string> list = new List<string>();

        public Hl7segment(string fullstring)
        {
            try
            {
                string[] seperatedString = fullstring.Split('|');
                header = seperatedString[0];
                for (int i = 0; i < seperatedString.Length; i++)
                {
                    list.Add(seperatedString[i]);
                }

            }catch(System.Exception e)
            {
                Console.Error.WriteLine(e);
            }
            
        }

        public string getString(int index)
        {
            return list[index];
        }
    }
}
