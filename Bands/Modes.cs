using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolSDR.Modes
{
    public enum ModeKind
    {
        None,
        Begin = 0,
        LSB = 2,
        USB = 4,
        AM = 8,
        SAM = 16,
        FM = 32,
        DIGI = 64,
        CW = 128,
        All = LSB|USB|AM|SAM|CW|FM|DIGI,
        End = 256
    }
}
