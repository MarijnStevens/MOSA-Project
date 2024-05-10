// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

public sealed class Compare32x32GreaterOrEqualThanMax : BaseTransform
{
	public Compare32x32GreaterOrEqualThanMax() : base(IR.Compare32x32, TransformType.Auto | TransformType.Optimization, 100)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.UnsignedLessOrEqual)
			return false;

		if (!context.Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.ConstantUnsigned64 != 0xFFFFFFFF)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var c1 = Operand.CreateConstant(1);

		context.SetInstruction(IR.Move32, result, c1);
	}
}

public sealed class Compare32x32GreaterOrEqualThanMax_v1 : BaseTransform
{
	public Compare32x32GreaterOrEqualThanMax_v1() : base(IR.Compare32x32, TransformType.Auto | TransformType.Optimization, 100)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.UnsignedGreaterOrEqual)
			return false;

		if (!context.Operand1.IsResolvedConstant)
			return false;

		if (context.Operand1.ConstantUnsigned64 != 0xFFFFFFFF)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var c1 = Operand.CreateConstant(1);

		context.SetInstruction(IR.Move32, result, c1);
	}
}
