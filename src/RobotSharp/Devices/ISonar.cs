using System;
using RobotSharp.Tools;

namespace RobotSharp.Devices
{
    public interface ISonar : ISetupable, IDisposable
    {
        double Distance();
    }
}