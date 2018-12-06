// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// SetReturnR4
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class SetReturnR4 : BaseIRInstruction
	{
		public override int ID { get { return 136; } }

		public SetReturnR4()
			: base(1, 0)
		{
		}

		public override FlowControl FlowControl { get { return FlowControl.Return; } }
	}
}
