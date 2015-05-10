using System;
using RobotSharp.Gpio;
using RobotSharp.Tools;

namespace RobotSharp.Devices
{
    public class Sonar : IDevice
    {
        public IOperatingSystemService OperatingSystemService { get; set; }
        public IGpioPort GpioPort { get; set; }

        private int pin;

        public Sonar(int pin)
        {
            this.pin = pin;
        }

        public void Setup()
        {
            
        }

        public double Distance()
        {
            Echo();

            var start = GetTime();
            var count = start;
            GpioPort.Setup(pin, Direction.Input, PullUpDown.Off);

            while (GpioPort.Input(pin) == HighLow.Low && (GetTime() - count) < 1) { }
            start = GetTime();
            count = start;
            var stop = count;

            while (GpioPort.Input(pin) == HighLow.High & (GetTime() - count) < 1) { }
            stop = GetTime();

            // Calculate pulse length
            var elapsed = stop - start;
            // Distance pulse travelled in that time is time
            // multiplied by the speed of sound 34000(cm/s) divided by 2

            return elapsed * 17000;
        }

        private void Echo()
        {
            GpioPort.Setup(pin, Direction.Output, PullUpDown.Off);
            GpioPort.Output(pin, HighLow.High);

            OperatingSystemService.Sleep(1);

            GpioPort.Output(pin, HighLow.Low);
        }

        private double GetTime()
        {
            return (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds/1000;
        }

        public void Dispose()
        {
            // nothing to do
        }
    }
}