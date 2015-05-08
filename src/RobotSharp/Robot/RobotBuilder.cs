using RobotSharp.Devices.Impl;
using RobotSharp.Gpio;
using RobotSharp.Gpio.Impl;
using RobotSharp.Tools;

namespace RobotSharp.Robot
{
    public class RobotBuilder
    {
        public static Pi2GoLiteRobot BuildPi2GoLite(IOperatingSystemService operatingSystemService, IGpioPort gpioPort)
        {
            IGpioController gpioController = new GpioController() { GpioPort = gpioPort };

            return new Pi2GoLiteRobot()
            {
                GpioController = gpioController,

                MotorLeft = new Motor(26, 24) { GpioController = gpioController, OperatingSystemService = operatingSystemService },
                MotorRight = new Motor(19, 21) { GpioController = gpioController, OperatingSystemService = operatingSystemService },

                LedFront = new Led(15) { GpioController = gpioController },
                LedRear = new Led(16) { GpioController = gpioController },

                IrLeft = new InfraredSensor(7) { GpioController = gpioController },
                IrRight = new InfraredSensor(11) { GpioController = gpioController },
                IrLeftLine = new InfraredSensor(12) { GpioController = gpioController },
                IrRightLine = new InfraredSensor(13) { GpioController = gpioController },

                Sonar = new Sonar(8) { GpioController = gpioController },

                Switch = new Switch(23) { GpioController = gpioController },

                ServoPan = new Servo(22) { GpioController = gpioController, OperatingSystemService = operatingSystemService },
                ServoTilt = new Servo(18) { GpioController = gpioController, OperatingSystemService = operatingSystemService },

                VideoStreaming = null
            };
        }
    }
}