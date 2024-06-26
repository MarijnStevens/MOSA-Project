// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.ConstantFolding;

/// <summary>
/// MulCarryOut64
/// </summary>
public sealed class MulCarryOut64 : BaseTransform
{
	public MulCarryOut64() : base(IR.MulCarryOut64, TransformType.Auto | TransformType.Optimization, 100, true)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!IsResolvedConstant(context.Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		if (IntegerTwiddling.IsMultiplyUnsignedCarry(context.Operand1.ConstantUnsigned64, context.Operand2.ConstantUnsigned64))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var result2 = context.Result2;

		var t1 = context.Operand1;
		var t2 = context.Operand2;

		var e1 = Operand.CreateConstant(MulUnsigned64(To64(t1), To64(t2)));

		context.SetInstruction(IR.Move64, result, e1);
		context.AppendInstruction(IR.Move64, result2, Operand.Constant64_1);
	}
}
