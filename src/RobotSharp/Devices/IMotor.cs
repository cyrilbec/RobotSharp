using System;
using RobotSharp.Tools;

namespace RobotSharp.Devices
{
    public interface IMotor : ISetupable, IDisposable
    {
        void Forward(float speed);
        void Reverse(float speed);
        void Stop();
    }
}