using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RobotSharp.Tools;

namespace RobotSharp.Gpio
{
    public interface IGpioPort : ISetupable, IDisposable
    {
        // simple gpio output / input
        void Setup(int pin, Direction direction, PullUpDown pullUpDown);
        Task SetupAsync(int pin, Direction direction, PullUpDown pullUpDown);

        void Output(int pin, HighLow value, long duration = -1);
        Task OutputAsync(int pin, HighLow value, long duration = -1);

        void Output(IEnumerable<GpioPortOperation> operations);
        Task OutputAsync(IEnumerable<GpioPortOperation> operations);

        HighLow Input(int pin);
        Task<HighLow> InputAsync(int pin);
        
        // pwm
        void StartPwm(int pin);
        Task StartPwmAsync(int pin);

        void ControlPwm(int pin, float? frequency, float? dutyCycle);
        Task ControlPwmAsync(int pin, float? frequency, float? dutyCycle);

        void StopPwm(int pin);
        Task StopPwmAsync(int pin);
    }
}