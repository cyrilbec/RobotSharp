using System;
using Nancy.Hosting.Self;

namespace RobotSharp.WebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new NancyHost(new Uri("http://localhost:81")))
            {
                host.Start();
                
                Console.WriteLine("Press any key to quit.");

                Console.ReadLine();
            }
        }
    }
}