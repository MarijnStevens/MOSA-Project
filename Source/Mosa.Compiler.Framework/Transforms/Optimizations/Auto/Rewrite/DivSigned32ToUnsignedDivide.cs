// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Rewrite;

public sealed class DivSigned32ToUnsignedDivide : BaseTransform
{
	public DivSigned32ToUnsignedDivide() : base(IR.DivSigned32, TransformType.Auto | TransformType.Optimization, 50)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!IsResolvedConstant(context.Operand2))
			return false;

		if (!IsLessThanZero(To32(context.Operand2)))
			return false;

		if (IsUnsignedMax32(To32(context.Operand2)))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2;

		var v1 = transform.VirtualRegisters.Allocate32();

		var e1 = Operand.CreateConstant(Neg32(To32(t2)));

		context.SetInstruction(IR.DivUnsigned32, v1, t1, e1);
		context.AppendInstruction(IR.Neg32, result, v1);
	}
}
