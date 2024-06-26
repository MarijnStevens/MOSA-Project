﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework;

public sealed class Transform
{
	#region Properties

	public MethodCompiler MethodCompiler { get; private set; }

	public Compiler Compiler { get; private set; }

	public TypeSystem TypeSystem { get; private set; }

	public TraceLog TraceLog { get; private set; }

	public TraceLog SpecialTraceLog { get; private set; }

	public VirtualRegisters VirtualRegisters { get; private set; }

	public PhysicalRegisters PhysicalRegisters { get; private set; }

	public LocalStack LocalStack { get; set; }

	public BasicBlocks BasicBlocks { get; set; }

	public TransformStageOption Options { get; private set; }

	public bool AreCPURegistersAllocated { get; private set; }

	public bool Is32BitPlatform { get; private set; }

	public int Window { get; private set; }

	public bool Devirtualization { get; private set; }

	public BaseMethodCompilerStage Stage { get; private set; }

	public int TransformCount { get; private set; }

	public bool IsInSSAForm { get; private set; }

	public bool IsLowerTo32 => Options.HasFlag(TransformStageOption.LowerTo32);

	public bool IsLowerCodeSize => Options.HasFlag(TransformStageOption.ReduceCodeSize);

	public bool IsSSAEnabled { get; private set; } = false;

	public bool IsTraceTransforms = false;

	#endregion Properties

	#region Properties - Indirect

	public uint NativePointerSize => Compiler.Architecture.NativePointerSize;

	public BaseArchitecture Architecture => Compiler.Architecture;

	public MosaMethod Method => MethodCompiler.Method;

	public MosaTypeLayout TypeLayout => MethodCompiler.TypeLayout;

	public MosaLinker Linker => Compiler.Linker;

	public MethodScanner MethodScanner => MethodCompiler.MethodScanner;

	public Operand ConstantZero => MethodCompiler.ConstantZero;

	#endregion Properties - Indirect

	#region Properties - Registers

	public Operand StackFrame => MethodCompiler.StackFrame;

	public Operand StackPointer => MethodCompiler.StackPointer;

	/// <summary>
	/// Gets the link register.
	/// </summary>
	public Operand LinkRegister => MethodCompiler.LinkRegister;

	/// <summary>
	/// Gets the program counter
	/// </summary>
	public Operand ProgramCounter => MethodCompiler.ProgramCounter;

	/// <summary>
	/// Gets the exception register.
	/// </summary>
	public Operand ExceptionRegister => MethodCompiler.ExceptionRegister;

	/// <summary>
	/// Gets the leave target register.
	/// </summary>
	public Operand LeaveTargetRegister => MethodCompiler.LeaveTargetRegister;

	#endregion Properties - Registers

	#region Properties - Instructions

	public BaseInstruction LoadInstruction { get; private set; }

	public BaseInstruction StoreInstruction { get; private set; }

	public BaseInstruction MoveInstruction { get; private set; }

	public BaseInstruction AddInstruction { get; private set; }

	public BaseInstruction SubInstruction { get; private set; }

	public BaseInstruction MulSignedInstruction { get; private set; }

	public BaseInstruction MulUnsignedInstruction { get; private set; }

	public BaseInstruction BranchInstruction { get; private set; }

	#endregion Properties - Instructions

	#region Data

	private readonly Dictionary<Type, BaseTransformManager> Managers = new();

	#endregion Data

	#region Set Contexts

	public void SetCompiler(Compiler compiler)
	{
		Compiler = compiler;

		Is32BitPlatform = Compiler.Architecture.Is32BitPlatform;
		TypeSystem = Compiler.TypeSystem;

		Devirtualization = Compiler.MosaSettings.Devirtualization;
		Window = Compiler.MosaSettings.OptimizationScanWindow;

		LoadInstruction = Is32BitPlatform ? IR.Load32 : IR.Load64;
		StoreInstruction = Is32BitPlatform ? IR.Store32 : IR.Store64;
		MoveInstruction = Is32BitPlatform ? IR.Move32 : IR.Move64;
		AddInstruction = Is32BitPlatform ? IR.Add32 : IR.Add64;
		SubInstruction = Is32BitPlatform ? IR.Sub32 : IR.Sub64;
		MulSignedInstruction = Is32BitPlatform ? IR.MulSigned32 : IR.MulSigned64;
		MulUnsignedInstruction = Is32BitPlatform ? IR.MulUnsigned32 : IR.MulUnsigned64;
		BranchInstruction = Is32BitPlatform ? IR.Branch32 : IR.Branch64;

		IsSSAEnabled = Compiler.MosaSettings.SSA;

		Options = TransformStageOption.None;

		TraceLog = null;
		Managers.Clear();

		// clear - just in case
		MethodCompiler = null;
		VirtualRegisters = null;
		PhysicalRegisters = null;
		LocalStack = null;
		BasicBlocks = null;
	}

