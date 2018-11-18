// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Instructions
{
	/// <summary>
	/// XChg32
	/// </summary>
	/// <seealso cref="Mosa.Platform.x64.X64Instruction" />
	public sealed class XChg32 : X64Instruction
	{
		public override int ID { get { return 523; } }

		internal XChg32()
			: base(2, 2)
		{
		}

		public override bool IsCommutative { get { return true; } }
	}
}
