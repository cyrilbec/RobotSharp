using RobotSharp.Gpio;

namespace RobotSharp.Devices.Impl
{
    public class InfraredSensor : IInfraredSensor
    {
        public IGpioController GpioController { get; set; }

        private IChannel channel;
        private int pin;

        public InfraredSensor(int pin)
        {
            this.pin = pin;
        }

        public void Setup()
        {
            channel = GpioController.GetChannel(pin);
            channel.ChangeDirection(Direction.Input);
        }

        public bool Value()
        {
            return channel.Read() == HighLow.High;
        }

        public void Dispose()
        {
            // nothing to do
        }
    }
}