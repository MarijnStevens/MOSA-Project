// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Instructions
{
	/// <summary>
	/// SetByteIfUnsignedGreaterThan
	/// </summary>
	/// <seealso cref="Mosa.Platform.x64.X64Instruction" />
	public sealed class SetByteIfUnsignedGreaterThan : X64Instruction
	{
		public override int ID { get { return 560; } }

		internal SetByteIfUnsignedGreaterThan()
			: base(1, 0)
		{
		}

		public override string AlternativeName { get { return "SetA"; } }

		public static readonly LegacyOpCode LegacyOpcode = new LegacyOpCode(new byte[] { 0x0F, 0x97 });

		public override bool IsZeroFlagUsed { get { return true; } }

		public override bool IsCarryFlagUsed { get { return true; } }

		public override BaseInstruction GetOpposite()
		{
			return X64.SetByteIfUnsignedLessOrEqual;
		}

		internal override void EmitLegacy(InstructionNode node, X64CodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 0);

			emitter.Emit(LegacyOpcode, node.Result);
		}
	}
}
