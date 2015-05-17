using System;
using RobotSharp.Camera;
using RobotSharp.Utils;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace RobotSharp.WebSocket.Server
{
    internal class WebSocketImageMessageSender : WebSocketBehavior
    {
        private IImageCapture imageCapture;
        private bool open = false;

        internal WebSocketImageMessageSender(IImageCapture imageCapture)
        {
            this.imageCapture = imageCapture;
            imageCapture.FrameAvailable += imageCapture_FrameAvailable;
            imageCapture.Start();
        }

        void imageCapture_FrameAvailable(object sender, FrameAvalaibleEventArgs e)
        {
            if (!open) return;

            var data = Util.ToBase64(e.Stream);
            Send(data);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            base.OnMessage(e);
        }

        protected override void OnOpen()
        {
            open = true;
            base.OnOpen();
        }

        protected override void OnClose(CloseEventArgs e)
        {
            open = false;
            base.OnClose(e);
        }
    }
}