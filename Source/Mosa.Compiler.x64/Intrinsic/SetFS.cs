﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::SetFS")]
	private static void SetFS(Context context, Transform transform)
	{
		context.SetInstruction(X64.MovStoreSeg64, transform.PhysicalRegisters.Allocate64(CPURegister.FS), context.Operand1);
	}
}
