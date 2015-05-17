using System.IO;

namespace RobotSharp.Camera
{
    public class FrameAvalaibleEventArgs
    {
        public FrameAvalaibleEventArgs(Stream stream, int frameNumber)
        {
            this.stream = stream;
            this.frameNumber = frameNumber;
        }

        private Stream stream;

        public Stream Stream
        {
            get { return stream; }
        }

        private int frameNumber;

        public int FrameNumber
        {
            get { return frameNumber; }
        }
    }
}