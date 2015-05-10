using System;
using System.Collections.Generic;
using RobotSharp.Tools;

namespace RobotSharp.Gpio
{
    public interface IGpioPort : ISetupable, IDisposable
    {
        // simple gpio output / input
        void Setup(int gpio, Direction direction, PullUpDown pullUpDown);
        void Output(int gpio, HighLow value, long duration = -1);
        void Output(IEnumerable<GpioPortOperation> operations);
        HighLow Input(int gpio);

        // pwm
        void StartPwm(int gpio);
        void ControlPwm(int gpio, float? frequency, float? dutyCycle);
        void StopPwm(int gpio);
    }
}