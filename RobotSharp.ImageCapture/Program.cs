using System;
using RobotSharp.Camera;
using RobotSharp.Utils;

namespace RobotSharp.ImageCapture
{
    public class Program
    {
        static void Main(string[] args)
        {
            var imageCapture = new ImageCapture();
            imageCapture.FrameAvailable += imageCapture_FrameAvailable;

            imageCapture.Start();

            Console.WriteLine("Press any key to quit.");
            Console.ReadKey(true);
        }

        static void imageCapture_FrameAvailable(object sender, FrameAvalaibleEventArgs e)
        {
            Console.WriteLine("frame available : {0}", e.FrameNumber);
            Console.WriteLine("data = {0}", Util.ToBase64(e.Stream));
        }
    }
}