// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Instructions
{
	/// <summary>
	/// SetByteIfGreaterOrEqual
	/// </summary>
	/// <seealso cref="Mosa.Platform.x64.X64Instruction" />
	public sealed class SetByteIfGreaterOrEqual : X64Instruction
	{
		public override int ID { get { return 566; } }

		internal SetByteIfGreaterOrEqual()
			: base(1, 0)
		{
		}

		public override string AlternativeName { get { return "SetGE"; } }

		public static readonly LegacyOpCode LegacyOpcode = new LegacyOpCode(new byte[] { 0x0F, 0x9D });

		public override bool IsSignFlagUsed { get { return true; } }

		public override bool IsOverflowFlagUsed { get { return true; } }

		public override BaseInstruction GetOpposite()
		{
			return X64.SetByteIfLessThan;
		}

		internal override void EmitLegacy(InstructionNode node, X64CodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 0);

			emitter.Emit(LegacyOpcode, node.Result);
		}
	}
}
