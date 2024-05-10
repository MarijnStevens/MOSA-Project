// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction;

public sealed class MulUnsigned32ByOne : BaseTransform
{
	public MulUnsigned32ByOne() : base(IR.MulUnsigned32, TransformType.Auto | TransformType.Optimization, 80)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsConstantOne)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;

		context.SetInstruction(IR.Move32, result, t1);
	}
}

public sealed class MulUnsigned32ByOne_v1 : BaseTransform
{
	public MulUnsigned32ByOne_v1() : base(IR.MulUnsigned32, TransformType.Auto | TransformType.Optimization, 80)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsConstantOne)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand2;

		context.SetInstruction(IR.Move32, result, t1);
	}
}
