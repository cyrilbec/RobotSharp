using System;

namespace RobotSharp.Tools
{
    public interface ILoopThread<T> : IDisposable
    {
        int UniqueId { get; }
        void Start(bool waitForSignal);
        void Enqueue(T item);
        void Signal();
        void Signal(T item);
        void Stop();
    }
}