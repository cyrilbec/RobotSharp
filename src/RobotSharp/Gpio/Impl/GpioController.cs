using System;
using System.Collections.Generic;

namespace RobotSharp.Gpio.Impl
{
    public class GpioController : IGpioController
    {
        public IGpioPort GpioPort { get; set; }

        private readonly IDictionary<int, IChannel> channels = new Dictionary<int, IChannel>();

        private static readonly int[] pinToGpioRev3 = new int[41]
        {
            -1, -1, -1, 2, -1,
            3, -1, 4, 14, -1,
            15, 17, 18, 27, -1,
            22, 23, -1, 24, 10,
            -1, 9, 25, 11, 8,
            -1, 7, -1, -1, 5,
            -1, 6, 12, 13, -1,
            19, 16, 26, 20, -1,
            21
        };

        private int[] pinToGpio;
        private bool setup;

        public void Setup()
        {
            if (setup) return;

            GpioPort.Setup();

            // set pin configuration (Pi 2 only)
            pinToGpio = pinToGpioRev3;

            setup = true;
        }

        public IChannel GetChannel(int channel)
        {
            // check if channel number is in range
            if (channel < 1 || channel > 26) throw new Exception("Invalid channel");

            if (channels.ContainsKey(channel)) return channels[channel];

            var gpio = pinToGpio[channel];
            var channelObj = new Channel(this, gpio);

            GpioPort.SetupGpio(channelObj.Gpio, channelObj.Direction, PullUpDown.Off);

            return channelObj;
        }

        public void Dispose()
        {
            if (!setup) return;

            // set everything back to input
            foreach (var channel in channels.Values)
            {
                if(channel.Direction == Direction.Input) continue;

                channel.ChangeDirection(Direction.Input);
            }

            channels.Clear();

            GpioPort.Dispose();

            setup = false;
        }
    }
}