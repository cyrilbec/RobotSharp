using System;
using System.IO;
using RobotSharp.Robot;
using RobotSharp.WebSocket.Protocol;
using WebSocketSharp;
using WebSocketSharp.Server;
using ErrorEventArgs = WebSocketSharp.ErrorEventArgs;

namespace RobotSharp.WebSocket.Server
{
    internal class WebSocketMessageReceiver : WebSocketBehavior
    {
        private IPi2GoLiteRobot robot;
        private Pi2GoLiteState state;
        private TextWriter traceWriter;

        internal WebSocketMessageReceiver(IPi2GoLiteRobot robot, Pi2GoLiteState state)
        {
            this.robot = robot;
            this.state = state;
        }

        public TextWriter TraceWriter { set { traceWriter = value; } }

        #region connection management

        protected override void OnOpen()
        {
            if (traceWriter != null)
                traceWriter.WriteLine("client connected");

            base.OnOpen();
        }

        protected override void OnClose(CloseEventArgs e)
        {
            if (traceWriter != null)
                traceWriter.WriteLine("client disconnected");

            base.OnClose(e);
        }

        protected override void OnError(ErrorEventArgs e)
        {
            if (traceWriter != null)
                traceWriter.WriteLine("connection error : {0}", e.Message);

            base.OnError(e);
        }

        #endregion

        protected override void OnMessage(MessageEventArgs e)
        {
            // read operation
            var bytes = e.RawData;
            var operation = (Operation)bytes[0];

            if (operation == Operation.Move)
            {
                var leftSpeed = Convert.ToSingle((sbyte)bytes[1]);
                var rightSpeed = Convert.ToSingle((sbyte)bytes[2]);

                robot.Forward(leftSpeed, rightSpeed);
            }
            else if (operation == Operation.CameraMove)
            {
                var movement = (CameraMovement)bytes[1];
                var degrees = Convert.ToInt32((sbyte) bytes[2]);

                if (movement == CameraMovement.Pan) robot.CameraChangePanPosition(degrees);
                else robot.CameraChangeTiltPosition(degrees);
            }

            base.OnMessage(e);
        }
    }
}