using System;
using RobotSharp.Gpio;
using RobotSharp.Pi2Go.Gpio;
using RobotSharp.Pi2Go.Tools;
using RobotSharp.Robot;
using RobotSharp.Tools;
using RobotSharp.WebSocket.Client;

namespace ManualControlSample
{
    class Program
    {
        private static IPi2GoLiteRobot robot;

        static void Main(string[] args)
        {
            try
            {
                // direct control
                //robot = BuildLocalRobot();

                // remote control via web socket
                robot = BuildRemoteRobot();

                robot.Setup();

                ManualControl();
            }
            finally
            {
                if (robot != null)
                    robot.Dispose();
            }

            Console.WriteLine("done.");
        }

        private static IPi2GoLiteRobot BuildLocalRobot()
        {
            IOperatingSystemService operatingSystemService = new ClassicDotnetOperatingSystemService();
            IGpioPort gpioPort =
                new DmaLinuxGpioPort(operatingSystemService);
            //new SysfsLinuxGpioPort(operatingSystemService);

            return RobotBuilder.BuildPi2GoLite(operatingSystemService, gpioPort);
        }

        private static IPi2GoLiteRobot BuildRemoteRobot()
        {
            return new WebSocketClientPi2GoLiteRobot("ws://localhost:81/pi2golite");
        }

        static void ManualControl()
        {
            const int speedStep = 10;
            int speed = 80;

            const int cameraStep = 5;
            int cameraPanPosition = 0;
            int cameraTiltPosition = 0;

            Console.WriteLine("press q to quit.");

            // move camera to initial position
            robot.CameraChangePanPosition(cameraPanPosition);
            robot.CameraChangeTiltPosition(cameraTiltPosition);

            // light off
            //robot.LedFront.Set(false);
            //robot.LedRear.Set(false);

            bool ledFront = false, ledRear = false;

            ConsoleKeyInfo key;
            while ((key = Console.ReadKey(true)).Key != ConsoleKey.Q)
            {
                // speed control
                if (key.Key == ConsoleKey.H && speed < 100)
                {
                    speed += speedStep;
                    Console.WriteLine("speed is now at {0}%", speed);
                }
                if (key.Key == ConsoleKey.N && speed > 0)
                {
                    speed -= speedStep;
                    Console.WriteLine("speed is now at {0}%", speed);
                }

                var hasShift = (key.Modifiers & ConsoleModifiers.Shift) != 0;

                // if shift is not pressed, control wheel
                if (!hasShift)
                {
                    if (key.Key == ConsoleKey.UpArrow)
                    {
                        robot.Forward(speed);
                        Console.WriteLine("forward");
                    }
                    if (key.Key == ConsoleKey.DownArrow)
                    {
                        robot.Reverse(speed);
                        Console.WriteLine("reverse");
                    }
                    if (key.Key == ConsoleKey.RightArrow)
                    {
                        robot.SpinRight(speed);
                        Console.WriteLine("spin right");
                    }
                    if (key.Key == ConsoleKey.LeftArrow)
                    {
                        robot.SpinLeft(speed);
                        Console.WriteLine("spin left");
                    }
                    if (key.Key == ConsoleKey.Spacebar)
                    {
                        robot.Stop();
                        Console.WriteLine("stop");
                    }
                    if (key.Key == ConsoleKey.D)
                    {
                        //Console.WriteLine("distance = {0}cm", robot.Sonar.Distance());
                    }
                }
                else
                {
                    Console.WriteLine("shift  + " + key.Key);
                    // if shift move camera
                    if (key.Key == ConsoleKey.A || key.Key == ConsoleKey.UpArrow)
                    {
                        cameraTiltPosition += cameraStep;
                        robot.CameraChangeTiltPosition(cameraTiltPosition);
                        Console.WriteLine("camera up");
                    }
                    if (key.Key == ConsoleKey.B || key.Key == ConsoleKey.DownArrow)
                    {
                        cameraTiltPosition -= cameraStep;
                        robot.CameraChangeTiltPosition(cameraTiltPosition);
                        Console.WriteLine("camera down");
                    }
                    if (key.Key == ConsoleKey.C || key.Key == ConsoleKey.RightArrow)
                    {
                        cameraPanPosition += cameraStep;
                        robot.CameraChangePanPosition(cameraPanPosition);
                        Console.WriteLine("camera right");
                    }
                    if (key.Key == ConsoleKey.D || key.Key == ConsoleKey.LeftArrow)
                    {
                        cameraPanPosition -= cameraStep;
                        robot.CameraChangePanPosition(cameraPanPosition);
                        Console.WriteLine("camera left");
                    }
                }

                
                if (key.Key == ConsoleKey.F)
                {
                    ledFront = !ledFront;
                    //robot.LedFront.Set(ledFront);
                }
                if (key.Key == ConsoleKey.R)
                {
                    ledRear = !ledRear;
                    //robot.LedRear.Set(ledRear);
                }
            }
        }
    }
}