// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// StoreManagedPointer
/// </summary>
[Transform("x64.IR")]
public sealed class StoreManagedPointer : BaseIRTransform
{
	public StoreManagedPointer() : base(IRInstruction.StoreManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.OrderLoadStoreOperands(context);

		context.SetInstruction(X64.MovStore32, null, context.Operand1, context.Operand2, context.Operand3);
	}
}