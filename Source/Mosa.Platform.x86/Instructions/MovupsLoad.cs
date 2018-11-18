// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// MovupsLoad
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.X86Instruction" />
	public sealed class MovupsLoad : X86Instruction
	{
		public override int ID { get { return 261; } }

		internal MovupsLoad()
			: base(1, 2)
		{
		}

		public override bool IsMemoryRead { get { return true; } }

		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == DefaultResultCount);
			System.Diagnostics.Debug.Assert(node.OperandCount == DefaultOperandCount);

			StaticEmitters.EmitMovupsLoad(node, emitter);
		}
	}
}
