using RobotSharp.Gpio;
using RobotSharp.Tools;

namespace RobotSharp.Devices.Impl
{
    public class Motor : IMotor
    {
        public IOperatingSystemService OperatingSystemService { get; set; }
        public IGpioController GpioController { get; set; }

        private IChannel channelA;
        private IChannel channelB;

        private ChannelPwm pwmChannelA;
        private ChannelPwm pwmChannelB;

        private int pinA;
        private int pinB;

        public Motor(int pinA, int pinB)
        {
            this.pinA = pinA;
            this.pinB = pinB;
        }

        public void Setup()
        {
            channelA = GpioController.GetChannel(pinA);
            channelA.ChangeDirection(Direction.Output);
            pwmChannelA = new ChannelPwm(OperatingSystemService, channelA);
            pwmChannelA.ChangeDutyCycle(0);
            pwmChannelA.ChangeFrequency(20);
            pwmChannelA.Start();

            channelB = GpioController.GetChannel(pinB);
            channelB.ChangeDirection(Direction.Output);
            pwmChannelB = new ChannelPwm(OperatingSystemService, channelB);
            pwmChannelB.ChangeDutyCycle(0);
            pwmChannelB.ChangeFrequency(20);
            pwmChannelB.Start();
        }

        public void Forward(float speed)
        {
            pwmChannelA.ChangeDutyCycle(speed);
            pwmChannelA.ChangeFrequency(speed + 5);

            pwmChannelB.ChangeDutyCycle(0);
        }

        public void Reverse(float speed)
        {
            pwmChannelA.ChangeDutyCycle(0);

            pwmChannelB.ChangeDutyCycle(speed);
            pwmChannelB.ChangeFrequency(speed + 5);
        }

        public void Stop()
        {
            pwmChannelA.ChangeDutyCycle(0);
            pwmChannelB.ChangeDutyCycle(0);
        }

        public void Dispose()
        {
            Stop();

            // stop pwm on each motor
            pwmChannelA.Stop();
            pwmChannelB.Stop();
        }
    }
}