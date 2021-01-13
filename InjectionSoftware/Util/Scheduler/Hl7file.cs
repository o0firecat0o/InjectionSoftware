using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionSoftware.Util.Scheduler
{
    public class Hl7file
    {
        List<Hl7segment> hl7Segments = new List<Hl7segment>();

        public void addSegment(Hl7segment segment)
        {
            hl7Segments.Add(segment);
        }

        public Hl7segment getSegment(string header)
        {
            foreach (var item in hl7Segments)
            {
                if(item.header == header)
                {
                    return item;
                }
            }
            Console.Error.WriteLine("[HL7 file] segment with header name: {0} does not exist", header);
            return new Hl7segment("");            
        }

        public bool hasSegment(string header)
        {
            foreach (var item in hl7Segments)
            {
                if (item.header == header)
                {
                    return true;
                }
            }
            return false;
        }

        public static Hl7file load(string fullString)
        {
            try
            {
                Hl7file hl7File = new Hl7file();
                string[] seperatedString = fullString.Split('\r');
                foreach (var item in seperatedString)
                {
                    if (item.Split('|')[0].Length >= 2)
                    {
                        hl7File.addSegment(new Hl7segment(item));
                    }
                }

                return hl7File;
            }
            catch(System.Exception e)
            {
                Console.Out.WriteLine(e);
                return null;
            }            
        }
    }
}
