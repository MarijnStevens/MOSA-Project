// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Instructions
{
	/// <summary>
	/// Add32 - Addition
	/// </summary>
	/// <seealso cref="Mosa.Platform.ARMv8A32.ARMv8A32Instruction" />
	public sealed class Add32 : ARMv8A32Instruction
	{
		public override int ID { get { return 577; } }

		internal Add32()
			: base(1, 4)
		{
		}

		public override bool IsCommutative { get { return true; } }

		public override bool IsCarryFlagUsed { get { return true; } }

		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 4);

			if (node.Operand2.IsCPURegister)
			{
				emitter.OpcodeEncoder.AppendNibble(GetConditionCode(node.ConditionCode));
				emitter.OpcodeEncoder.Append2Bits(0b00);
				emitter.OpcodeEncoder.AppendBit(0b0);
				emitter.OpcodeEncoder.AppendNibble(0b0100);
				emitter.OpcodeEncoder.AppendBit(0b0);
				emitter.OpcodeEncoder.Append4Bits(node.Operand1.Register.RegisterCode);
				emitter.OpcodeEncoder.Append4Bits(node.Result.Register.RegisterCode);
				emitter.OpcodeEncoder.AppendNibble(0b0000);
				emitter.OpcodeEncoder.AppendBit(0b0);
				emitter.OpcodeEncoder.Append2Bits(0b00);
				emitter.OpcodeEncoder.AppendBit(0b0);
				emitter.OpcodeEncoder.Append4Bits(node.Operand2.Register.RegisterCode);
				return;
			}

			throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
		}
	}
}
