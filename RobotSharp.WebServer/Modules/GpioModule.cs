using Nancy;
using RobotSharp.Pi2Go.Gpio;
using RobotSharp.Pi2Go.Tools;
using RobotSharp.WebServer.Helpers;

namespace RobotSharp.WebServer.Modules
{
    public class GpioModule : NancyModule
    {
        private static GpioHelper gpioHelper;
        private static GpioHelper GpioHelper
        {
            get
            {
                if (gpioHelper == null)
                {
                    var operationSystemService = new ClassicDotnetOperatingSystemService();
                    var gpioPort = new DmaLinuxGpioPort(operationSystemService);
                    gpioHelper = new GpioHelper(gpioPort);
                }

                return gpioHelper;
            }
        }

        public GpioModule()
        {
            Post["/"] = parameters =>
            {
                
                return View["Index", new
                {
                    GpioHelper.Pins,
                }];
            };
        }
    }
}