// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.IR;

/// <summary>
/// SetReturnCompound
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
public sealed class SetReturnCompound : BaseIRInstruction
{
	public SetReturnCompound()
		: base(1, 0)
	{
	}

	public override bool IsFlowNext => false;

	public override bool IsReturn => true;
}
