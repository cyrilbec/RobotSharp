using RobotSharp.Gpio;

namespace RobotSharp.Devices.Impl
{
    public class Switch : ISwitch
    {
        public IGpioController GpioController { get; set; }

        private int pin;
        private IChannel channel;

        public Switch(int pin)
        {
            this.pin = pin;
        }

        public void Setup()
        {
            channel = GpioController.GetChannel(pin);
            channel.ChangeDirection(Direction.Input, PullUpDown.Up);
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