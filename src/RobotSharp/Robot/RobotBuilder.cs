using RobotSharp.Devices.Impl;
using RobotSharp.Gpio;
using RobotSharp.Tools;

namespace RobotSharp.Robot
{
    public class RobotBuilder
    {
        public static Pi2GoLiteRobot BuildPi2GoLite(IOperatingSystemService operatingSystemService, IGpioPort gpioPort)
        {
            return new Pi2GoLiteRobot()
            {
                GpioPort = gpioPort,

                MotorLeft = new Motor(7, 8) { GpioPort = gpioPort },
                MotorRight = new Motor(10, 9) { GpioPort = gpioPort },

                LedFront = new Led(22) { GpioPort = gpioPort },
                LedRear = new Led(23) { GpioPort = gpioPort },

                IrLeft = new InfraredSensor(4) { GpioPort = gpioPort },
                IrRight = new InfraredSensor(17) { GpioPort = gpioPort },
                IrLeftLine = new InfraredSensor(18) { GpioPort = gpioPort },
                IrRightLine = new InfraredSensor(27) { GpioPort = gpioPort },

                Sonar = new Sonar(14) { GpioPort = gpioPort },

                Switch = new Switch(11) { GpioPort = gpioPort },

                ServoPan = new Servo(25) { GpioPort = gpioPort, OperatingSystemService = operatingSystemService },
                ServoTilt = new Servo(24) { GpioPort = gpioPort, OperatingSystemService = operatingSystemService },

                VideoStreaming = null
            };
        }
    }
}