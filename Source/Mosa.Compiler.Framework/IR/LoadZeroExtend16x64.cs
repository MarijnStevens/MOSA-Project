// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// LoadZeroExtend16x64
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class LoadZeroExtend16x64 : BaseIRInstruction
	{
		public override int ID { get { return 77; } }

		public LoadZeroExtend16x64()
			: base(2, 1)
		{
		}

		public override bool IsMemoryRead { get { return true; } }
	}
}
