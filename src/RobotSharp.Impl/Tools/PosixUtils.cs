using System.Diagnostics;
using System.IO;
using System.Text;

namespace RobotSharp.Pi2Go.Tools
{
    public static class PosixUtils
    {
        public static void ExecuteKillAll(string processName)
        {
            var process = CreateBashCommandProcess("killall " + processName);
            process.Start();
            process.Close();
        }

        public static Process CreateBashCommandProcess(string cmd)
        {
            var processStartInfo = new ProcessStartInfo { FileName = "/bin/bash", Arguments = "-c \"" + cmd + "\"" };
            var process = new Process() { StartInfo = processStartInfo };

            return process;
        }

        public static void EchoToFile(string fileName, string str)
        {
            using (var streamWriter = OpenFileForEcho(fileName))
                Echo(streamWriter, str);
        }

        private static StreamWriter OpenFileForEcho(string fileName)
        {
            return new StreamWriter(new FileStream(fileName, FileMode.Open, FileAccess.Write, FileShare.ReadWrite), Encoding.ASCII);
        }

        public static void Echo(StreamWriter streamWriter, string str)
        {
            streamWriter.BaseStream.Seek(0, SeekOrigin.Begin);
            streamWriter.Write(str + "\n");
            streamWriter.Flush();
        }

        private static StreamReader OpenFileForCat(string fileName)
        {
            return new StreamReader(new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
        }

        public static string CatFile(string fileName)
        {
            using (var streamReader = OpenFileForCat(fileName))
                return Cat(streamReader);
        }

        public static string Cat(StreamReader streamReader)
        {
            streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            return streamReader.ReadToEnd();
        }
    }
}