	public void SetMethodCompiler(MethodCompiler methodCompiler)
	{
		MethodCompiler = methodCompiler;
		VirtualRegisters = methodCompiler.VirtualRegisters;
		PhysicalRegisters = methodCompiler.PhysicalRegisters;
		LocalStack = methodCompiler.LocalStack;
		BasicBlocks = methodCompiler.BasicBlocks;
		AreCPURegistersAllocated = methodCompiler.AreCPURegistersAllocated;

		Options = TransformStageOption.None;

		IsTraceTransforms = methodCompiler.IsTraceTransforms;

		TraceLog = null;
		Managers.Clear();

		Stage = null;
	}

	public void SetStage(BaseMethodCompilerStage stage)
	{
		Stage = stage;
		TransformCount = 0;
		IsInSSAForm = MethodCompiler.IsInSSAForm;
		Managers.Clear();
	}

	#endregion Set Contexts

	public void SetStageOption(TransformStageOption option)
	{
		Options = option | option;
	}

	#region Manager

	public T GetManager<T>() where T : class
	{
		Managers.TryGetValue(typeof(T), out var manager);

		return manager as T;
	}

	public void AddManager(BaseTransformManager transformManager)
	{
		Managers.Add(transformManager.GetType(), transformManager);
	}

	#endregion Manager

	#region Logs

	public void ClearLogs()
	{
		TraceLog = null;
		SpecialTraceLog = null;
	}

	public void SetLog(TraceLog traceLog)
	{
		TraceLog = traceLog;
	}

	public void SetLogs(TraceLog traceLog = null, TraceLog specialTraceLog = null)
	{
		TraceLog = traceLog;
		SpecialTraceLog = specialTraceLog;
	}

	#endregion Logs

	public bool ApplyTransform(Context context, BaseTransform transform)
	{
		if (!transform.Match(context, this))
			return false;

		TraceBefore(context, transform);

		transform.Transform(context, this);

		TraceAfter(context);

		return true;
	}

	#region Trace

	private void TraceBefore(Context context, BaseTransform transformation)
	{
		TransformCount++;

		if (!IsTraceTransforms)
			return;

		TraceLog?.Log($"{TransformCount,-7}\t| {transformation.Name}");

		if (transformation.Log)
			SpecialTraceLog?.Log($"{transformation.Name}\t{Method.FullName} at {context}");

		TraceLog?.Log($"{context.Block}\t| {context}");
	}

	private void TraceAfter(Context context)
	{
		if (!IsTraceTransforms)
			return;

		TraceInstructions();

		TraceLog?.Log($"       \t| {context}");
		TraceLog?.Log();
	}

	public void TraceBefore(BaseBlockTransform transformation, BasicBlock block)
	{
		TransformCount++;

		if (!IsTraceTransforms)
			return;

		TraceLog?.Log($"{TransformCount,-7}\t| {transformation.Name}");

		if (transformation.Log)
			SpecialTraceLog?.Log($"{transformation.Name}\t{Method.FullName}");

		TraceLog?.Log($"{block}\t| {transformation.Name}");
	}

	public void TraceAfter(BaseBlockTransform transformation)
	{
		if (!IsTraceTransforms)
			return;

		TraceInstructions();

		TraceLog?.Log($"       \t| {transformation.Name}");
		TraceLog?.Log();
	}

	public void TraceInstructions()
	{
		if (!IsTraceTransforms)
			return;

		MethodCompiler.CreateTranformInstructionTrace(Stage, TransformCount);
	}

	#endregion Trace

	#region Basic Block Helpers

	/// <summary>
	/// Splits the block.
	/// </summary>
	/// <param name="context">The context.</param>
	/// <returns></returns>
	public Context Split(Context context)
	{
		return new Context(Split(context.Node));
	}

	/// <summary>
	/// Splits the block.
	/// </summary>
	/// <param name="node">The node.</param>
	/// <returns></returns>
	public BasicBlock Split(Node node)
	{
		var newblock = CreateNewBlock(-1, node.Label);

		node.Split(newblock);

		return newblock;
	}

