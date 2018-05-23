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
		VECTOR,
        NONE
	}

	public enum BlastCheatType
	{
		FREEZE,
		HELLGENIE
	}
	
	public enum BlastPipeType
	{
		SET,
		ADD,
		SUBSTRACT,
		MULTIPLY,
		DIVIDE
	}

	public enum BlastPipeAlgo
	{
		VALUE,
		TILTVALUE,
		TILT,
		NONE
	}

	public enum BlastByteAlgo
    {
        RANDOM,
        RANDOMTILT,
        TILT,
		VECTOR,
		NONE
    }

    public enum CorruptionEngine
    {
        NIGHTMARE,
        HELLGENIE,
        DISTORTION,
        FREEZE,
		PIPE,
		VECTOR,
        EXTERNALROM,
        NONE
    }

}
