// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Rev32 - Byte-Reverse Word
	/// </summary>
	/// <seealso cref="Mosa.Platform.ARMv6.ARMv6Instruction" />
	public sealed class Rev32 : ARMv6Instruction
	{
		public override int ID { get { return 645; } }

		internal Rev32()
			: base(1, 3)
		{
		}
	}
}
