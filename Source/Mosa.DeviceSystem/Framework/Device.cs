﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.DeviceSystem.HardwareAbstraction;
using Mosa.DeviceSystem.Services;

namespace Mosa.DeviceSystem.Framework;

/// <summary>
/// Describes a generic device in the device driver framework. It is used everywhere in the framework as the "parent" class of all
/// devices. It uses the concept of parents and children, in that the current device can retrieve its parent device and its children
/// devices.
/// </summary>
public class Device
{
	public string Name { get; set; }

	public DeviceBusType BusType { get; set; }

	public BaseDeviceDriver DeviceDriver { get; set; }

	public DeviceStatus Status { get; set; }

	public Device Parent { get; set; }

	//public IService Service { get; set; }

	public List<Device> Children { get; } = new List<Device>();

	public HardwareResources Resources { get; set; }

	public BaseDeviceConfiguration Configuration { get; set; }

	public ulong ComponentID { get; set; }

	public DeviceDriverRegistryEntry DeviceDriverRegistryEntry { get; set; }

	public DeviceService DeviceService { get; internal set; }
}
