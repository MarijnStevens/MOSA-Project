﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.ConstantMove;

public sealed class Compare64x64 : BaseTransform
{
	public Compare64x64() : base(IR.Compare64x64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (IsConstant(context.Operand2))
			return false;

		if (!IsConstant(context.Operand1))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(IR.Compare64x64, context.ConditionCode.GetReverse(), context.Result, context.Operand2, context.Operand1);
	}
}
