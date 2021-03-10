using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionSoftware.Util
{
    public static class ConsoleLogger
    {
        public static StringWriter sw = new StringWriter();

        public static void Init()
        {
            Console.SetOut(sw);
            Console.SetError(sw);
        }
    }
}
