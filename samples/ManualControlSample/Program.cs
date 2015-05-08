﻿using System;
using RobotSharp.Gpio;
using RobotSharp.Pi2Go.LowLevel;
using RobotSharp.Pi2Go.Tools;
using RobotSharp.Robot;
using RobotSharp.Tools;

namespace ManualControlSample
{
    class Program
    {
        private static Pi2GoLiteRobot robot;

        static void Main(string[] args)
        {
            try
            {
                IOperatingSystemService operatingSystemService = new ClassicDotnetOperatingSystemService();
                IGpioPort gpioPort = 
                    new DmaLinuxGpioPort();
                    //new SysfsLinuxGpioPort() {OperatingSystemService = operatingSystemService};

                robot = RobotBuilder.BuildPi2GoLite(operatingSystemService, gpioPort);

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

        static void ManualControl()
        {
            int speed = 80;
            Console.WriteLine("press q to quit.");

            // move camera to initial position
            robot.CameraChangePanPosition(0);
            robot.CameraChangeTiltPosition(0);

            // light off
            robot.LedFront.Set(false);
            robot.LedRear.Set(false);

            bool ledFront = false, ledRear = false;

            ConsoleKeyInfo key;
            while ((key = Console.ReadKey(true)).Key != ConsoleKey.Q)
            {
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
                        Console.WriteLine("distance = {0}cm", robot.Sonar.Distance());
                    }
                }
                else
                {
                    Console.WriteLine("shift  + " + key.Key);
                    // if shift move camera
                    if (key.Key == ConsoleKey.A)
                    {
                        robot.CameraUp();
                        Console.WriteLine("camera up");
                    }
                    if (key.Key == ConsoleKey.B)
                    {
                        robot.CameraDown();
                        Console.WriteLine("camera down");
                    }
                    if (key.Key == ConsoleKey.C)
                    {
                        robot.CameraRight();
                        Console.WriteLine("camera right");
                    }
                    if (key.Key == ConsoleKey.D)
                    {
                        robot.CameraLeft();
                        Console.WriteLine("camera left");
                    }
                }

                if (key.Key == ConsoleKey.Add && speed < 100)
                {
                    speed += 10;
                    Console.WriteLine("speed is now at {0}%", speed);
                }
                if (key.Key == ConsoleKey.Subtract && speed > 0)
                {
                    speed -= 10;
                    Console.WriteLine("speed is now at {0}%", speed);
                }
                if (key.Key == ConsoleKey.F)
                {
                    ledFront = !ledFront;
                    robot.LedFront.Set(ledFront);
                }
                if (key.Key == ConsoleKey.R)
                {
                    ledRear = !ledRear;
                    robot.LedRear.Set(ledRear);
                }
            }
        }
    }
}