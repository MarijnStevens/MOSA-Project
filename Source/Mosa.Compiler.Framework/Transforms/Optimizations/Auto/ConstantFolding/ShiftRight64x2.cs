// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

public sealed class ShiftRight64x2 : BaseTransform
{
	public ShiftRight64x2() : base(IR.ShiftRight64, TransformType.Auto | TransformType.Optimization, 90)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IR.ShiftRight64)
			return false;

		if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand2))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		if (!IsIntegerBetween0And64(context.Operand1.Definitions[0].Operand2))
			return false;

		if (!IsIntegerBetween0And64(context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2;
		var t3 = context.Operand2;

		var e1 = Operand.CreateConstant(Add64(To64(t2), To64(t3)));

		context.SetInstruction(IR.ShiftRight64, result, t1, e1);
	}
}
