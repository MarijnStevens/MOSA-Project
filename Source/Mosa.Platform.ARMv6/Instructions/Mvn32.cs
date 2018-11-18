// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Common;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Mvn32 - Bitwise NOT
	/// </summary>
	/// <seealso cref="Mosa.Platform.ARMv6.ARMv6Instruction" />
	public sealed class Mvn32 : ARMv6Instruction
	{
		public override int ID { get { return 640; } }

		internal Mvn32()
			: base(1, 3)
		{
		}

		protected override void Emit(InstructionNode node, ARMv6CodeEmitter emitter)
		{
			EmitDataProcessingInstruction(node, emitter, Bits.b1111);
		}
	}
}
