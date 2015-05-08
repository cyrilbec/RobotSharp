using RobotSharp.Tools;
using System;
using System.Collections.Generic;
using System.Threading;
using Mono.Unix.Native;

namespace RobotSharp.Pi2Go.Tools
{
    public class ClassicDotnetOperatingSystemService : IOperatingSystemService
    {
        private static readonly IDictionary<int, IDisposable> threadables =
            new Dictionary<int, IDisposable>();

        private static int nextThreadableUniqueId = 0;
        private static object threadablesSyncRoot = new object();

        public ILoopThread<T> CreateLoopThread<T>(Action<T> whatToDo, int stopTimeout = 1000)
        {
            ClassicDotnetThreadable<T> threadable;
            lock (threadablesSyncRoot)
            {
                var uniqueId = GetNewUniqueId();
                threadable = new ClassicDotnetThreadable<T>(this, uniqueId, whatToDo, stopTimeout);
                threadables.Add(uniqueId, threadable);
            }

            return threadable;
        }

        internal void RemoveLoopThread(int uniqueId)
        {
            lock (threadablesSyncRoot)
            {
                threadables.Remove(uniqueId);
            }
        }

        private int GetNewUniqueId()
        {
            if (nextThreadableUniqueId == int.MaxValue) nextThreadableUniqueId = 0;

            while (threadables.ContainsKey(nextThreadableUniqueId))
                nextThreadableUniqueId++;

            return nextThreadableUniqueId;
        }

        private void DisposeAllLoopThreads()
        {
            while (threadables.Values.Count > 0)
                threadables[0].Dispose();

            nextThreadableUniqueId = 0;
        }

        public void Sleep(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        // thread static to avoid memory allocation
        [ThreadStatic]
        private static Timespec durationTimespec;
        [ThreadStatic]
        private static Timespec remTimespec;

        public void NanoSleep(long nanoseconds)
        {
            durationTimespec.tv_nsec = nanoseconds;

            Syscall.nanosleep(ref durationTimespec, ref remTimespec);
        }

        public void Dispose()
        {
            DisposeAllLoopThreads();
        }

        ~ClassicDotnetOperatingSystemService()
        {
            Dispose();
        }
    }
}