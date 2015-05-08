using System;
using RobotSharp.Tools;

namespace RobotSharp.Devices
{
    public interface ISwitch : ISetupable, IDisposable
    {
        bool Value();
    }
}