// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Instructions;

/// <summary>
/// LoadZeroExtend8x64
/// </summary>
public sealed class LoadZeroExtend8x64 : BaseIRInstruction
{
	public LoadZeroExtend8x64()
		: base(2, 1)
	{
	}

	public override bool IsMemoryRead => true;
}
