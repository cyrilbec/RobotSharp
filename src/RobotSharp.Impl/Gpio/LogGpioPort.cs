using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using RobotSharp.Gpio;

namespace RobotSharp.Pi2Go.Gpio
{
    public class LogGpioPort : IGpioPort
    {
        private TextWriter textWriter;

        public LogGpioPort(TextWriter textWriter)
        {
            this.textWriter = textWriter;
        }

        public void Setup(int pin, Direction direction, PullUpDown pullUpDown)
        {
            textWriter.WriteLine("Setup pin {0} Direction {1} PullUpDown {2}", pin, direction, pullUpDown);
        }

        public Task SetupAsync(int pin, Direction direction, PullUpDown pullUpDown)
        {
            throw new NotImplementedException();
        }

        public void Output(int pin, HighLow value, long duration = -1)
        {
            textWriter.WriteLine("Output pin {0} Value {1} Duration {2}", pin, value, duration);
        }

        public Task OutputAsync(int pin, HighLow value, long duration = -1)
        {
            throw new NotImplementedException();
        }

        public void Output(IEnumerable<GpioPortOperation> operations)
        {
            foreach(var operation in operations)
                Output(operation.Pin, operation.Value, operation.Duration);
        }

        public Task OutputAsync(IEnumerable<GpioPortOperation> operations)
        {
            throw new NotImplementedException();
        }

        public HighLow Input(int pin)
        {
            textWriter.WriteLine("Input pin {0}", pin);
            return HighLow.Low;
        }

        public Task<HighLow> InputAsync(int pin)
        {
            throw new NotImplementedException();
        }

        public void StartPwm(int pin)
        {
            textWriter.WriteLine("Start PWM pin {0}", pin);
        }

        public Task StartPwmAsync(int pin)
        {
            throw new NotImplementedException();
        }

        public void ControlPwm(int pin, float? frequency, float? dutyCycle)
        {
            textWriter.WriteLine("Control PWM pin {0} Frequency {1} DutyCycle {2}", pin, frequency, dutyCycle);
        }

        public Task ControlPwmAsync(int pin, float? frequency, float? dutyCycle)
        {
            throw new NotImplementedException();
        }

        public void StopPwm(int pin)
        {
            textWriter.WriteLine("Stop PWM pin {0}", pin);
        }

        public Task StopPwmAsync(int pin)
        {
            throw new NotImplementedException();
        }

        public void Setup()
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}
