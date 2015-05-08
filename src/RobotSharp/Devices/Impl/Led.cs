using RobotSharp.Gpio;

namespace RobotSharp.Devices.Impl
{
    public class Led : ILed
    {
        public IGpioController GpioController { get; set; }

        private IChannel channel;
        private int pin;

        public Led(int pin)
        {
            this.pin = pin;
        }

        public void Setup()
        {
            channel = GpioController.GetChannel(pin);
            channel.ChangeDirection(Direction.Output);
        }

        public void Set(bool on)
        {
            channel.Write(on ? HighLow.High : HighLow.Low);
        }

        public void Dispose()
        {
            Set(false);
        }
    }
}