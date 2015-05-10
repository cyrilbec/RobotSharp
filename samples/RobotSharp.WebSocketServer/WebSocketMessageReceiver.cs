using WebSocketSharp;
using WebSocketSharp.Server;

namespace RobotSharp.WebSocketServer
{
    public class WebSocketMessageReceiver : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            
        }
    }

    public enum Operation : byte
    {
        // low level gpio ops
        Setup = 0,
        Output = 1,
        Input = 2,
        StartPwm = 3,
        ControlPwm = 4,
        StopPwm = 5,

        // call to device drivers
        Device = 6
    }
}