// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction;

public sealed class MulSigned64ByZero : BaseTransform
{
	public MulSigned64ByZero() : base(IR.MulSigned64, TransformType.Auto | TransformType.Optimization, 80)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsConstantZero)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var e1 = Operand.Constant64_0;

		context.SetInstruction(IR.Move64, result, e1);
	}
}

public sealed class MulSigned64ByZero_v1 : BaseTransform
{
	public MulSigned64ByZero_v1() : base(IR.MulSigned64, TransformType.Auto | TransformType.Optimization, 80)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsConstantZero)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var e1 = Operand.Constant64_0;

		context.SetInstruction(IR.Move64, result, e1);
	}
}
