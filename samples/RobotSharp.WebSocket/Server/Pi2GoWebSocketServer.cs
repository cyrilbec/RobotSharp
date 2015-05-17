using System;
using System.IO;
using RobotSharp.Camera;
using RobotSharp.Robot;

namespace RobotSharp.WebSocket.Server
{
    public class Pi2GoWebSocketServer : IDisposable
    {
        private WebSocketSharp.Server.WebSocketServer webSocket;

        private const string pathOperations = "/control";
        private const string pathImages = "/camera";

        private Pi2GoLiteState state;

        public Pi2GoWebSocketServer(IPi2GoLiteRobot robot, int port, IImageCapture imageCapture = null, TextWriter traceWriter = null)
        {
            state = new Pi2GoLiteState();

            webSocket = new WebSocketSharp.Server.WebSocketServer(port);
            webSocket.AddWebSocketService(pathOperations, () => new WebSocketOperationMessageReceiver(robot, state) {TraceWriter = traceWriter});

            if (imageCapture != null)
                webSocket.AddWebSocketService(pathImages, () => new WebSocketImageMessageSender(imageCapture));

            webSocket.Start();
        }

        public void Dispose()
        {
            webSocket.Stop();
        }
    }
}