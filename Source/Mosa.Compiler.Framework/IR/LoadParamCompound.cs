// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// LoadParamCompound
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class LoadParamCompound : BaseIRInstruction
	{
		public override int ID { get { return 76; } }

		public LoadParamCompound()
			: base(1, 1)
		{
		}

		public override bool IsMemoryRead { get { return true; } }
	}
}
