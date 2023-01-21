// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding
{
	/// <summary>
	/// AddR8x2
	/// </summary>
	public sealed class AddR8x2 : BaseTransform
	{
		public AddR8x2() : base(IRInstruction.AddR8, TransformType.Auto | TransformType.Optimization)
		{
		}

		public override int Priority => 90;

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.AddR8)
				return false;

			if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand2))
				return false;

			if (!IsResolvedConstant(context.Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2;
			var t3 = context.Operand2;

			var e1 = transform.CreateConstant(AddR8(ToR8(t2), ToR8(t3)));

			context.SetInstruction(IRInstruction.AddR8, result, t1, e1);
		}
	}

	/// <summary>
	/// AddR8x2_v1
	/// </summary>
	public sealed class AddR8x2_v1 : BaseTransform
	{
		public AddR8x2_v1() : base(IRInstruction.AddR8, TransformType.Auto | TransformType.Optimization)
		{
		}

		public override int Priority => 90;

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.AddR8)
				return false;

			if (!IsResolvedConstant(context.Operand2.Definitions[0].Operand2))
				return false;

			if (!IsResolvedConstant(context.Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2.Definitions[0].Operand1;
			var t3 = context.Operand2.Definitions[0].Operand2;

			var e1 = transform.CreateConstant(AddR8(ToR8(t3), ToR8(t1)));

			context.SetInstruction(IRInstruction.AddR8, result, t2, e1);
		}
	}

	/// <summary>
	/// AddR8x2_v2
	/// </summary>
	public sealed class AddR8x2_v2 : BaseTransform
	{
		public AddR8x2_v2() : base(IRInstruction.AddR8, TransformType.Auto | TransformType.Optimization)
		{
		}

		public override int Priority => 90;

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.AddR8)
				return false;

			if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand1))
				return false;

			if (!IsResolvedConstant(context.Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2;
			var t3 = context.Operand2;

			var e1 = transform.CreateConstant(AddR8(ToR8(t1), ToR8(t3)));

			context.SetInstruction(IRInstruction.AddR8, result, t2, e1);
		}
	}

	/// <summary>
	/// AddR8x2_v3
	/// </summary>
	public sealed class AddR8x2_v3 : BaseTransform
	{
		public AddR8x2_v3() : base(IRInstruction.AddR8, TransformType.Auto | TransformType.Optimization)
		{
		}

		public override int Priority => 90;

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.AddR8)
				return false;

			if (!IsResolvedConstant(context.Operand2.Definitions[0].Operand1))
				return false;

			if (!IsResolvedConstant(context.Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2.Definitions[0].Operand1;
			var t3 = context.Operand2.Definitions[0].Operand2;

			var e1 = transform.CreateConstant(AddR8(ToR8(t2), ToR8(t1)));

			context.SetInstruction(IRInstruction.AddR8, result, t3, e1);
		}
	}
}