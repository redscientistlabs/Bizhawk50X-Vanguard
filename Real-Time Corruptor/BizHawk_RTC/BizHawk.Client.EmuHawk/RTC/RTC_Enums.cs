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


	public enum BlastUnitSource
	{
		VALUE,
		STORE
	}
	public enum ActionTime
	{
		GENERATE,   //For when something is happening at generate time
		IMMEDIATE,  //Frame 0 for the blastunit. Right when it's applied. Used for Distortion
		PREEXECUTE, //For when you want it to happen right before the first step
		EXECUTE     //For when you want it to happen every step
	}
	public enum StoreType
	{
		ONCE, 
		CONTINUOUS
	}

	public enum NightmareAlgo
	{
		RANDOM,
		RANDOMTILT,
		TILT
	}
	public enum NightmareType
	{
		SET,
		ADD,
		SUBTRACT
	}

	public enum CustomUnitSource
	{
		VALUE,
		STORE
	}

	public enum CustomValueSource
	{
		RANDOM,
		VALUELIST,
		RANGE
	}
	public enum CustomStoreAddress
	{
		SAME,
		RANDOM
	}

	public enum CorruptionEngine
	{
		NIGHTMARE,
		HELLGENIE,
		DISTORTION,
		FREEZE,
		PIPE,
		VECTOR,
		BLASTGENERATORENGINE,
		CUSTOM,
		NONE
	}

	public enum BGBlastByteModes
	{
		SET,
		ADD,
		SUBTRACT,
		RANDOM,
		RANDOM_RANGE,
		SHIFT_LEFT,
		SHIFT_RIGHT,
		REPLACE_X_WITH_Y,
		BITWISE_AND,
		BITWISE_OR,
		BITWISE_XOR,
		BITWISE_COMPLEMENT,
		BITWISE_SHIFT_LEFT,
		BITWISE_SHIFT_RIGHT,
		BITWISE_ROTATE_LEFT,
		BITWISE_ROTATE_RIGHT
	}

	public enum BGBlastCheatModes
	{
		SET,
		ADD,
		SUBTRACT,
		RANDOM,
		RANDOM_RANGE,
		SHIFT_LEFT,
		SHIFT_RIGHT,
		FREEZE,
		REPLACE_X_WITH_Y,
		BITWISE_AND,
		BITWISE_OR,
		BITWISE_XOR,
		BITWISE_COMPLEMENT,
		BITWISE_SHIFT_LEFT,
		BITWISE_SHIFT_RIGHT,
		BITWISE_ROTATE_LEFT,
		BITWISE_ROTATE_RIGHT
	}

	public enum BGBlastPipeModes
	{
		CHAINED,
		SOURCE_SET,
		SOURCE_RANDOM,
		DEST_RANDOM
	}

	public enum ProblematicItemTypes
	{
		PROCESS,
		ASSEMBLY
	}
}
