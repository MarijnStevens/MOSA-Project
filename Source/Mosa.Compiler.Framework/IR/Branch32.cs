// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Branch32
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class Branch32 : BaseIRInstruction
	{
		public Branch32()
			: base(0, 2)
		{
		}

		public override FlowControl FlowControl { get { return FlowControl.ConditionalBranch; } }
	}
}