	/// <summary>
	/// Creates the new block.
	/// </summary>
	/// <param name="blockLabel">The label.</param>
	/// <param name="instructionLabel">The instruction label.</param>
	/// <returns></returns>
	private BasicBlock CreateNewBlock(int blockLabel, int instructionLabel)
	{
		return BasicBlocks.CreateBlock(blockLabel, instructionLabel);
	}

	/// <summary>
	/// Creates empty blocks.
	/// </summary>
	/// <param name="blocks">The Blocks.</param>
	/// <param name="instructionLabel">The instruction label.</param>
	/// <returns></returns>
	public Context[] CreateNewBlockContexts(int blocks, int instructionLabel)
	{
		// Allocate the context array
		var result = new Context[blocks];

		for (var index = 0; index < blocks; index++)
		{
			result[index] = CreateNewBlockContext(instructionLabel);
		}

		return result;
	}

	/// <summary>
	/// Create an empty block.
	/// </summary>
	/// <param name="instructionLabel">The instruction label.</param>
	/// <returns></returns>
	private Context CreateNewBlockContext(int instructionLabel)
	{
		return new Context(CreateNewBlock(-1, instructionLabel));
	}

	#endregion Basic Block Helpers

	#region Phi Helpers

	public static void UpdatePhiTarget(BasicBlock target, BasicBlock source, BasicBlock newSource)
	{
		PhiHelper.UpdatePhiTarget(target, source, newSource);
	}

	public static void UpdatePhiTargets(List<BasicBlock> targets, BasicBlock source, BasicBlock newSource)
	{
		PhiHelper.UpdatePhiTargets(targets, source, newSource);
	}

	public static void UpdatePhiBlocks(List<BasicBlock> phiBlocks)
	{
		PhiHelper.UpdatePhiBlocks(phiBlocks);
	}

	public static void UpdatePhiBlock(BasicBlock phiBlock)
	{
		PhiHelper.UpdatePhiBlock(phiBlock);
	}

	public static void UpdatePhi(Node node)
	{
		PhiHelper.UpdatePhi(node);
	}

	#endregion Phi Helpers

	#region Move Helpers

	public void MoveOperand1ToVirtualRegister(Context context, BaseInstruction moveInstruction)
	{
		Debug.Assert(!context.Operand1.IsVirtualRegister);

		var operand1 = context.Operand1;

		var v1 = VirtualRegisters.Allocate(operand1);

		context.InsertBefore().AppendInstruction(moveInstruction, v1, operand1);
		context.Operand1 = v1;
	}

	public void MoveOperand2ToVirtualRegister(Context context, BaseInstruction moveInstruction)
	{
		Debug.Assert(!context.Operand2.IsVirtualRegister);

		var operand2 = context.Operand2;

		var v1 = VirtualRegisters.Allocate(operand2);

		context.InsertBefore().AppendInstruction(moveInstruction, v1, operand2);
		context.Operand2 = v1;
	}

	public void MoveOperand1And2ToVirtualRegisters(Context context, BaseInstruction moveInstruction)
	{
		Debug.Assert(!context.Operand1.IsVirtualRegister || !context.Operand2.IsVirtualRegister);

		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		if (operand1.IsConstant && operand2.IsConstant && operand1.ConstantUnsigned64 == operand2.ConstantUnsigned64)
		{
			var v1 = VirtualRegisters.Allocate(operand1);

			context.InsertBefore().AppendInstruction(moveInstruction, v1, operand1);
			context.Operand1 = v1;
			context.Operand2 = v1;
			return;
		}
		else if (operand1.IsConstant && operand2.IsConstant)
		{
			var v1 = VirtualRegisters.Allocate(operand1);
			var v2 = VirtualRegisters.Allocate(operand2);

			context.InsertBefore().AppendInstruction(moveInstruction, v1, operand1);
			context.InsertBefore().AppendInstruction(moveInstruction, v2, operand2);
			context.Operand1 = v1;
			context.Operand2 = v2;
			return;
		}
		else if (operand1.IsConstant)
		{
			var v1 = VirtualRegisters.Allocate(operand1);

			context.InsertBefore().AppendInstruction(moveInstruction, v1, operand1);
			context.Operand1 = v1;
		}
		else if (operand2.IsConstant)
		{
			var v1 = VirtualRegisters.Allocate(operand2);

			context.InsertBefore().AppendInstruction(moveInstruction, v1, operand2);
			context.Operand2 = v1;
		}
	}

