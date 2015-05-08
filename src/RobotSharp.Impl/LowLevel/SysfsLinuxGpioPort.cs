using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RobotSharp.Gpio;
using RobotSharp.Pi2Go.Tools;
using RobotSharp.Tools;

namespace RobotSharp.Pi2Go.LowLevel
{
    public class SysfsLinuxGpioPort : IGpioPort
    {
        public IOperatingSystemService OperatingSystemService { get; set; }

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

        private bool setup;

        public void Setup()
        {
            if (setup) return;

            if (OperatingSystemService == null) throw new Exception("Operating system is mandatory");

            exportWriter =
                new StreamWriter(new FileStream(GpioExportPath, FileMode.Open, FileAccess.Write),
                    Encoding.ASCII);
            unexportWriter =
                new StreamWriter(new FileStream(GpioUnexportPath, FileMode.Open, FileAccess.Write),
                    Encoding.ASCII);

            setup = true;
        }

        public void SetupGpio(int gpio, Direction direction, PullUpDown pullUpDown)
        {
            Pin pin;

            if (pins.ContainsKey(gpio)) pin = pins[gpio];
            else
            {
                pin = new Pin();

                // export pin
                Export(gpio);

                // create direction streams
                pin.DirectionStream = new FileStream(string.Format(GpioPinDirectionPath, gpio), FileMode.Open,
                    FileAccess.ReadWrite, FileShare.ReadWrite);
                pin.DirectionWriter = new StreamWriter(pin.DirectionStream, Encoding.ASCII);
                pin.DirectionReader = new StreamReader(pin.DirectionStream, Encoding.ASCII);

                // create value streams
                pin.ValueStream = new FileStream(string.Format(GpioPinValuePath, gpio), FileMode.Open,
                    FileAccess.ReadWrite, FileShare.ReadWrite);
                pin.ValueWriter = new StreamWriter(pin.ValueStream, Encoding.ASCII);
                pin.ValueReader = new StreamReader(pin.ValueStream, Encoding.ASCII);

                pins.Add(gpio, pin);
            }

            // set pin direction
            PosixUtils.Echo(pin.DirectionWriter, direction == Direction.Input ? "in" : "out");

            // TODO : and pull up down ?
        }

        public void OutputGpio(int gpio, HighLow value)
        {
            var pin = pins[gpio];
            OutputGpio(pin, value);
        }

        private void OutputGpio(Pin pin, HighLow value)
        {
            PosixUtils.Echo(pin.ValueWriter, value == HighLow.High ? "1" : "0");
        }

        public HighLow InputGpio(int gpio)
        {
            var pin = pins[gpio];
            var value = PosixUtils.Cat(pin.ValueReader);

            return value == "1" ? HighLow.High : HighLow.Low;
        }

        private void Export(int gpio)
        {
            if (IsExported(gpio)) return;

            PosixUtils.Echo(exportWriter, gpio.ToString());
        }

        private void Unexport(int gpio)
        {
            if (!IsExported(gpio)) return;

            PosixUtils.Echo(unexportWriter, gpio.ToString());
        }

        private bool IsExported(int gpio)
        {
            var pinPath = string.Format(GpioPinPath, gpio);
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
    }
}