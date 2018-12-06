// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// TryStart
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class TryStart : BaseIRInstruction
	{
		public override int ID { get { return 173; } }

		public TryStart()
			: base(0, 0)
		{
		}

		public override bool IgnoreDuringCodeGeneration { get { return true; } }

		public override bool IgnoreInstructionBasicBlockTargets { get { return true; } }
	}
}
