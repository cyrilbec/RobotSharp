using System;

namespace RobotSharp.Tools
{
    public interface IOperatingSystemService : IDisposable
    {
        // threads management
        ILoopThread<T> CreateLoopThread<T>(Action<T> whatToDo, int stopTimeout = 1000);
        void Sleep(int milliseconds);
        void NanoSleep(long nanoseconds);
    }
}