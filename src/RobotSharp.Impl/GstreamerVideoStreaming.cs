using RobotSharp.Pi2Go.Tools;

namespace RobotSharp.Devices.Impl.Camera
{
    public class GstreamerVideoStreaming
    {
        private bool started;

        public void Setup()
        {
            // nothing to do
        }

        public void Start()
        {
            var process =
                PosixUtils.CreateBashCommandProcess(
                    "raspivid -t 0 -w 480 -h 260 -fps 23 -b 800000 -p 0,0,640,480 -o - | gst-launch -v fdsrc !  h264parse ! rtph264pay config-interval=10 pt=96 ! udpsink host=192.168.0.3 port=5000");
            process.Start();
            process.Close();

            started = true;
        }

        public void Stop()
        {
            PosixUtils.ExecuteKillAll("raspivid");
            started = false;
        }

        public void Dispose()
        {
            if (started) Stop();
        }
    }
}