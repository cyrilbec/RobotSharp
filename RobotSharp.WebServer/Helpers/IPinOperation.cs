using System;
using System.Collections.Generic;

namespace RobotSharp.WebServer.Helpers
{
    public interface IPinOperation
    {
        int PinNumber { get; set; }

        string Name { get; }
        string Description { get; }

        void Start();
        void Stop();

        IDictionary<string, Type> Parameters { get; }
        IDictionary<string, object> ParametersValues { get; set; }
    }
}