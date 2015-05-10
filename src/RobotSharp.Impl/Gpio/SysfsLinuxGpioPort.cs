using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using RobotSharp.Gpio;
using RobotSharp.Gpio.SoftwarePwm;
using RobotSharp.Pi2Go.Tools;
using RobotSharp.Tools;

namespace RobotSharp.Pi2Go.Gpio
{
    public class SysfsLinuxGpioPort : IGpioPort
    {
        private readonly IOperatingSystemService operatingSystemService;
        private readonly SoftwarePwm softwarePwm;

        private StreamWriter exportWriter;
        private StreamWriter unexportWriter;

        private class Pin : IDisposable
        {
            // streams for read / write value on pin
            public FileStream ValueStream;
            public StreamWriter ValueWriter;
            public StreamReader ValueReader;

            // streams for read / write direction on pin
            public FileStream DirectionStream;
            public StreamWriter DirectionWriter;
            public StreamReader DirectionReader;

            public void Dispose()
            {
                ValueStream.Dispose();
                DirectionStream.Dispose();
            }
        }

        private IDictionary<int, Pin> pins = new Dictionary<int, Pin>();

        private const string GpioBasePath = "/sys/class/gpio";
        private static readonly string GpioExportPath = string.Concat(GpioBasePath, "/export");
        private static readonly string GpioUnexportPath = string.Concat(GpioBasePath, "/unexport");
        private static readonly string GpioPinPath = string.Concat(GpioBasePath, "/gpio{0}");
        private static readonly string GpioPinDirectionPath = string.Concat(GpioBasePath, "/gpio{0}/direction");
        private static readonly string GpioPinValuePath = string.Concat(GpioBasePath, "/gpio{0}/value");

        public SysfsLinuxGpioPort(IOperatingSystemService operatingSystemService)
        {
            this.operatingSystemService = operatingSystemService;
            softwarePwm = new SoftwarePwm(operatingSystemService, this);
        }

        private bool setup;

        public void Setup()
        {
            if (setup) return;

            exportWriter =
                new StreamWriter(new FileStream(GpioExportPath, FileMode.Open, FileAccess.Write),
                    Encoding.ASCII);
            unexportWriter =
                new StreamWriter(new FileStream(GpioUnexportPath, FileMode.Open, FileAccess.Write),
                    Encoding.ASCII);

            setup = true;
        }

        public void Setup(int pin, Direction direction, PullUpDown pullUpDown)
        {
            Pin pinObj;

            if (pins.ContainsKey(pin)) pinObj = pins[pin];
            else
            {
                pinObj = new Pin();

                // export pin
                Export(pin);

                // create direction streams
                pinObj.DirectionStream = new FileStream(string.Format(GpioPinDirectionPath, pin), FileMode.Open,
                    FileAccess.ReadWrite, FileShare.ReadWrite);
                pinObj.DirectionWriter = new StreamWriter(pinObj.DirectionStream, Encoding.ASCII);
                pinObj.DirectionReader = new StreamReader(pinObj.DirectionStream, Encoding.ASCII);

                // create value streams
                pinObj.ValueStream = new FileStream(string.Format(GpioPinValuePath, pin), FileMode.Open,
                    FileAccess.ReadWrite, FileShare.ReadWrite);
                pinObj.ValueWriter = new StreamWriter(pinObj.ValueStream, Encoding.ASCII);
                pinObj.ValueReader = new StreamReader(pinObj.ValueStream, Encoding.ASCII);

                pins.Add(pin, pinObj);
            }

            // set pin direction
            PosixUtils.Echo(pinObj.DirectionWriter, direction == Direction.Input ? "in" : "out");

            // TODO : and pull up down ?
        }

        public void Output(int pin, HighLow value, long duration = -1)
        {
            var pinObj = pins[pin];
            OutputGpio(pinObj, value);
        }

        private void OutputGpio(Pin pin, HighLow value, long duration = -1)
        {
            PosixUtils.Echo(pin.ValueWriter, value == HighLow.High ? "1" : "0");

            if (duration != -1)
                operatingSystemService.NanoSleep(duration);
        }

        public void Output(IEnumerable<GpioPortOperation> operations)
        {
            if (operations == null) throw new ArgumentNullException("operations");
            foreach (var operation in operations)
                Output(operation.Pin, operation.Value, operation.Duration);
        }

        public HighLow Input(int pin)
        {
            var pinObj = pins[pin];
            var value = PosixUtils.Cat(pinObj.ValueReader);

            return value == "1" ? HighLow.High : HighLow.Low;
        }

        private void Export(int pin)
        {
            if (IsExported(pin)) return;

            PosixUtils.Echo(exportWriter, pin.ToString());
        }

        private void Unexport(int pin)
        {
            if (!IsExported(pin)) return;

            PosixUtils.Echo(unexportWriter, pin.ToString());
        }

        private bool IsExported(int pin)
        {
            var pinPath = string.Format(GpioPinPath, pin);
            return Directory.Exists(pinPath);
        }

        ~SysfsLinuxGpioPort()
        {
            Dispose(false);
        }

        private bool disposed;

        protected void Dispose(bool isDisposing)
        {
            if (disposed) return;

            if (!setup)
            {
                disposed = true;
                return;
            }

            if (isDisposing)
            {
                // dispose managed resources
                // set all pins to input and unexport
                foreach (var pinKeyValue in pins)
                {
                    Unexport(pinKeyValue.Key);
                    pinKeyValue.Value.Dispose();
                }

                exportWriter.Dispose();
                unexportWriter.Dispose();
                exportWriter = null;
                unexportWriter = null;
            }

            // dispose unmanaged resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void StartPwm(int pin)
        {
            softwarePwm.StartPwm(pin);
        }

        public void ControlPwm(int pin, float? frequency, float? dutyCycle)
        {
            softwarePwm.ControlPwm(pin, frequency, dutyCycle);
        }

        public void StopPwm(int pin)
        {
            softwarePwm.StopPwm(pin);
        }

        #region dummy async implementations

        public async Task SetupAsync(int pin, Direction direction, PullUpDown pullUpDown)
        {
            Setup(pin, direction, pullUpDown);
        }

        public async Task OutputAsync(int pin, HighLow value, long duration = -1)
        {
            Output(pin, value, duration);
        }

        public async Task OutputAsync(IEnumerable<GpioPortOperation> operations)
        {
            Output(operations);
        }

        public async Task<HighLow> InputAsync(int pin)
        {
            return Input(pin);
        }

        public async Task StartPwmAsync(int pin)
        {
            StartPwm(pin);
        }

        public async Task ControlPwmAsync(int pin, float? frequency, float? dutyCycle)
        {
            ControlPwm(pin, frequency, dutyCycle);
        }

        public async Task StopPwmAsync(int pin)
        {
            StopPwm(pin);
        }

        #endregion
    }
}