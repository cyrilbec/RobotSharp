using System;
using RobotSharp.Pi2Go.Gpio;
using RobotSharp.Pi2Go.Tools;
using RobotSharp.Robot;
using RobotSharp.WebSocket.Server;

namespace RobotSharp.WebSocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var operatingSystemService = new ClassicDotnetOperatingSystemService();
            //var gpioPort = new LogGpioPort(Console.Out);
            var gpioPort = new DmaLinuxGpioPort(operatingSystemService);

            // create robot
            var robot = RobotBuilder.BuildPi2GoLite(operatingSystemService, gpioPort);
            robot.Setup();

            using (new Pi2GoWebSocketServer(robot, 81, Console.Out))
            {
                Console.WriteLine("Server started");
                Console.WriteLine("Press any key to quit.");
                Console.ReadKey(true);
            }
        }
    }
}