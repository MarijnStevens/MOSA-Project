// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Instructions;

/// <summary>
/// IsInstanceOfType
/// </summary>
public sealed class IsInstanceOfType : BaseIRInstruction
{
	public IsInstanceOfType()
		: base(2, 1)
	{
	}

	public override bool IsMemoryRead => true;
}