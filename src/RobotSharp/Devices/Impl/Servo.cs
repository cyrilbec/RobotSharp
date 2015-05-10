using RobotSharp.Gpio;
using RobotSharp.Tools;

namespace RobotSharp.Devices.Impl
{
    public class Servo : IServo
    {
        // TODO : this class need to be more generic

        public IOperatingSystemService OperatingSystemService { get; set; }
        public IGpioPort GpioPort { get; set; }

        private int pin;
        private int degrees = 0;
        private ILoopThread<int> loopThread;

        private GpioPortOperation[] operations;

        private bool setup;

        public Servo(int pin)
        {
            this.pin = pin;
        }

        public void Setup()
        {
            if (setup) return;

            GpioPort.Setup(pin, Direction.Output, PullUpDown.Off);
            operations = new[]
            {new GpioPortOperation(pin) {Value = HighLow.High}, new GpioPortOperation(pin) {Value = HighLow.Low}};

            // start thread
            loopThread = OperatingSystemService.CreateLoopThread<int>(DoMove);
            loopThread.Start(true);

            setup = true;
        }

        public void Move(int degrees)
        {
            if (this.degrees == degrees) return;

            this.degrees = degrees;

            // send signal to thread
            loopThread.Signal(degrees);
        }
        
        // frequency in 10 us
        private const int frequency = 2000;//20 * 100;

        private void DoMove(int degrees)
        {
            // calculate width of impulsion from degrees
            var width = 50 + ((90 - degrees)*200/180);

            // on during "onDuration" microseconds
            operations[0].Duration = width*10000;

            // off during "offDuration" microseconds
            operations[1].Duration = (frequency - width)*10000;

            // launch opérations in batch mode
            GpioPort.Output(operations);
        }

        public void Dispose()
        {
            if (!setup) return;

            loopThread.Dispose();

            setup = false;
        }
    }
}