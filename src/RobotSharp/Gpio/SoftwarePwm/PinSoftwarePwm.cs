﻿using System;
using RobotSharp.Tools;

namespace RobotSharp.Gpio.SoftwarePwm
{
    internal class SoftwareChannelPwm
    {
        private readonly IGpioPort gpioPort;
        private readonly int pin;

        private float frequency;
        private float dutyCycle;

        // durations to apply
        private long durationHigh;
        private long durationLow;

        private ILoopThread<object> loopThread;

        public SoftwareChannelPwm(IOperatingSystemService operatingSystemService, IGpioPort gpioPort,int pin)
        {
            this.gpioPort = gpioPort;
            this.pin = pin;

            ChangeFrequency(1000.0f);

            loopThread = operatingSystemService.CreateLoopThread<object>(Loop);
        }

        public float Frequency
        {
            get { return frequency; }
        }

        public float DutyCycle
        {
            get { return dutyCycle; }
        }

        public void ChangeDutyCycle(float dutyCycle)
        {
            if (dutyCycle < 0.0 || dutyCycle > 100.0)
                throw new Exception("Duty cycle must have a value from 0.0 to 100.0");

            this.dutyCycle = dutyCycle;

            UpdateDurations();
        }

        public void ChangeFrequency(float frequency)
        {
            if (frequency <= 0.0)
                throw new Exception("Frequency must be greater than 0.0");

            this.frequency = frequency; // x times for 1000ms

            UpdateDurations();
        }

        public void Start()
        {
            loopThread.Start(false);
        }

        public void Stop()
        {
            loopThread.Stop();
        }

        private const int MillisecondsToNanoseconds = 1000000;

        private void UpdateDurations()
        {
            var sliceTime = 1000.0f / frequency / 100.0f; // in 0.01ms

            // calculate duration on seconds
            durationHigh = Convert.ToInt64(dutyCycle*sliceTime)*MillisecondsToNanoseconds;

            // caculate duration off seconds
            durationLow = Convert.ToInt64((100.0 - dutyCycle)*sliceTime)*MillisecondsToNanoseconds;
        }

        private void Loop(object obj)
        {
            if (dutyCycle > 0.0f)
            {
                gpioPort.Output(pin, HighLow.High, durationHigh);

                if (dutyCycle < 100.0f)
                    gpioPort.Output(pin, HighLow.Low, durationLow);
            }
        }
    }
}