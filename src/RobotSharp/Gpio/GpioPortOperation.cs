namespace RobotSharp.Gpio
{
    public class GpioPortOperation
    {
        public GpioPortOperation(int pin)
        {
            Pin = pin;
        }

        public int Pin { get; private set; }
        public HighLow Value { get; set; }
        public int Duration { get; set; }
    }
}