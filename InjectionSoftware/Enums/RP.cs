using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionSoftware.Enums
{
    [Flags]
    public enum RP
    {
        Null = 0x00,
        F18_FDG = 0x01,
        F18_PSMA = 0x02,
        F18_FLDopa = 0x04,
        C11_Acetate = 0x08,
        C11_PIB = 0x10,
        C11_Methionine = 0x20,
        Ga68_Dotatate = 0x40,
        Ga68_PSMA = 0x80,
    }
}
