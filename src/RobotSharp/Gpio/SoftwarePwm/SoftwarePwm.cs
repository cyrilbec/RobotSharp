using System;
using System.Collections.Generic;
using RobotSharp.Tools;

namespace RobotSharp.Gpio.SoftwarePwm
{
    public class SoftwarePwm
    {
        private readonly IGpioPort gpioPort;
        private readonly IOperatingSystemService operatingSystemService;

        private readonly IDictionary<int, SoftwareChannelPwm> pwms =
            new Dictionary<int, SoftwareChannelPwm>();

        public SoftwarePwm(IOperatingSystemService operatingSystemService, IGpioPort gpioPort)
        {
            this.gpioPort = gpioPort;
            this.operatingSystemService = operatingSystemService;
        }

        public void StartPwm(int gpio)
        {
            if (pwms.ContainsKey(gpio)) throw new Exception("Pwm already started on pin");

            var pwm = new SoftwareChannelPwm(operatingSystemService, gpioPort, gpio);
            pwms.Add(gpio, pwm);
            pwm.Start();
        }

        public void ControlPwm(int gpio, float? frequency, float? dutyCycle)
        {
            var pwm = GetPwm(gpio);

            if (frequency.HasValue)
                pwm.ChangeFrequency(frequency.Value);
            if (dutyCycle.HasValue)
                pwm.ChangeDutyCycle(dutyCycle.Value);
        }

        public void StopPwm(int gpio)
        {
            var pwm = GetPwm(gpio);
            pwm.Stop();
        }

        private SoftwareChannelPwm GetPwm(int gpio)
        {
            if (!pwms.ContainsKey(gpio)) throw new Exception(string.Format("Pwm not started on pin {0}", gpio));
            return pwms[gpio];
        }
    }
}