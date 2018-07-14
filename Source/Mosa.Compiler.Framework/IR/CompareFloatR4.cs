// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// CompareFloatR4
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class CompareFloatR4 : BaseIRInstruction
	{
		public override int ID { get { return 19; } }

		public CompareFloatR4()
			: base(2, 1)
		{
		}

		public override BuiltInType ResultType { get { return BuiltInType.Boolean; } }
	}
}
