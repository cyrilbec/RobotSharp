namespace RobotSharp.Camera
{
    public delegate void FrameAvailableEventHandler(object sender, FrameAvalaibleEventArgs e);

    public interface IImageCapture
    {
        event FrameAvailableEventHandler FrameAvailable;
        void Start();
    }
}