using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using RobotSharp.Camera;
using RobotSharp.Pi2Go.Tools;

namespace RobotSharp.ImageCapture
{
    public class ImageCapture
    {
        public delegate void FrameAvailableEventHandler(object sender, FrameAvalaibleEventArgs e);

        public event FrameAvailableEventHandler FrameAvailable;

        // TODO : add event "no more frame"

        private int frameCount = 0;
        private Thread frameParsingThread;
        private Process imageGeneratorProcess;

        public void Start()
        {
            frameParsingThread = new Thread(ParseFrames);
            frameParsingThread.Start();
        }

        private void ParseFrames()
        {
            // jpegs stream input
            var jpegsStream = GetJpegsInputStream();

            // create stream for the first frame
            var frameStream = CreateNewFrameOuputStream();

            var first = true;
            byte precByte = 0;
            foreach (var currentByte in ReadBytes(jpegsStream))
            {
                // write byte to the frame stream
                frameStream.WriteByte(currentByte);

                if (first) first = false;

                // check if is the end of frame
                else if (precByte == 255 && currentByte == 217)
                {
                    // trigger event
                    if (FrameAvailable != null)
                        FrameAvailable(this, new FrameAvalaibleEventArgs(frameStream, frameCount));

                    frameStream = CreateNewFrameOuputStream();
                }

                precByte = currentByte;
            }
        }

        private Stream CreateNewFrameOuputStream()
        {
            frameCount++;
            return new MemoryStream();
        }

        private Stream GetJpegsInputStream()
        {
            // stdout from pipe
            // sample command line :
            // raspistill -n -e jpg -w 640 -h 480 -q 75 -x none -tl 0 -t 10000 -o - | mono RobotSharp.ImageCapture.exe
            //return Console.OpenStandardInput();

            // start a process and get his stdout's
            imageGeneratorProcess = PosixUtils.CreateBashCommandProcess("raspistill -n -e jpg -w 640 -h 480 -q 75 -x none -tl 0 -t 10000 -o -");
            imageGeneratorProcess.Start();
            return imageGeneratorProcess.StandardOutput.BaseStream;
        }

        private IEnumerable<byte> ReadBytes(Stream jpegStream)
        {
            using (var stream = jpegStream)
            {
                var buffer = new byte[8 * 1024];
                int len;
                while ((len = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    for (var i = 0; i < len; i++)
                        yield return buffer[i];
                }
            }
        }
    }
}