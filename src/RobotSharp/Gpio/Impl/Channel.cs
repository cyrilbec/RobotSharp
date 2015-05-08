using System;

namespace RobotSharp.Gpio.Impl
{
    public class Channel : IChannel
    {
        private readonly GpioController gpioController;
        public IGpioController GpioController
        {
            get { return gpioController; }
        }

        internal Channel(GpioController gpioController, int gpio)
        {
            this.gpioController = gpioController;
            this.gpio = gpio;
        }

        private readonly int gpio;
        internal int Gpio
        {
            get { return gpio; }
        }

        private PullUpDown pullUpDown;
        public PullUpDown PullUpDown
        {
            get { return pullUpDown; }
        }

        private Direction direction;
        public Direction Direction
        {
            get { return direction; }
        }

        public void ChangeDirection(Direction direction, PullUpDown pullUpDown = PullUpDown.Off)
        {
            gpioController.GpioPort.SetupGpio(gpio, direction, pullUpDown);
            this.direction = direction;
        }

        public HighLow Read()
        {
            if (direction != Direction.Input) throw new Exception("The GPIO channel has not been set up as an input");

            return gpioController.GpioPort.InputGpio(gpio);
        }

        public void Write(HighLow value)
        {
            if(direction != Direction.Output) throw new Exception("The GPIO channel has not been set up as an output");

            gpioController.GpioPort.OutputGpio(gpio, value);
        }
    }
}