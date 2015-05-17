using System;
using System.IO;

namespace RobotSharp.Utils
{
    public static class Util
    {
        public static string ToBase64(Stream stream)
        {
            stream.Position = 0;
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            return Convert.ToBase64String(bytes);
        }
    }
}