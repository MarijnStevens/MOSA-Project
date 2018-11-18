// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Mrs32 - Status register access
	/// </summary>
	/// <seealso cref="Mosa.Platform.ARMv6.ARMv6Instruction" />
	public sealed class Mrs32 : ARMv6Instruction
	{
		public override int ID { get { return 637; } }

		internal Mrs32()
			: base(1, 3)
		{
		}
	}
}
