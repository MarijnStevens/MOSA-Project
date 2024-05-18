namespace System.Runtime.CompilerServices;

public readonly struct TaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public bool IsCompleted
	{
		get
		{
			throw null;
		}
	}

	public void GetResult()
	{
	}

	public void OnCompleted(Action continuation)
	{
	}

	public void UnsafeOnCompleted(Action continuation)
	{
	}
}
public readonly struct TaskAwaiter<TResult> : ICriticalNotifyCompletion, INotifyCompletion
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public bool IsCompleted
	{
		get
		{
			throw null;
		}
	}

	public TResult GetResult()
	{
		throw null;
	}

	public void OnCompleted(Action continuation)
	{
	}

	public void UnsafeOnCompleted(Action continuation)
	{
	}
}
