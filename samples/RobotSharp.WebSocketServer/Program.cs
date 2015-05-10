using System;

namespace RobotSharp.WebSocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var wssv = new WebSocketSharp.Server.WebSocketServer(81);
            wssv.AddWebSocketService<WebSocketMessageReceiver>("/gpioport");
            wssv.Start();

            Console.WriteLine("Press any key to quit.");
            Console.ReadKey(true);
            wssv.Stop();
        }
    }
}