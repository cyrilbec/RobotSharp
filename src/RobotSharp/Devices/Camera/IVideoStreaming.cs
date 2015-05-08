using System;
using RobotSharp.Tools;

namespace RobotSharp.Devices.Camera
{
    public interface IVideoStreaming : ISetupable, IDisposable
    {
        void Start();
        void Stop();
    }
}