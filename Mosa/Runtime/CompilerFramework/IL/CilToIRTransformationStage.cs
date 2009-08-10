﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Mosa.Runtime.CompilerFramework.IR;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Transforms CIL instructions into their appropriate IR.
    /// </summary>
    /// <remarks>
    /// This transformation stage transforms CIL instructions into their equivalent IR sequences.
    /// </remarks>
    public sealed class CilToIrTransformationStage : 
        CodeTransformationStage,
        IILVisitor<CodeTransformationStage.Context>
    {
        #region IMethodCompilerStage Members

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public override string Name
        {
            get { return @"CilToIrTransformationStage"; }
        }
		
        /// <summary>
        /// Adds this stage to the given pipeline.
        /// </summary>
        /// <param name="pipeline">The pipeline to add this stage to.</param>
		public override void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
		}

        #endregion // IMethodCompilerStage Members

        #region IILVisitor<Context> Members

        void IILVisitor<Context>.Nop(NopInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Break(BreakInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Ldarg(LdargInstruction instruction, Context ctx)
        {
            ProcessLoadInstruction(instruction, ctx);
        }

        void IILVisitor<Context>.Ldarga(LdargaInstruction instruction, Context ctx)
        {
            Replace(ctx, new AddressOfInstruction(instruction.Results[0], instruction.Operands[0]));
        }

        void IILVisitor<Context>.Ldloc(LdlocInstruction instruction, Context ctx)
        {
            ProcessLoadInstruction(instruction, ctx);
        }

        void IILVisitor<Context>.Ldloca(LdlocaInstruction instruction, Context ctx)
        {
            Replace(ctx, new AddressOfInstruction(instruction.Results[0], instruction.Operands[0]));
        }

        void IILVisitor<Context>.Ldc(LdcInstruction instruction, Context ctx)
        {
            ProcessLoadInstruction(instruction, ctx);
        }

        void IILVisitor<Context>.Ldobj(LdobjInstruction instruction, Context ctx)
        {
            // This is actually ldind.* and ldobj - the opcodes have the same meanings
            Replace(ctx, new IR.LoadInstruction(instruction.Results[0], instruction.Operands[0]));
        }

        void IILVisitor<Context>.Ldstr(LdstrInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Ldfld(LdfldInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Ldflda(LdfldaInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Ldsfld(LdsfldInstruction instruction, Context ctx)
        {
            RuntimeField field = instruction.Field;
            Replace(ctx, new MoveInstruction(instruction.Results[0], new MemberOperand(field)));
        }

        void IILVisitor<Context>.Ldsflda(LdsfldaInstruction instruction, Context ctx)
        {
            Replace(ctx, new AddressOfInstruction(instruction.Results[0], instruction.Operands[0]));
        }

        void IILVisitor<Context>.Ldftn(LdftnInstruction instruction, Context ctx)
        {
            ReplaceWithInternalCall(ctx, instruction, VmCall.GetFunctionPtr);
        }

        void IILVisitor<Context>.Ldvirtftn(LdvirtftnInstruction instruction, Context ctx)
        {
            ReplaceWithInternalCall(ctx, instruction, VmCall.GetVirtualFunctionPtr);
        }

        void IILVisitor<Context>.Ldtoken(LdtokenInstruction instruction, Context ctx)
        {
            ReplaceWithInternalCall(ctx, instruction, VmCall.GetHandleForToken);
        }

        void IILVisitor<Context>.Stloc(StlocInstruction instruction, Context ctx)
        {
            ProcessStoreInstruction(instruction, ctx);
        }

        void IILVisitor<Context>.Starg(StargInstruction instruction, Context ctx)
        {
            ProcessStoreInstruction(instruction, ctx);
        }

        void IILVisitor<Context>.Stobj(StobjInstruction instruction, Context ctx)
        {
            // This is actually stind.* and stobj - the opcodes have the same meanings
            Replace(ctx, new StoreInstruction(instruction.Operands[0], instruction.Operands[1]));
        }

        void IILVisitor<Context>.Stfld(StfldInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Stsfld(StsfldInstruction instruction, Context ctx)
        {
            RuntimeField field = instruction.Field;
            Replace(ctx, new MoveInstruction(new MemberOperand(field), instruction.Operands[0]));
        }

        void IILVisitor<Context>.Dup(DupInstruction instruction, Context ctx)
        {
            // We don't need the dup anymore.
            Remove(ctx);
        }

        void IILVisitor<Context>.Pop(PopInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Jmp(JumpInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Call(CallInstruction instruction, Context ctx)
        {
            ProcessRedirectableInvokeInstruction(instruction, ctx);
        }

        void IILVisitor<Context>.Calli(CalliInstruction instruction, Context ctx)
        {
            ProcessInvokeInstruction(instruction, ctx);
        }

        void IILVisitor<Context>.Ret(ReturnInstruction instruction, Context ctx)
        {
            if (1 == instruction.Operands.Length)
                Replace(ctx, new IR.ReturnInstruction(instruction.Operands[0]));
            else
                Replace(ctx, new IR.ReturnInstruction());
        }

        void IILVisitor<Context>.Branch(BranchInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.UnaryBranch(UnaryBranchInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.BinaryBranch(BinaryBranchInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Switch(SwitchInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.BinaryLogic(BinaryLogicInstruction instruction, Context ctx)
        {
            Type type;
            switch (instruction.Code)
            {
                case OpCode.And:
                    type = typeof(LogicalAndInstruction);
                    break;

                case OpCode.Or:
                    type = typeof(LogicalOrInstruction);
                    break;

                case OpCode.Xor:
                    type = typeof(LogicalXorInstruction);
                    break;

                case OpCode.Div_un:
                    type = typeof(UDivInstruction);
                    break;

                case OpCode.Rem_un:
                    type = typeof(URemInstruction);
                    break;

                default:
                    throw new NotSupportedException();
            }

            Instruction result = Architecture.CreateInstruction(type, instruction.Results[0], instruction.First, instruction.Second);
            Replace(ctx, result);
        }

        void IILVisitor<Context>.Shift(ShiftInstruction instruction, Context ctx)
        {
            Type replType;
            switch (instruction.Code)
            {
                case OpCode.Shl:
                    replType = typeof(ShiftLeftInstruction);
                    break;

                case OpCode.Shr:
                    replType = typeof(ArithmeticShiftRightInstruction);
                    break;

                case OpCode.Shr_un:
                    replType = typeof(ShiftRightInstruction);
                    break;

                default:
                    throw new NotSupportedException();
            }

            Instruction result = Architecture.CreateInstruction(replType, instruction.Results[0], instruction.First, instruction.Second);
            Replace(ctx, result);
        }

        void IILVisitor<Context>.Neg(NegInstruction instruction, Context ctx)
        {
            Replace(ctx, new[] {
                Architecture.CreateInstruction(typeof(SubInstruction), OpCode.Sub, instruction.Results[0], instruction.Operands[0]) 
            });
        }

        void IILVisitor<Context>.Not(NotInstruction instruction, Context ctx)
        {
            Replace(ctx, Architecture.CreateInstruction(typeof(LogicalNotInstruction), instruction.Results[0], instruction.Operands[0]));
        }

        void IILVisitor<Context>.Conversion(ConversionInstruction instruction, Context ctx)
        {
            ProcessConversionInstruction(instruction, ctx);
        }

        void IILVisitor<Context>.Callvirt(CallvirtInstruction instruction, Context ctx)
        {
            ProcessInvokeInstruction(instruction, ctx);
        }

        void IILVisitor<Context>.Cpobj(CpobjInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Newobj(NewobjInstruction instruction, Context ctx)
        {
            /* FIXME:
             * 
             * - Newobj is actually three things at once:
             *   - Find the type from the token
             *   - Allocate memory for the found type
             *   - Invoke the ctor with the arguments
             * - We're rewriting it exactly as specified above, e.g. first we find
             *   the type handle (similar to ldtoken), we pass that to an internal
             *   call to allocate the memory and finally we call the ctor on the
             *   allocated object.
             * - We don't have to check the allocation result, if it fails, the 
             *   allocator will happily throw the OutOfMemoryException.
             * - We do need to find the right ctor to invoke though.
             * 
             */
        }

        void IILVisitor<Context>.Castclass(CastclassInstruction instruction, Context ctx)
        {
            // We don't need to check the result, if the icall fails, it'll happily throw
            // the InvalidCastException.
            ReplaceWithInternalCall(ctx, instruction, VmCall.Castclass);
        }

        void IILVisitor<Context>.Isinst(IsInstInstruction instruction, Context ctx)
        {
            ReplaceWithInternalCall(ctx, instruction, VmCall.IsInstanceOfType);
        }

        void IILVisitor<Context>.Unbox(UnboxInstruction instruction, Context ctx)
        {
            ReplaceWithInternalCall(ctx, instruction, VmCall.Unbox);
        }

        void IILVisitor<Context>.Throw(ThrowInstruction instruction, Context ctx)
        {
            ReplaceWithInternalCall(ctx, instruction, VmCall.Throw);
        }

        void IILVisitor<Context>.Box(BoxInstruction instruction, Context ctx)
        {
            ReplaceWithInternalCall(ctx, instruction, VmCall.Box);
        }

        void IILVisitor<Context>.Newarr(NewarrInstruction instruction, Context ctx)
        {
            ReplaceWithInternalCall(ctx, instruction, VmCall.Allocate);
        }

        void IILVisitor<Context>.Ldlen(LdlenInstruction instruction, Context ctx)
        {
            // FIXME
        }

        void IILVisitor<Context>.Ldelema(LdelemaInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Ldelem(LdelemInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Stelem(StelemInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.UnboxAny(UnboxAnyInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Refanyval(RefanyvalInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.UnaryArithmetic(UnaryArithmeticInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Mkrefany(MkrefanyInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.ArithmeticOverflow(ArithmeticOverflowInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Endfinally(EndfinallyInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Leave(LeaveInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Arglist(ArglistInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.BinaryComparison(BinaryComparisonInstruction instruction, Context ctx)
        {
            ConditionCode code = GetConditionCode(instruction.Code);
            Operand op1 = instruction.Operands[0];
            if (op1.StackType == StackTypeCode.F)
            {
                Replace(ctx, new FloatingPointCompareInstruction(instruction.Results[0], op1, code, instruction.Operands[1]));
            }
            else
            {
                Replace(ctx, new IntegerCompareInstruction(instruction.Results[0], op1, code, instruction.Operands[1]));
            }
        }

        void IILVisitor<Context>.Localalloc(LocalallocInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Endfilter(EndfilterInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.InitObj(InitObjInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Cpblk(CpblkInstruction instruction, Context ctx)
        {
            ReplaceWithInternalCall(ctx, instruction, VmCall.Memcpy);
        }

        void IILVisitor<Context>.Initblk(InitblkInstruction instruction, Context ctx)
        {
            ReplaceWithInternalCall(ctx, instruction, VmCall.Memset);
        }

        void IILVisitor<Context>.Prefix(PrefixInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Rethrow(RethrowInstruction instruction, Context ctx)
        {
            ReplaceWithInternalCall(ctx, instruction, VmCall.Rethrow);
        }

        void IILVisitor<Context>.Sizeof(SizeofInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Refanytype(RefanytypeInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Add(AddInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Sub(SubInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Mul(MulInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Div(DivInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Rem(RemInstruction instruction, Context ctx)
        {
        }

        #endregion // IILVisitor<Context> Members

        #region Internals

        /// <summary>
        /// Determines if a store is silently truncating the value.
        /// </summary>
        /// <param name="dest">The destination operand.</param>
        /// <param name="source">The source operand.</param>
        /// <returns>True if the store is truncating, otherwise false.</returns>
        private static bool IsTruncating(Operand dest, Operand source)
        {
            CilElementType cetDest = dest.Type.Type;
            CilElementType cetSource = source.Type.Type;

            if (cetDest == CilElementType.I4 || cetDest == CilElementType.U4)
            {
                return (cetSource == CilElementType.I8 || cetSource == CilElementType.U8);
            }
            if (cetDest == CilElementType.I2 || cetDest == CilElementType.U2 || cetDest == CilElementType.Char)
            {
                return (cetSource == CilElementType.I8 || cetSource == CilElementType.U8 || cetSource == CilElementType.I4 || cetSource == CilElementType.U4);
            }
            if (cetDest == CilElementType.I1 || cetDest == CilElementType.U1)
            {
                return (cetSource == CilElementType.I8 || cetSource == CilElementType.U8 || cetSource == CilElementType.I4 || cetSource == CilElementType.U4 || cetSource == CilElementType.I2 || cetSource == CilElementType.U2 || cetSource == CilElementType.U2);
            }
            
            return false;
        }

        /// <summary>
        /// Determines if the load should sign extend the given source operand.
        /// </summary>
        /// <param name="source">The source operand to determine sign extension for.</param>
        /// <returns>True if the given operand should be loaded with its sign extended.</returns>
        private static bool IsSignExtending(Operand source)
        {
            bool result = false;
            CilElementType cet = source.Type.Type;
            if (cet == CilElementType.I1 || cet == CilElementType.I2)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Determines the implicit result type of the load instruction.
        /// </summary>
        /// <param name="sigType">The signature type of the source operand.</param>
        /// <returns>The signature type of the result.</returns>
        /// <remarks>
        /// This method performs the implicit type conversion mandated by the CIL spec
        /// for load instructions.
        /// </remarks>
        private SigType GetImplicitResultSigType(SigType sigType)
        {
            SigType result;

            switch (sigType.Type)
            {
                case CilElementType.I1: goto case CilElementType.I2;
                case CilElementType.I2:
                    result = new SigType(CilElementType.I4);
                    break;

                case CilElementType.U1: goto case CilElementType.U2;
                case CilElementType.U2:
                    result = new SigType(CilElementType.U4);
                    break;

                case CilElementType.Char: goto case CilElementType.U2;
                case CilElementType.Boolean: goto case CilElementType.U2;


                default:
                    result = sigType;
                    break;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        private enum ConvType
        {
            I1 = 0,
            I2 = 1,
            I4 = 2,
            I8 = 3,
            U1 = 4,
            U2 = 5,
            U4 = 6,
            U8 = 7,
            R4 = 8,
            R8 = 9,
            I = 10,
            U = 11,
            Ptr = 12
        }

        /// <summary>
        /// Converts a <see cref="CilElementType"/> to <see cref="ConvType"/>.
        /// </summary>
        /// <param name="cet">The CilElementType to convert.</param>
        /// <returns>The equivalent ConvType.</returns>
        /// <exception cref="T:System.NotSupportedException"><paramref name="cet"/> can't be converted.</exception>
        private ConvType ConvTypeFromCilType(CilElementType cet)
        {
            switch (cet)
            {
                case CilElementType.Char: return ConvType.U2;
                case CilElementType.I1: return ConvType.I1;
                case CilElementType.I2: return ConvType.I2;
                case CilElementType.I4: return ConvType.I4;
                case CilElementType.I8: return ConvType.I8;
                case CilElementType.U1: return ConvType.U1;
                case CilElementType.U2: return ConvType.U2;
                case CilElementType.U4: return ConvType.U4;
                case CilElementType.U8: return ConvType.U8;
                case CilElementType.R4: return ConvType.R4;
                case CilElementType.R8: return ConvType.R8;
                case CilElementType.I: return ConvType.I;
                case CilElementType.U: return ConvType.U;
                case CilElementType.Ptr: return ConvType.Ptr;
            }

            // Requested CilElementType is not supported
            throw new NotSupportedException();
        }

        private static readonly Type[][] s_convTable = new Type[13][] {
            /* I1 */ new Type[13] { 
                /* I1 */ typeof(IR.MoveInstruction),
                /* I2 */ typeof(IR.LogicalAndInstruction),
                /* I4 */ typeof(IR.LogicalAndInstruction),
                /* I8 */ typeof(IR.LogicalAndInstruction),
                /* U1 */ typeof(IR.MoveInstruction),
                /* U2 */ typeof(IR.LogicalAndInstruction),
                /* U4 */ typeof(IR.LogicalAndInstruction),
                /* U8 */ typeof(IR.LogicalAndInstruction),
                /* R4 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* R8 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* I  */ typeof(IR.LogicalAndInstruction),
                /* U  */ typeof(IR.LogicalAndInstruction),
                /* Ptr*/ typeof(IR.LogicalAndInstruction),
            },
            /* I2 */ new Type[13] { 
                /* I1 */ typeof(IR.SignExtendedMoveInstruction),
                /* I2 */ typeof(IR.MoveInstruction),
                /* I4 */ typeof(IR.LogicalAndInstruction),
                /* I8 */ typeof(IR.LogicalAndInstruction),
                /* U1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U2 */ typeof(IR.MoveInstruction),
                /* U4 */ typeof(IR.LogicalAndInstruction),
                /* U8 */ typeof(IR.LogicalAndInstruction),
                /* R4 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* R8 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* I  */ typeof(IR.LogicalAndInstruction),
                /* U  */ typeof(IR.LogicalAndInstruction),
                /* Ptr*/ typeof(IR.LogicalAndInstruction),
            },
            /* I4 */ new Type[13] { 
                /* I1 */ typeof(IR.SignExtendedMoveInstruction),
                /* I2 */ typeof(IR.SignExtendedMoveInstruction),
                /* I4 */ typeof(IR.MoveInstruction),
                /* I8 */ typeof(IR.LogicalAndInstruction),
                /* U1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U2 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U4 */ typeof(IR.MoveInstruction),
                /* U8 */ typeof(IR.LogicalAndInstruction),
                /* R4 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* R8 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* I  */ typeof(IR.LogicalAndInstruction),
                /* U  */ typeof(IR.LogicalAndInstruction),
                /* Ptr*/ typeof(IR.LogicalAndInstruction),
            },
            /* I8 */ new Type[13] {
                /* I1 */ typeof(IR.SignExtendedMoveInstruction),
                /* I2 */ typeof(IR.SignExtendedMoveInstruction),
                /* I4 */ typeof(IR.SignExtendedMoveInstruction),
                /* I8 */ typeof(IR.MoveInstruction),
                /* U1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U2 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U4 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U8 */ typeof(IR.MoveInstruction),
                /* R4 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* R8 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* I  */ typeof(IR.LogicalAndInstruction),
                /* U  */ typeof(IR.LogicalAndInstruction),
                /* Ptr*/ typeof(IR.LogicalAndInstruction),
            },
            /* U1 */ new Type[13] {
                /* I1 */ typeof(IR.MoveInstruction),
                /* I2 */ typeof(IR.LogicalAndInstruction),
                /* I4 */ typeof(IR.LogicalAndInstruction),
                /* I8 */ typeof(IR.LogicalAndInstruction),
                /* U1 */ typeof(IR.MoveInstruction),
                /* U2 */ typeof(IR.LogicalAndInstruction),
                /* U4 */ typeof(IR.LogicalAndInstruction),
                /* U8 */ typeof(IR.LogicalAndInstruction),
                /* R4 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* R8 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* I  */ typeof(IR.LogicalAndInstruction),
                /* U  */ typeof(IR.LogicalAndInstruction),
                /* Ptr*/ typeof(IR.LogicalAndInstruction),
            },
            /* U2 */ new Type[13] {
                /* I1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* I2 */ typeof(IR.MoveInstruction),
                /* I4 */ typeof(IR.LogicalAndInstruction),
                /* I8 */ typeof(IR.LogicalAndInstruction),
                /* U1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U2 */ typeof(IR.MoveInstruction),
                /* U4 */ typeof(IR.LogicalAndInstruction),
                /* U8 */ typeof(IR.LogicalAndInstruction),
                /* R4 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* R8 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* I  */ typeof(IR.LogicalAndInstruction),
                /* U  */ typeof(IR.LogicalAndInstruction),
                /* Ptr*/ typeof(IR.LogicalAndInstruction),
            },
            /* U4 */ new Type[13] {
                /* I1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* I2 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* I4 */ typeof(IR.MoveInstruction),
                /* I8 */ typeof(IR.LogicalAndInstruction),
                /* U1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U2 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U4 */ typeof(IR.MoveInstruction),
                /* U8 */ typeof(IR.LogicalAndInstruction),
                /* R4 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* R8 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* I  */ typeof(IR.LogicalAndInstruction),
                /* U  */ typeof(IR.LogicalAndInstruction),
                /* Ptr*/ typeof(IR.LogicalAndInstruction),
            },
            /* U8 */ new Type[13] {
                /* I1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* I2 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* I4 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* I8 */ typeof(IR.MoveInstruction),
                /* U1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U2 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U4 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U8 */ typeof(IR.MoveInstruction),
                /* R4 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* R8 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* I  */ typeof(IR.LogicalAndInstruction),
                /* U  */ typeof(IR.LogicalAndInstruction),
                /* Ptr*/ typeof(IR.LogicalAndInstruction),
            },
            /* R4 */ new Type[13] {
                /* I1 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* I2 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* I4 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* I8 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* U1 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* U2 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* U4 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* U8 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* R4 */ typeof(IR.MoveInstruction),
                /* R8 */ typeof(IR.MoveInstruction),
                /* I  */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* U  */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* Ptr*/ null,
            },
            /* R8 */ new Type[13] {
                /* I1 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* I2 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* I4 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* I8 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* U1 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* U2 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* U4 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* U8 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* R4 */ typeof(IR.MoveInstruction),
                /* R8 */ typeof(IR.MoveInstruction),
                /* I  */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* U  */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* Ptr*/ null,
            },
            /* I  */ new Type[13] {
                /* I1 */ typeof(IR.SignExtendedMoveInstruction),
                /* I2 */ typeof(IR.SignExtendedMoveInstruction),
                /* I4 */ typeof(IR.SignExtendedMoveInstruction),
                /* I8 */ typeof(IR.SignExtendedMoveInstruction),
                /* U1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U2 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U4 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U8 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* R4 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* R8 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* I  */ typeof(IR.MoveInstruction),
                /* U  */ typeof(IR.MoveInstruction),
                /* Ptr*/ typeof(IR.MoveInstruction),
            },
            /* U  */ new Type[13] {
                /* I1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* I2 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* I4 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* I8 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U2 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U4 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U8 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* R4 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* R8 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* I  */ typeof(IR.MoveInstruction),
                /* U  */ typeof(IR.MoveInstruction),
                /* Ptr*/ typeof(IR.MoveInstruction),
            },
            /* Ptr*/ new Type[13] {
                /* I1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* I2 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* I4 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* I8 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U2 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U4 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U8 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* R4 */ null,
                /* R8 */ null,
                /* I  */ typeof(IR.MoveInstruction),
                /* U  */ typeof(IR.MoveInstruction),
                /* Ptr*/ typeof(IR.MoveInstruction),
            },
        };

        /// <summary>
        /// Selects the appropriate IR conversion instruction.
        /// </summary>
        /// <param name="instruction">The IL conversion instruction.</param>
        /// <param name="ctx">The transformation context.</param>
        private void ProcessConversionInstruction(ConversionInstruction instruction, Context ctx)
        {
            Operand dest = instruction.Results[0];
            Operand src = instruction.Operands[0];

            CheckAndConvertInstruction(ctx, dest, src);
        }

        private void CheckAndConvertInstruction(Context context, Operand destinationOperand, Operand sourceOperand)
        {
            uint mask = 0;
            Type extension = null;
            ConvType ctDest = ConvTypeFromCilType(destinationOperand.Type.Type);
            ConvType ctSrc = ConvTypeFromCilType(sourceOperand.Type.Type);

            Type type = s_convTable[(int)ctDest][(int)ctSrc];
            if (null == type)
                throw new NotSupportedException();

            ComputeExtensionTypeAndMask(ctDest, ref extension, ref mask);

            if (type == typeof(LogicalAndInstruction) || 0 != mask)
            {
                Debug.Assert(0 != mask, @"Conversion is an AND, but no mask given.");

                List<Instruction> instructions = new List<Instruction>();
                if (type != typeof(LogicalAndInstruction))
                {
                    ProcessMixedTypeConversion(instructions, type, mask, destinationOperand, sourceOperand);
                }
                else
                {
                    ProcessSingleTypeTruncation(instructions, type, mask, destinationOperand, sourceOperand);
                }

                ExtendAndTruncateResult(instructions, extension, destinationOperand);

                Replace(context, instructions);
            }
            else
            {
                Replace(context, Architecture.CreateInstruction(type, destinationOperand, sourceOperand));
            }
        }

        private void ComputeExtensionTypeAndMask(ConvType destinationType, ref Type extensionType, ref uint mask)
        {
            switch (destinationType)
            {
                case ConvType.I1:
                    mask = 0xFF;
                    extensionType = typeof(SignExtendedMoveInstruction);
                    break;

                case ConvType.I2:
                    mask = 0xFFFF;
                    extensionType = typeof(SignExtendedMoveInstruction);
                    break;

                case ConvType.I4:
                    mask = 0xFFFFFFFF;
                    break;

                case ConvType.I8:
                    break;

                case ConvType.U1:
                    mask = 0xFF;
                    extensionType = typeof(ZeroExtendedMoveInstruction);
                    break;

                case ConvType.U2:
                    mask = 0xFFFF;
                    extensionType = typeof(ZeroExtendedMoveInstruction);
                    break;

                case ConvType.U4:
                    mask = 0xFFFFFFFF;
                    break;

                case ConvType.U8:
                    break;

                case ConvType.R4:
                    break;

                case ConvType.R8:
                    break;

                case ConvType.I:
                    break;

                case ConvType.U:
                    break;

                case ConvType.Ptr:
                    break;

                default:
                    Debug.Assert(false);
                    throw new NotSupportedException();
            }
        }

        private void ProcessMixedTypeConversion(List<Instruction> instructionList, Type type, uint mask, Operand destinationOperand, Operand sourceOperand)
        {
            instructionList.AddRange(new[] {
                Architecture.CreateInstruction(type, destinationOperand, sourceOperand),
                Architecture.CreateInstruction(typeof(LogicalAndInstruction), destinationOperand, destinationOperand, new ConstantOperand(new SigType(CilElementType.U4), mask))
            });
        }

        private void ProcessSingleTypeTruncation(List<Instruction> instructionList, Type type, uint mask, Operand destinationOperand, Operand sourceOperand)
        {
            if (sourceOperand.Type.Type == CilElementType.I8 || sourceOperand.Type.Type == CilElementType.U8)
            {
                instructionList.Add(Architecture.CreateInstruction(typeof(IR.MoveInstruction), destinationOperand, sourceOperand));
                instructionList.Add(Architecture.CreateInstruction(type, destinationOperand, sourceOperand, new ConstantOperand(new SigType(CilElementType.U4), mask)));
            }
            else
                instructionList.Add(Architecture.CreateInstruction(type, destinationOperand, sourceOperand, new ConstantOperand(new SigType(CilElementType.U4), mask)));
        }

        private void ExtendAndTruncateResult(List<Instruction> instructionList, Type extensionType, Operand destinationOperand)
        {
            if (null != extensionType && destinationOperand is RegisterOperand)
            {
                RegisterOperand resultOperand = new RegisterOperand(new SigType(CilElementType.I4), ((RegisterOperand)destinationOperand).Register);
                instructionList.Add(Architecture.CreateInstruction(extensionType, resultOperand, destinationOperand));
            }
        }

        /// <summary>
        /// Processes redirectable method call instructions.
        /// </summary>
        /// <param name="instruction">The call instruction.</param>
        /// <param name="ctx">The transformation context.</param>
        /// <remarks>
        /// This method checks if the call target has an Intrinsic-Attribute applied with
        /// the current architecture. If it has, the method call is replaced by the specified
        /// native instruction.
        /// </remarks>
        private void ProcessRedirectableInvokeInstruction(CallInstruction instruction, Context ctx)
        {
            // Retrieve the invocation target
            RuntimeMethod rm = instruction.InvokeTarget;
            Debug.Assert(null != rm, @"Call doesn't have a target.");
            // Retrieve the runtime type
            RuntimeType rt = RuntimeBase.Instance.TypeLoader.GetType(@"Mosa.Runtime.CompilerFramework.IntrinsicAttribute, Mosa.Runtime");
            // The replacement instruction
            Instruction replacement = null;

            if (rm.IsDefined(rt))
            {
                // FIXME: Change this to a GetCustomAttributes call, once we can do that :)
                foreach (RuntimeAttribute ra in rm.CustomAttributes)
                {
                    if (ra.Type == rt)
                    {
                        // Get the intrinsic attribute
                        IntrinsicAttribute ia = (IntrinsicAttribute)ra.GetAttribute();
                        if (ia.Architecture.IsInstanceOfType(Architecture))
                        {
                            // Found a replacement for the call...
                            try
                            {
                                Operand[] args = new Operand[instruction.Results.Length + instruction.Operands.Length];
                                int idx = 0;
                                foreach (Operand op in instruction.Results)
                                    args[idx++] = op;
                                foreach (Operand op in instruction.Operands)
                                    args[idx++] = op;

                                replacement = (Instruction)Activator.CreateInstance(ia.InstructionType, args, null);
                                break;
                            }
                            catch (Exception e)
                            {
                                Trace.WriteLine("Failed to replace intrinsic call with its instruction:");
                                Trace.WriteLine(e);
                            }
                        }
                    }
                }
            }

            if (replacement == null)
                ProcessInvokeInstruction(instruction, ctx);
            else
                Replace(ctx, replacement);
        }

        /// <summary>
        /// Processes a method call instruction.
        /// </summary>
        /// <param name="instruction">The call instruction.</param>
        /// <param name="ctx">The transformation context.</param>
        private void ProcessInvokeInstruction(InvokeInstruction instruction, Context ctx)
        {
            /* FIXME: Check for IntrinsicAttribute and replace the invoke instruction with the intrinsic,
             * if necessary.
             */
        }

        /// <summary>
        /// Replaces the IL load instruction by an appropriate IR move instruction or removes it entirely, if
        /// it is a native size.
        /// </summary>
        /// <param name="load">The IL load instruction to process.</param>
        /// <param name="ctx">Provides the transformation context.</param>
        private void ProcessLoadInstruction(ILoadInstruction load, Context ctx)
        {
            // We don't need to rewire the source/destination yet, its already there. :(
            Remove(ctx);

/* FIXME: This is only valid with reg alloc!
            Type type = null;

            load = load as LoadInstruction;

            // Is this a sign or zero-extending move?
            if (true == IsSignExtending(load.Source))
            {
                type = typeof(IR.SignExtendedMoveInstruction);
            }
            else if (true == IsZeroExtending(load.Source))
            {
                type = typeof(IR.ZeroExtendedMoveInstruction);
            }

            // Do we have a move replacement?
            if (null == type)
            {
                // No, we can safely drop the load instruction and can rewire the operands.
                /*if (1 == load.Destination.Definitions.Count && 1 == load.Destination.Uses.Count)
                {
                    load.Destination.Replace(load.Source);
                    Remove(ctx);
                }
                return;
            }
            else
            {
                Replace(ctx, Architecture.CreateInstruction(type, load.Destination, load.Source));
            }*/
        }

        /// <summary>
        /// Replaces the IL store instruction by an appropriate IR move instruction.
        /// </summary>
        /// <param name="store">The IL store instruction to replace.</param>
        /// <param name="ctx">Provides the transformation context.</param>
        private void ProcessStoreInstruction(IStoreInstruction store, Context ctx)
        {
            // Do we need to truncate the value?
            if (IsTruncating(store.Destination, store.Source))
            {
                // Replace it with a truncating move...
                // FIXME: Implement proper truncation as specified in the CIL spec
                //Debug.Assert(false);
                if (IsSignExtending(store.Source))
                    Replace(ctx, Architecture.CreateInstruction(typeof(SignExtendedMoveInstruction), store.Destination, store.Source));
                else
                    Replace(ctx, Architecture.CreateInstruction(typeof(ZeroExtendedMoveInstruction), store.Destination, store.Source));
                return;
            }
            if (1 == store.Source.Definitions.Count && 1 == store.Source.Uses.Count)
            {
                store.Source.Replace(store.Destination);
                Remove(ctx);
                return;
            }

            Replace(ctx, Architecture.CreateInstruction(typeof(MoveInstruction), store.Destination, store.Source));
        }


        /// <summary>
        /// Gets the condition code for an opcode.
        /// </summary>
        /// <param name="opCode">The op code.</param>
        /// <returns>The IR condition code.</returns>
        private ConditionCode GetConditionCode(OpCode opCode)
        {
            ConditionCode result;
            switch (opCode)
            {
                case OpCode.Ceq: result = ConditionCode.Equal; break;
                case OpCode.Cgt: result = ConditionCode.GreaterThan; break;
                case OpCode.Cgt_un: result = ConditionCode.UnsignedGreaterThan; break;
                case OpCode.Clt: result = ConditionCode.LessThan; break;
                case OpCode.Clt_un: result = ConditionCode.UnsignedLessThan; break;

                default:
                    throw new NotSupportedException();
            }
            return result;
        }

        /// <summary>
        /// Replaces the instruction with an internal call.
        /// </summary>
        /// <param name="ctx">The transformation context.</param>
        /// <param name="instruction">The instruction to replace.</param>
        /// <param name="internalCallTarget">The internal call target.</param>
        private void ReplaceWithInternalCall(Context ctx, Instruction instruction, object internalCallTarget)
        {
            RuntimeType rt = RuntimeBase.Instance.TypeLoader.GetType(@"Mosa.Runtime.RuntimeBase");
            RuntimeMethod callTarget = FindMethod(rt, internalCallTarget.ToString());

            // Transform the opcode with an internal call
            CallInstruction call = new CallInstruction(OpCode.Call);
            call.SetInvokeTarget(Compiler, callTarget);

            int i = 0;
            foreach (Operand op in instruction.Operands)
                call.SetOperand(i++, op);
            i = 0;
            foreach (Operand op in instruction.Results)
                call.SetResult(i++, op);

            Replace(ctx, call);
        }

        /// <summary>
        /// Finds the method.
        /// </summary>
        /// <param name="rt">The rt.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private RuntimeMethod FindMethod(RuntimeType rt, string name)
        {
            //name[0] = Char.ToUpper(name[0]);
            foreach (RuntimeMethod method in rt.Methods)
            {
                if (name == method.Name)
                {
                    return method;
                }
            }

            throw new MissingMethodException(rt.Name, name);
        }

        #endregion // Internals
    }
}
