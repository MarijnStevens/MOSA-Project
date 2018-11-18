// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Dsb32 - Data Synchronization Barrier
	/// </summary>
	/// <seealso cref="Mosa.Platform.ARMv6.ARMv6Instruction" />
	public sealed class Dsb32 : ARMv6Instruction
	{
		public override int ID { get { return 623; } }

		internal Dsb32()
			: base(1, 3)
		{
		}
	}
}
