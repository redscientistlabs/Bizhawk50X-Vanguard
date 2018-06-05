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

	public enum BGBlastModes
	{
		SHIFT,
		ADD,
		SUBTRACT,
		SET,
		RANDOM,
		BITWISE_SHIFT_LEFT,
		BITWISE_SHIFT_RIGHT,
		BITWISE_AND,
		BITWISE_OR,
		BITWISE_XOR,
		BITWISE_COMPLEMENT,
		REPLACE_X_WITH_Y,
	}
	public enum BGBlastCheatModes
	{
		FREEZE
	}
	public enum BGBlastPipeModes
	{
		SHIFT
	}

}
