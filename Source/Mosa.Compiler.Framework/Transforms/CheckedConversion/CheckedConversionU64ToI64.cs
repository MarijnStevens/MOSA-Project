// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// CheckedConversionU64ToI64
/// </summary>
public sealed class CheckedConversionU64ToI64 : BaseCheckedConversionTransform
{
	public CheckedConversionU64ToI64() : base(IRInstruction.CheckedConversionU64ToI64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		CallCheckOverflow(transform, context, "U8ToI8");
	}
}