using System;
using RobotSharp.Tools;

namespace RobotSharp.Devices
{
    public interface IServo : ISetupable, IDisposable
    {
        void Move(int degrees);
    }
}