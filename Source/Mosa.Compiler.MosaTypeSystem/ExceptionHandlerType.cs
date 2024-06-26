﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.MosaTypeSystem;

/// <summary>
///
/// </summary>
public enum ExceptionHandlerType
{
	/// <summary>
	/// A typed exception handler clause.
	/// </summary>
	Exception = 0,

	/// <summary>
	/// An exception filter and handler clause.
	/// </summary>
	Filter = 1,

	/// <summary>
	/// A finally clause.
	/// </summary>
	Finally = 2,

	/// <summary>
	/// A fault clause. This is similar to finally, except its only executed if an exception is/was processed.
	/// </summary>
	Fault = 4
}
