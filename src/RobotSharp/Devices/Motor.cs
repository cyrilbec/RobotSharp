using RobotSharp.Gpio;

namespace RobotSharp.Devices
{
    public class Motor : IDevice
    {
        public IGpioPort GpioPort { get; set; }
        
        private int pinA;
        private int pinB;

        public Motor(int pinA, int pinB)
        {
            this.pinA = pinA;
            this.pinB = pinB;
        }

        public void Setup()
        {
            GpioPort.Setup(pinA, Direction.Output, PullUpDown.Off);
            GpioPort.StartPwm(pinA);
            GpioPort.ControlPwm(pinA, 20, 0);

            GpioPort.Setup(pinB, Direction.Output, PullUpDown.Off);
            GpioPort.StartPwm(pinB);
            GpioPort.ControlPwm(pinB, 20, 0);
        }

        public void Forward(float speed)
        {
            GpioPort.ControlPwm(pinA, speed + 5, speed);
            GpioPort.ControlPwm(pinB, null, 0);
        }

        public void Reverse(float speed)
        {
            GpioPort.ControlPwm(pinA, null, 0);

            GpioPort.ControlPwm(pinB, speed + 5, speed);
        }

        public void Stop()
        {
            GpioPort.ControlPwm(pinA, null, 0);
            GpioPort.ControlPwm(pinB, null, 0);
        }

        public void Dispose()
        {
            Stop();

            // stop pwm on each motor
            GpioPort.StopPwm(pinA);
            GpioPort.StopPwm(pinB);
        }
    }
}