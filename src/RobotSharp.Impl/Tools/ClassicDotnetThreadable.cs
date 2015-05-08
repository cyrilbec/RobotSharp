using System;
using System.Collections.Concurrent;
using System.Threading;
using RobotSharp.Tools;

namespace RobotSharp.Pi2Go.Tools
{
    internal class ClassicDotnetThreadable<T> : ILoopThread<T>
    {
        private int uniqueId;
        private Action<T> whatToDo;

        private Thread thread;
        internal volatile bool running;
        private ManualResetEventSlim manualResetEventSlim;
        private bool started;
        private int stopTimeout;
        private ClassicDotnetOperatingSystemService service;

        private readonly ConcurrentQueue<T> queue = new ConcurrentQueue<T>();

        public ClassicDotnetThreadable(ClassicDotnetOperatingSystemService service, int uniqueId, Action<T> whatToDo, int stopTimeout)
        {
            this.service = service;
            this.uniqueId = uniqueId;
            this.whatToDo = whatToDo;
            this.stopTimeout = stopTimeout;
        }

        private void StartSimpleLoop()
        {
            var defaultValue = default(T);
            while (running) whatToDo(defaultValue);
        }

        private void StartLoopWithSignalAwaiting()
        {
            while (running)
            {
                manualResetEventSlim.Wait();

                if (!running) return;

                T item;
                while (queue.TryDequeue(out item))
                    whatToDo(item);

                manualResetEventSlim.Reset();
            }
        }

        public int UniqueId
        {
            get { return uniqueId; }
        }

        public void Start(bool waitForSignal)
        {
            if (started) return;

            started = true;
            running = true;

            if (waitForSignal)
            {
                thread = new Thread(StartLoopWithSignalAwaiting);
                manualResetEventSlim = new ManualResetEventSlim(false);
            }
            else
                thread = new Thread(StartSimpleLoop);

            thread.Start();
        }

        public void Enqueue(T item)
        {
            queue.Enqueue(item);
        }

        public void Signal(T item)
        {
            Enqueue(item);
            Signal();
        }

        public void Signal()
        {
            if (manualResetEventSlim == null) return;

            manualResetEventSlim.Set();
        }

        public void Stop()
        {
            if (!started) return;

            running = false;

            if (manualResetEventSlim != null) manualResetEventSlim.Set();
            thread.Join(stopTimeout);

            if (thread.ThreadState == ThreadState.Running)
            {
                try { thread.Abort(); }
                catch (Exception) { }
            }
        }

        ~ClassicDotnetThreadable()
        {
            Dispose(false);
        }

        private bool disposed;

        protected void Dispose(bool isDisposing)
        {
            // test if not already disposed
            if (disposed) return;

            // if not started, no need to release any resources
            if (!started)
            {
                disposed = true;
                return;
            }

            if (isDisposing)
            {
                // dispose managed resources
                Stop();

                if (manualResetEventSlim != null) manualResetEventSlim.Dispose();
                if (service != null) service.RemoveLoopThread(uniqueId);
            }

            // dispose unmanaged resources

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}