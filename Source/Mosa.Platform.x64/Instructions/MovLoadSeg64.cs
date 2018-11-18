// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Instructions
{
	/// <summary>
	/// MovLoadSeg64
	/// </summary>
	/// <seealso cref="Mosa.Platform.x64.X64Instruction" />
	public sealed class MovLoadSeg64 : X64Instruction
	{
		public override int ID { get { return 429; } }

		internal MovLoadSeg64()
			: base(1, 1)
		{
		}

		public static readonly LegacyOpCode LegacyOpcode = new LegacyOpCode(new byte[] { 0x8C });

		internal override void EmitLegacy(InstructionNode node, X64CodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 1);

			emitter.Emit(LegacyOpcode, node.Operand1, node.Result);
		}
	}
}
