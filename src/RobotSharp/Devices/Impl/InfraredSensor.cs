using RobotSharp.Gpio;

namespace RobotSharp.Devices.Impl
{
    public class InfraredSensor : IInfraredSensor
    {
        public IGpioPort GpioPort { get; set; }

        private int pin;

        public InfraredSensor(int pin)
        {
            this.pin = pin;
        }

        public void Setup()
        {
            GpioPort.Setup(pin, Direction.Input, PullUpDown.Off);
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