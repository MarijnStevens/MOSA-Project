// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR
{
	/// <summary>
	/// Store32
	/// </summary>
	public sealed class Store32 : BaseTransform
	{
		public Store32() : base(IRInstruction.Store32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.OrderLoadStoreOperands(context);

			context.SetInstruction(X64.MovStore32, null, context.Operand1, context.Operand2, context.Operand3);
		}
	}
}