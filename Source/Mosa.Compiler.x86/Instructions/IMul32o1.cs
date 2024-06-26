// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Instructions;

/// <summary>
/// IMul32o1
/// </summary>
public sealed class IMul32o1 : X86Instruction
{
	internal IMul32o1()
		: base(2, 2)
	{
	}

	public override bool IsCommutative => true;

	public override bool IsZeroFlagUnchanged => true;

	public override bool IsZeroFlagUndefined => true;

	public override bool IsCarryFlagModified => true;

	public override bool IsSignFlagUnchanged => true;

	public override bool IsSignFlagUndefined => true;

	public override bool IsOverflowFlagModified => true;

	public override bool IsParityFlagUnchanged => true;

	public override bool IsParityFlagUndefined => true;

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 2);
		System.Diagnostics.Debug.Assert(node.OperandCount == 2);
		System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());

		opcodeEncoder.Append8Bits(0xF7);
		opcodeEncoder.Append2Bits(0b11);
		opcodeEncoder.Append3Bits(0b101);
		opcodeEncoder.Append3Bits(node.Operand2.Register.RegisterCode);

		System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
	}
}
