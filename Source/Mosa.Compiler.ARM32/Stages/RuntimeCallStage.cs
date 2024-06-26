// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.ARM32.Transforms.RuntimeCall;

namespace Mosa.Compiler.ARM32.Stages;

/// <summary>
/// Runtime Call Transformation Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
public sealed class RuntimeCallStage : Compiler.Framework.Stages.BaseTransformStage
{
	public override string Name => "ARM32." + GetType().Name;

	public RuntimeCallStage()
		: base(1)
	{
		AddTranforms(RuntimeCallTransforms.List);
	}
}
