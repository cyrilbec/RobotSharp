using System;
using RobotSharp.Gpio;
using RobotSharp.Tools;

namespace RobotSharp.Devices
{
    public interface IDevice : ISetupable, IDisposable
    {
        IGpioPort GpioPort { get; set; }
    }
}