﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.RSP.Command;

public class SetBreakPoint : GDBCommand
{
	protected byte Type;

	public ulong Address { get; }

	public ulong Size { get; }

	protected override string PackArguments => $"{Type},{Address:x},{Size:x}";

	public SetBreakPoint(ulong address, byte size, byte type, CallBack callBack = null) : base("Z", callBack)
	{
		Address = address;
		Size = size;
		Type = type;
	}
}
