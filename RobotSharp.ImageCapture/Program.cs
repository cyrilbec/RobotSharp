using System;
using System.IO;
using System.Security.Cryptography;
using RobotSharp.Images;

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
            Console.WriteLine("data = {0}", ToBase64(e.Stream));
        }

        static string ToBase64(Stream stream)
        {
            stream.Position = 0;
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            return Convert.ToBase64String(bytes, Base64FormattingOptions.None);
        }
    }
}