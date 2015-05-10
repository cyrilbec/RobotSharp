using RobotSharp.Gpio;

namespace RobotSharp.Devices
{
    public class Switch : IDevice
    {
        public IGpioPort GpioPort { get; set; }

        private int pin;

        public Switch(int pin)
        {
            this.pin = pin;
        }

        public void Setup()
        {
            GpioPort.Setup(pin, Direction.Input, PullUpDown.Up);
        }

        public bool Value()
        {
            return GpioPort.Input(pin) == HighLow.High;
        }
        
        public void Dispose()
        {
            // nothing to do
        }
    }
}