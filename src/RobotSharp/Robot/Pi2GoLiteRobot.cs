using System;
using System.Collections.Generic;
using System.Linq;
using RobotSharp.Devices;
using RobotSharp.Devices.Camera;
using RobotSharp.Gpio;
using RobotSharp.Tools;

namespace RobotSharp.Robot
{
    public class Pi2GoLiteRobot : ISetupable, IDisposable
    {
        public IGpioPort GpioPort { get; set; }

        public IMotor MotorLeft { get; set; }
        public IMotor MotorRight { get; set; }

        // LEDs
        public ILed LedFront { get; set; }
        public ILed LedRear { get; set; }

        // sonar
        public ISonar Sonar { get; set; }

        // infrared sensor
        public IInfraredSensor IrLeft { get; set; }
        public IInfraredSensor IrRight { get; set; }
        public IInfraredSensor IrLeftLine { get; set; }
        public IInfraredSensor IrRightLine { get; set; }

        // switch
        public ISwitch Switch { get; set; }

        // camera
        public IServo ServoPan { get; set; }
        public IServo ServoTilt { get; set; }
        public IVideoStreaming VideoStreaming { get; set; }

        private object[] allPeripherals;

        private IEnumerable<T> GetAllPeripherals<T>()
        {
            return allPeripherals.Where(x => x != null).Cast<T>();
        }

        private bool setup;

        public void Setup()
        {
            if (setup) return;

            GpioPort.Setup();

            allPeripherals = new object[]
            {
                MotorLeft, MotorRight, LedFront, LedRear, Sonar, IrLeft, IrRight, IrLeftLine, IrRightLine, Switch,
                ServoPan, ServoTilt, VideoStreaming
            };

            // setup all peripherals
            foreach (var periph in GetAllPeripherals<ISetupable>())
                periph.Setup();

            setup = true;
        }

        public void Forward(float speed)
        {
            MotorLeft.Forward(speed);
            MotorRight.Forward(speed);
        }

        public void Forward(float leftSpeed, float rightSpeed)
        {
            MotorLeft.Forward(leftSpeed);
            MotorRight.Forward(rightSpeed);
        }

        public void Reverse(float speed)
        {
            MotorLeft.Reverse(speed);
            MotorRight.Reverse(speed);
        }

        public void SpinLeft(float speed)
        {
            MotorLeft.Reverse(speed);
            MotorRight.Forward(speed);
        }

        public void SpinRight(float speed)
        {
            MotorLeft.Forward(speed);
            MotorRight.Reverse(speed);
        }

        public void Stop()
        {
            MotorLeft.Stop();
            MotorRight.Stop();
        }

        #region camera move

        // current positions of servos
        private int panPosition = 0;
        private int tiltPosition = 0;

        // step in degree
        private int cameraMoveStep = 10;

        public int CameraMoveStep
        {
            get { return cameraMoveStep; }
            set { cameraMoveStep = value; }
        }

        public void CameraLeft()
        {
            if (panPosition <= -90) return;
            panPosition -= CameraMoveStep;
            CameraChangePanPosition(panPosition);
        }

        public void CameraRight()
        {
            if (panPosition >= 90) return;
            panPosition += CameraMoveStep;
            CameraChangePanPosition(panPosition);
        }

        public void CameraUp()
        {
            if (tiltPosition >= 90) return;
            tiltPosition += CameraMoveStep;
            CameraChangeTiltPosition(tiltPosition);
        }

        public void CameraDown()
        {
            if (tiltPosition <= -90) return;
            tiltPosition -= CameraMoveStep;
            CameraChangeTiltPosition(tiltPosition);
        }

        public void CameraChangePanPosition(int degrees)
        {
            ServoPan.Move(degrees);
        }

        public void CameraChangeTiltPosition(int degrees)
        {
            ServoTilt.Move(degrees);
        }

        #endregion

        public void Dispose()
        {
            if (!setup) return;

            foreach (var periph in GetAllPeripherals<IDisposable>())
                periph.Dispose();
        }
    }
}