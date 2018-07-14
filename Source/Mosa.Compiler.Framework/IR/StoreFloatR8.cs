// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// StoreFloatR8
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class StoreFloatR8 : BaseIRInstruction
	{
		public override int ID { get { return 146; } }

		public StoreFloatR8()
			: base(3, 0)
		{
		}

		public override bool IsMemoryWrite { get { return true; } }
	}
}
