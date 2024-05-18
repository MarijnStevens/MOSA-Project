namespace System.Diagnostics.PerformanceData;

public enum CounterType
{
	RawDataHex32 = 0,
	RawDataHex64 = 256,
	RawData32 = 65536,
	RawData64 = 65792,
	Delta32 = 4195328,
	Delta64 = 4195584,
	SampleCounter = 4260864,
	QueueLength = 4523008,
	LargeQueueLength = 4523264,
	QueueLength100Ns = 5571840,
	QueueLengthObjectTime = 6620416,
	RateOfCountPerSecond32 = 272696320,
	RateOfCountPerSecond64 = 272696576,
	RawFraction32 = 537003008,
	RawFraction64 = 537003264,
	PercentageActive = 541132032,
	PrecisionSystemTimer = 541525248,
	PercentageActive100Ns = 542180608,
	PrecisionTimer100Ns = 542573824,
	ObjectSpecificTimer = 543229184,
	PrecisionObjectSpecificTimer = 543622400,
	SampleFraction = 549585920,
	PercentageNotActive = 557909248,
	PercentageNotActive100Ns = 558957824,
	MultiTimerPercentageActive = 574686464,
	MultiTimerPercentageActive100Ns = 575735040,
	MultiTimerPercentageNotActive = 591463680,
	MultiTimerPercentageNotActive100Ns = 592512256,
	AverageTimer32 = 805438464,
	ElapsedTime = 807666944,
	AverageCount64 = 1073874176,
	SampleBase = 1073939457,
	AverageBase = 1073939458,
	RawBase32 = 1073939459,
	RawBase64 = 1073939712,
	MultiTimerBase = 1107494144
}
