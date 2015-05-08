using System;
using RobotSharp.Tools;

namespace RobotSharp.Gpio
{
    public interface IGpioPort : ISetupable, IDisposable
    {
        // simple gpio output / input
        void SetupGpio(int gpio, Direction direction, PullUpDown pullUpDown);
        void OutputGpio(int gpio, HighLow value);
        HighLow InputGpio(int gpio);
    }
}