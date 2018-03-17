using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTC
{
    public enum BlastRadius
    {
        SPREAD,
        CHUNK,
        BURST,
		NORMALIZED,
		PROPORTIONAL,
		EVEN,
		NONE
    }

    public enum BlastByteType
    {
        SET,
        ADD,
        SUBSTRACT,
        NONE
    }

    public enum BlastByteAlgo
    {
        RANDOM,
        RANDOMTILT,
        TILT,
    }

    public enum CorruptionEngine
    {
        NIGHTMARE,
        HELLGENIE,
        DISTORTION,
        FREEZE,
		PIPE,
		VECTOR,
		BIT,
        EXTERNALROM,
        NONE
    }

}
