using System;
using System.IO;
using RobotSharp.Robot;

namespace RobotSharp.WebSocket.Server
{
    public class Pi2GoWebSocketServer : IDisposable
    {
        private WebSocketSharp.Server.WebSocketServer webSocket;

        private const string path = "/pi2golite";

        private Pi2GoLiteState state;

        public Pi2GoWebSocketServer(IPi2GoLiteRobot robot, int port, TextWriter traceWriter = null)
        {
            state = new Pi2GoLiteState();

            webSocket = new WebSocketSharp.Server.WebSocketServer(port);
            webSocket.AddWebSocketService(path, () => new WebSocketMessageReceiver(robot, state) {TraceWriter = traceWriter});
            webSocket.Start();
        }

        public void Dispose()
        {
            webSocket.Stop();
        }
    }
}