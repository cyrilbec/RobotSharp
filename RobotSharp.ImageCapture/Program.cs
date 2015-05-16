using System;

namespace RobotSharp.ImageCapture
{
    public class Program
    {
        static void Main(string[] args)
        {
            var imageCapture = new ImageCapture();
            imageCapture.FrameAvailable += imageCapture_FrameAvailable;

            imageCapture.Start();

            Console.ReadKey(true);
            Console.WriteLine("done.");
        }

        static void imageCapture_FrameAvailable(object sender, FrameAvalaibleEventArgs e)
        {
            Console.WriteLine("frame available : {0}", e.FrameNumber);
        }
    }
}