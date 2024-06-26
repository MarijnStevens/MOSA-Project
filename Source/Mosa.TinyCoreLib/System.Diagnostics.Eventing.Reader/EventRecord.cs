using System.Collections.Generic;
using System.Security.Principal;

namespace System.Diagnostics.Eventing.Reader;

public abstract class EventRecord : IDisposable
{
	public abstract Guid? ActivityId { get; }

	public abstract EventBookmark Bookmark { get; }

	public abstract int Id { get; }

	public abstract long? Keywords { get; }

	public abstract IEnumerable<string> KeywordsDisplayNames { get; }

	public abstract byte? Level { get; }

	public abstract string LevelDisplayName { get; }

	public abstract string LogName { get; }

	public abstract string MachineName { get; }

	public abstract short? Opcode { get; }

	public abstract string OpcodeDisplayName { get; }

	public abstract int? ProcessId { get; }

	public abstract IList<EventProperty> Properties { get; }

	public abstract Guid? ProviderId { get; }

	public abstract string ProviderName { get; }

	public abstract int? Qualifiers { get; }

	public abstract long? RecordId { get; }

	public abstract Guid? RelatedActivityId { get; }

	public abstract int? Task { get; }

	public abstract string TaskDisplayName { get; }

	public abstract int? ThreadId { get; }

	public abstract DateTime? TimeCreated { get; }

	public abstract SecurityIdentifier UserId { get; }

	public abstract byte? Version { get; }

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public abstract string FormatDescription();

	public abstract string FormatDescription(IEnumerable<object> values);

	public abstract string ToXml();
}
