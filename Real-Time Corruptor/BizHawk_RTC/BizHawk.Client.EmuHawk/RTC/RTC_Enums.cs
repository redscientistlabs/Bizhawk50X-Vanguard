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
		ADDRESS,
		BACKUP,
		VECTOR
	}
	public enum BackupSource
	{
		IMMEDIATE,  //Frame 0 for the blastunit. Right when it's applied. Used for Distortion
		PREEXECUTE, //The frame where the blastunit is going to start executing. Used for freeze
		NONE
	}

	public enum BlastUnitAlgo
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
		BLASTGENERATORENGINE,
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
