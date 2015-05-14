using System;
using System.IO;
using RobotSharp.Robot;
using RobotSharp.WebSocket.Protocol;

namespace RobotSharp.WebSocket.Client
{
    public class WebSocketClientPi2GoLiteRobot : IPi2GoLiteRobot
    {
        private WebSocketSharp.WebSocket webSocket;
        private string url;
        private bool setup;
        private TextWriter traceWriter;

        public WebSocketClientPi2GoLiteRobot(string url, TextWriter traceWriter = null)
        {
            this.url = url;
            this.traceWriter = traceWriter;
        }

        #region web socket events

        void webSocket_OnClose(object sender, WebSocketSharp.CloseEventArgs e)
        {
            if(traceWriter != null)
                traceWriter.WriteLine("connection closed");
        }

        void webSocket_OnError(object sender, WebSocketSharp.ErrorEventArgs e)
        {
            if (traceWriter != null)
                traceWriter.WriteLine("connection error {0}", e.Message);
        }

        void webSocket_OnMessage(object sender, WebSocketSharp.MessageEventArgs e)
        {
            if (traceWriter != null)
                traceWriter.WriteLine("message received");
        }

        #endregion

        public void Setup()
        {
            if (setup) return;

            webSocket = new WebSocketSharp.WebSocket(url);

            webSocket.OnMessage += webSocket_OnMessage;
            webSocket.OnError += webSocket_OnError;
            webSocket.OnClose += webSocket_OnClose;

            webSocket.Connect();
        }

        public void Forward(float speed)
        {
            SendMove(speed, speed);
        }

        public void Forward(float leftSpeed, float rightSpeed)
        {
            SendMove(leftSpeed, rightSpeed);
        }

        public void Reverse(float speed)
        {
            speed *= -1;
            SendMove(speed, speed);
        }

        public void SpinLeft(float speed)
        {
            SendMove(speed, speed*-1);
        }

        public void SpinRight(float speed)
        {
            SendMove(speed*-1, speed);
        }

        public void Stop()
        {
            SendMove(0, 0);
        }

        public void CameraChangePanPosition(int degrees)
        {
            SendCameraPosition(CameraMovement.Pan, degrees);
        }

        public void CameraChangeTiltPosition(int degrees)
        {
            SendCameraPosition(CameraMovement.Tilt, degrees);
        }

        public void Dispose()
        {
            webSocket.Dispose();
        }

        private void SendMove(float leftSpeed, float rightSpeed)
        {
            var leftSByte = Convert.ToSByte(leftSpeed);
            var rightSByte = Convert.ToSByte(rightSpeed);

            webSocket.Send(new[] {(byte) Operation.Move, (byte) leftSByte, (byte) rightSByte});
        }

        private void SendCameraPosition(CameraMovement movement, int degrees)
        {
            var degreesSByte = Convert.ToSByte(degrees);
            webSocket.Send(new[] {(byte) Operation.CameraMove, (byte) movement, (byte) degreesSByte});
        }
    }
}