	#endregion Move Helpers

	#region Linker Helpers

	public Operand CreateR4Label(float value)
	{
		var symbol = Linker.GetConstantSymbol(value);

		var label = Operand.CreateLabelR4(symbol.Name);

		return label;
	}

	public Operand CreateR8Label(double value)
	{
		var symbol = Linker.GetConstantSymbol(value);

		var label = Operand.CreateLabelR8(symbol.Name);

		return label;
	}

	public Operand CreateFloatingPointLabel(Operand operand)
	{
		var symbol = operand.IsR4
			? Linker.GetConstantSymbol(operand.ConstantFloat)
			: Compiler.Linker.GetConstantSymbol(operand.ConstantDouble);

		var label = operand.IsR4
			? Operand.CreateLabelR4(symbol.Name)
			: Operand.CreateLabelR8(symbol.Name);

		return label;
	}

	#endregion Linker Helpers

	#region Floating Point Helpers

	public Operand LoadValueR4(Context context, float value, BaseInstruction loadInstruction)
	{
		var label = CreateR4Label(value);

		var v1 = VirtualRegisters.AllocateR4();

		context.InsertBefore().SetInstruction(loadInstruction, v1, label, Operand.Constant32_0);

		return v1;
	}

	public Operand LoadValueR8(Context context, double value, BaseInstruction loadInstruction)
	{
		var label = CreateR8Label(value);

		var v1 = VirtualRegisters.AllocateR8();

		context.InsertBefore().SetInstruction(loadInstruction, v1, label, Operand.Constant32_0);

		return v1;
	}

	public Operand MoveConstantToFloatRegister(Context context, Operand operand, BaseInstruction instructionR4, BaseInstruction instructionR8)
	{
		if (!operand.IsConstant)
			return operand;

		var label = CreateFloatingPointLabel(operand);

		var v1 = operand.IsR4 ? VirtualRegisters.AllocateR4() : VirtualRegisters.AllocateR8();

		var instruction = operand.IsR4 ? instructionR4 : instructionR8;

		context.InsertBefore().SetInstruction(instruction, v1, label, Operand.Constant32_0);

		return v1;
	}

	#endregion Floating Point Helpers

	public void SplitOperand(Operand operand, out Operand operandLow, out Operand operandHigh)
	{
		MethodCompiler.SplitOperand(operand, out operandLow, out operandHigh);
	}

	public (Operand Low, Operand High) SplitOperand(Operand operand)
	{
		MethodCompiler.SplitOperand(operand, out var low, out var high);

		return (low, high);
	}

	public MosaMethod GetMethod(string fullName, string methodName)
	{
		var type = TypeSystem.GetType(fullName);

		if (type == null)
			return null;

		var method = type.FindMethodByName(methodName);

		return method;
	}

	public void ReplaceWithCall(Context context, string fullName, string methodName)
	{
		var method = GetMethod(fullName, methodName);

		Debug.Assert(method != null, $"Cannot find method: {methodName}");

		// FUTURE: throw compiler exception

		var symbol = Operand.CreateLabel(method, Is32BitPlatform);

		if (context.OperandCount == 1)
		{
			context.SetInstruction(IR.CallStatic, context.Result, symbol, context.Operand1);
		}
		else if (context.OperandCount == 2)
		{
			context.SetInstruction(IR.CallStatic, context.Result, symbol, context.Operand1, context.Operand2);
		}
		else
		{
			// FUTURE: throw compiler exception
		}

		MethodScanner.MethodInvoked(method, Method);
	}

	public static void MoveConstantRight(Context context) // static
	{
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		if (operand2.IsConstant || operand1.IsVirtualRegister)
			return;

		// Move constant to the right
		context.Operand1 = operand2;
		context.Operand2 = operand1;
		context.ConditionCode = context.ConditionCode.GetReverse();
	}

	public void OrderLoadStoreOperands(Context context)  // FUTURE: Rename
	{
		if (context.Operand1.IsResolvedConstant && context.Operand2.IsResolvedConstant)
		{
			context.Operand1 = Operand.CreateConstant(context.Operand1.ConstantUnsigned64 + context.Operand2.ConstantUnsigned64);
			context.Operand2 = Operand.Constant32_0;
		}

		if (context.Operand1.IsConstant && !context.Operand2.IsConstant)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			context.Operand2 = operand1;
			context.Operand1 = operand2;
		}
	}
}
