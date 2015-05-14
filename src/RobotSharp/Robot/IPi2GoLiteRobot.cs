using System;
using RobotSharp.Tools;

namespace RobotSharp.Robot
{
    public interface IPi2GoLiteRobot : ISetupable, IDisposable
    {
        void Forward(float speed);
        void Forward(float leftSpeed, float rightSpeed);
        void Reverse(float speed);
        void SpinLeft(float speed);
        void SpinRight(float speed);
        void Stop();

        void CameraChangePanPosition(int degrees);
        void CameraChangeTiltPosition(int degrees);
    }
}