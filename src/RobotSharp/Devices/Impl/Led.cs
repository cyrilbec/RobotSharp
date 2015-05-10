using RobotSharp.Gpio;

namespace RobotSharp.Devices.Impl
{
    public class Led : ILed
    {
        public IGpioPort GpioPort { get; set; }

        private int pin;

        public Led(int pin)
        {
            this.pin = pin;
        }

        public void Setup()
        {
            GpioPort.Setup(pin, Direction.Output, PullUpDown.Off);
        }

        public void Set(bool on)
        {
            GpioPort.Output(pin, on ? HighLow.High : HighLow.Low);
        }

        public void Dispose()
        {
            Set(false);
        }
    }
}