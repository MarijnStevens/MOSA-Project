// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// CompareInt64x32
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class CompareInt64x32 : BaseIRInstruction
	{
		public override int ID { get { return 25; } }

		public CompareInt64x32()
			: base(2, 1)
		{
		}

		public override BuiltInType ResultType { get { return BuiltInType.Boolean; } }
	}
}
