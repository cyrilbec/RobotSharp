using System;
using RobotSharp.Tools;

namespace RobotSharp.Devices
{
    public interface ILed : ISetupable, IDisposable
    {
        void Set(bool on);
    }
}