using System;
using RobotSharp.Tools;

namespace RobotSharp.Devices
{
    public interface IInfraredSensor : ISetupable, IDisposable
    {
        bool Value();
    }
}