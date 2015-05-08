using System;
using RobotSharp.Tools;

namespace RobotSharp
{
    public interface IGpioController : ISetupable, IDisposable
    {
        IChannel GetChannel(int channel);
    }
}