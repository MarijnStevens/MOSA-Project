﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Simplification;

public sealed class IfThenElse32Same : BaseTransform
{
	public IfThenElse32Same() : base(IR.IfThenElse32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return AreSame(context.Operand2, context.Operand3);
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(IR.Move32, context.Result, context.Operand1);
	}
}
