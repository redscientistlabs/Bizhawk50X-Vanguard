using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGlitchHarvester
{
    public enum BlastRadius
    {
        SPREAD,
        CHUNK,
        BURST,
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
        NONE
    }

    public enum CorruptionEngine
    {
        NIGHTMARE,
		VECTOR,
        NONE
    }

}
