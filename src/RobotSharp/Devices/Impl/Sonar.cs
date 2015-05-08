using System;
using RobotSharp.Gpio;
using RobotSharp.Tools;

namespace RobotSharp.Devices.Impl
{
    public class Sonar : ISonar
    {
        public IOperatingSystemService OperatingSystemService { get; set; }
        public IGpioController GpioController { get; set; }

        private int pin;
        private IChannel channel;

        public Sonar(int pin)
        {
            this.pin = pin;
        }

        public void Setup()
        {
            channel = GpioController.GetChannel(pin);
        }

        public double Distance()
        {
            Echo();

            var start = GetTime();
            var count = start;
            channel.ChangeDirection(Direction.Input);

            while (channel.Read() == HighLow.Low && (GetTime() - count) < 1) { }
            start = GetTime();
            count = start;
            var stop = count;

            while (channel.Read() == HighLow.High & (GetTime() - count) < 1) { }
            stop = GetTime();

            // Calculate pulse length
            var elapsed = stop - start;
            // Distance pulse travelled in that time is time
            // multiplied by the speed of sound 34000(cm/s) divided by 2

            return elapsed * 17000;
        }

        private void Echo()
        {
            channel.ChangeDirection(Direction.Output);
            channel.Write(HighLow.High);

            OperatingSystemService.Sleep(1);

            channel.Write(HighLow.Low);
        }

        private double GetTime()
        {
            return (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds / 1000;
        }

        public void Dispose()
        {
            // nothing to do
        }
    }
}