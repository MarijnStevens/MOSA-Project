﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// This stage inserts the IR ExceptionStart instructions at the beginning of each exception block and
	/// IR Flow instructions when necessary after leave instructions
	/// </summary>
	public class ExceptionPrologueStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			InsertExceptionStartInstructions();
			InsertFlowOrJumpInstructions();
		}

		private void InsertExceptionStartInstructions()
		{
			foreach (var clause in MethodCompiler.Method.ExceptionHandlers)
			{
				if (clause.HandlerType == ExceptionHandlerType.Exception)
				{
					var tryHandler = BasicBlocks.GetByLabel(clause.HandlerStart);

					var exceptionObject = MethodCompiler.CreateVirtualRegister(clause.Type);

					var context = new Context(tryHandler);

					context.AppendInstruction(IRInstruction.ExceptionStart, exceptionObject);
				}
			}
		}

		private void InsertFlowOrJumpInstructions()
		{
			foreach (var block in BasicBlocks)
			{
				for (var node = block.BeforeLast; !node.IsBlockStartInstruction; node = node.Previous)
				{
					if (node.IsEmpty)
						continue;

					if (!(node.Instruction is CIL.LeaveInstruction))
						continue;

					var target = node.BranchTargets[0];

					if (IsLeaveAndTargetWithinTry(node))
					{
						var test = IsLeaveAndTargetWithinTry(node); // delete me

						node.SetInstruction(IRInstruction.Jmp, target);

						BasicBlocks.RemoveHeaderBlock(target);

						continue;
					}

					var entry = FindImmediateExceptionHandler(node);

					if (entry == null)
						break;

					if (!entry.IsLabelWithinTry(node.Label))
						break;

					var flowNode = new InstructionNode(IRInstruction.Flow, target);

					node.Insert(flowNode);

					if (BasicBlocks.IsHeaderBlock(target))
					{
						BasicBlocks.RemoveHeaderBlock(target);
					}

					break;
				}
			}
		}
	}
}
