using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Mono.Unix.Native;
using RobotSharp.Gpio;
using RobotSharp.Gpio.SoftwarePwm;
using RobotSharp.Tools;

namespace RobotSharp.Pi2Go.Gpio
{
    public class DmaLinuxGpioPort : IGpioPort
    {
        private readonly IOperatingSystemService operatingSystemService;
        private readonly SoftwarePwm softwarePwm;

        private const int BCM2708_PERI_BASE_DEFAULT = 536870912; //0x2000000;
        private const int GPIO_BASE_OFFSET = 2097152; // = 0x20000;
        private const int PAGE_SIZE = 4 * 1024;
        private const int BLOCK_SIZE = 4 * 1024;
        private const int FSEL_OFFSET = 0; // 0x0000
        private const int SET_OFFSET = 7; // 0x001c / 4
        private const int CLR_OFFSET = 10; // 0x0028 / 4
        private const int PINLEVEL_OFFSET = 13; // 0x0034 / 4
        private const int PULLUPDN_OFFSET = 37; // 0x0094 / 4
        private const int PULLUPDNCLK_OFFSET = 38; // 0x0098 / 4
        private const int INPUT = 1; // is really 0 for control register!
        private const int OUTPUT = 0; // is really 1 for control register!
        private const int PUD_OFF = 0;
        private const int PUD_DOWN = 1;
        private const int PUD_UP = 2;
        private const int HIGH = 1;
        private const int LOW = 0;

        private const string DevMemFilePath = "/dev/mem";
        private const string DeviceTreePath = "/proc/device-tree/soc/ranges";
        private const string DeviceTreeAccessMode = "rb";

        private IntPtr gpioMap;
        private int devMemFileDescriptor;
        private bool setup;

        public DmaLinuxGpioPort(IOperatingSystemService operatingSystemService)
        {
            this.operatingSystemService = operatingSystemService;
            softwarePwm = new SoftwarePwm(operatingSystemService, this);
        }

        public void Setup()
        {
            if (setup) return;

            // get file descriptor for /dev/mem
            devMemFileDescriptor = Syscall.open(DevMemFilePath, OpenFlags.O_RDWR | OpenFlags.O_SYNC);

            var gpioMem = Stdlib.malloc(BLOCK_SIZE + (PAGE_SIZE - 1));

            var gpioMemInt = gpioMem.ToInt32();
            if ((gpioMemInt % PAGE_SIZE) == 1)
                gpioMem = IntPtr.Add(gpioMem, PAGE_SIZE - (gpioMemInt % PAGE_SIZE));

            var periBase = GetPeriBase();
            var gpioBase = (periBase == 0 ? BCM2708_PERI_BASE_DEFAULT : periBase) + GPIO_BASE_OFFSET;

            gpioMap = Syscall.mmap(gpioMem, BLOCK_SIZE, MmapProts.PROT_READ | MmapProts.PROT_WRITE,
                MmapFlags.MAP_SHARED, devMemFileDescriptor, gpioBase);

            setup = true;
        }

        public unsafe void Setup(int gpio, Direction direction, PullUpDown pullUpDown)
        {
            var gpioMapPtr = (int*)gpioMap;

            var offset = FSEL_OFFSET + (gpio / 10);
            var shift = (gpio % 10) * 3;

            SetPullUpDown(gpioMapPtr, gpio, pullUpDown);
            if (direction == Direction.Output)
                *(gpioMapPtr + offset) = (*(gpioMapPtr + offset) & ~(7 << shift)) | (1 << shift);
            else
                *(gpioMapPtr + offset) = (*(gpioMapPtr + offset) & ~(7 << shift));
        }

        public unsafe void Output(int gpio, HighLow value, long duration = -1)
        {
            // calculate offset
            var offset =
                value == HighLow.High
                    ? SET_OFFSET + (gpio / 32)
                    : CLR_OFFSET + (gpio / 32);

            // get unsafe base map pointer
            var basePtr = (int*)gpioMap;

            // write value to memory at (base + offset)
            *(basePtr + offset) = 1 << (gpio % 32);

            if (duration != -1)
                operatingSystemService.NanoSleep(duration);
        }

        public void Output(IEnumerable<GpioPortOperation> operations)
        {
            if (operations == null) throw new ArgumentNullException("operations");
            foreach (var operation in operations)
                Output(operation.Pin, operation.Value, operation.Duration);
        }

        public unsafe HighLow Input(int gpio)
        {
            var basePtr = (int*)gpioMap;
            var offset = PINLEVEL_OFFSET + (gpio / 32);
            var mask = (1 << gpio % 32);
            var value = *(basePtr + offset) & mask;

            return value == HIGH ? HighLow.High : HighLow.Low;
        }

        private unsafe int GetPinDirection(int gpio)
        {
            var gpioMapPtr = (int*)gpioMap;

            var offset = FSEL_OFFSET + (gpio / 10);
            var shift = (gpio % 10) * 3;
            var value = *(gpioMapPtr + offset);

            value >>= shift;
            value &= 7;

            // possible value for input : 0=input, 1=output, 4=alt0
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void SetPullUpDown(int* gpioMapPtr, int gpio, PullUpDown pud)
        {
            var clkOffset = PULLUPDNCLK_OFFSET + (gpio / 32);
            var shift = (gpio % 32);

            if (pud == PullUpDown.Down)
                *(gpioMapPtr + PULLUPDN_OFFSET) = (*(gpioMapPtr + PULLUPDN_OFFSET) & ~3) | PUD_DOWN;
            else if (pud == PullUpDown.Up)
                *(gpioMapPtr + PULLUPDN_OFFSET) = (*(gpioMapPtr + PULLUPDN_OFFSET) & ~3) | PUD_UP;
            else
                *(gpioMapPtr + PULLUPDN_OFFSET) &= ~3;

            Thread.SpinWait(150);
            *(gpioMapPtr + clkOffset) = 1 << shift;
            Thread.SpinWait(150);

            *(gpioMapPtr + PULLUPDN_OFFSET) &= ~3;
            *(gpioMapPtr + clkOffset) = 0;
        }

        private static int GetPeriBase()
        {
            var fp = Stdlib.fopen(DeviceTreePath, DeviceTreeAccessMode);

            if (fp == IntPtr.Zero) return 0;

            var buf = new byte[4];

            // get peri base from device tree
            Stdlib.fseek(fp, 4, SeekFlags.SEEK_SET);

            var periBase = 0;
            if (Stdlib.fread(buf, 1, (ulong)buf.Length, fp) == (ulong)buf.Length)
                periBase = buf[0] << 24 | buf[1] << 16 | buf[2] << 8 | buf[3] << 0;

            Stdlib.fclose(fp);

            return periBase;
        }

        ~DmaLinuxGpioPort()
        {
            Dispose(false);
        }

        private bool disposed;

        protected void Dispose(bool disposing)
        {
            if (disposed) return;

            if (!setup)
            {
                disposed = true;
                return;
            }

            if (disposing)
            {
                // dispose managed resources
            }

            // dispose unmanaged resources
            // unmap memory mapped file from /dev/mem from current process address space
            Syscall.munmap(gpioMap, BLOCK_SIZE);

            // release file descriptor for /dev/mem
            Syscall.close(devMemFileDescriptor);
            setup = false;

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void StartPwm(int gpio)
        {
            softwarePwm.StartPwm(gpio);
        }

        public void ControlPwm(int gpio, float? frequency, float? dutyCycle)
        {
            softwarePwm.ControlPwm(gpio, frequency, dutyCycle);
        }

        public void StopPwm(int gpio)
        {
            softwarePwm.StopPwm(gpio);
        }
    }
}