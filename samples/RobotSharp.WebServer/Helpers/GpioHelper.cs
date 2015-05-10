using System.Collections.Generic;
using RobotSharp.Gpio;

namespace RobotSharp.WebServer.Helpers
{
    public class GpioHelper
    {
        private IGpioPort gpioPort;

        #region Raspberry Pi 2 pins

        private readonly IList<Pin> pins = new List<Pin>()
        {
            new Pin() {Number = 1, Name = "3.3v", Description = "DC Power", Type = PinType.DcPower3_3},
            new Pin() {Number = 2, Name = "5v", Description = "DC Power", Type = PinType.DcPrower5},
            new Pin() {Number = 3, Name = "GPIO02", Description = "SDA1, I²C", Type = PinType.Other},
            new Pin() {Number = 4, Name = "5v", Description = "DC Power", Type = PinType.DcPrower5},
            new Pin() {Number = 5, Name = "GPIO03", Description = "SCL1, I²C", Type = PinType.Other},
            new Pin() {Number = 6, Name = "Ground", Description = null, Type = PinType.Ground},
            new Pin() {Number = 7, Name = "GPIO04", Description = "GPIO_GCLK", Type = PinType.Other},
            new Pin() {Number = 8, Name = "GPIO14", Description = "TXD0", Type = PinType.Other},
            new Pin() {Number = 9, Name = "Ground", Description = null, Type = PinType.Ground},
            new Pin() {Number = 10, Name = "GPIO15", Description = "RXD0", Type = PinType.Other},
            new Pin() {Number = 11, Name = "GPIO17", Description = "GPIO_END0", Type = PinType.Other},
            new Pin() {Number = 12, Name = "GPIO18", Description = "GPIO_GEN1", Type = PinType.Other},
            new Pin() {Number = 13, Name = "GPIO27", Description = "GPIO_GEN2", Type = PinType.Other},
            new Pin() {Number = 14, Name = "Ground", Description = null, Type = PinType.Ground},
            new Pin() {Number = 15, Name = "GPIO22", Description = "GPIO_GEN3", Type = PinType.Other},
            new Pin() {Number = 16, Name = "GPIO23", Description = "GPIO_GEN4", Type = PinType.Other},
            new Pin() {Number = 17, Name = "3.3v", Description = "DC Power", Type = PinType.DcPower3_3},
            new Pin() {Number = 18, Name = "GPIO24", Description = "GPIO_GEN5", Type = PinType.Other},
            new Pin() {Number = 19, Name = "GPIO10", Description = "SPI_MOSI", Type = PinType.Other},
            new Pin() {Number = 20, Name = "Ground", Description = null, Type = PinType.Ground},
            new Pin() {Number = 21, Name = "GPIO09", Description = "SPI_MISO", Type = PinType.Other},
            new Pin() {Number = 22, Name = "GPIO25", Description = "GPIO_GEN6", Type = PinType.Other},
            new Pin() {Number = 23, Name = "GPIO11", Description = "SPI_CLK", Type = PinType.Other},
            new Pin() {Number = 24, Name = "GPIO08", Description = "SPI_CE0_N", Type = PinType.Other},
            new Pin() {Number = 25, Name = "Ground", Description = null, Type = PinType.Ground},
            new Pin() {Number = 26, Name = "GPIO07", Description = "SPI_CE1_N", Type = PinType.Other},
            new Pin() {Number = 27, Name = "ID_SD", Description = "I²C ID EEPROM", Type = PinType.Other},
            new Pin() {Number = 28, Name = "ID_SC", Description = "I²C ID EEPROM", Type = PinType.Other},
            new Pin() {Number = 29, Name = "GPIO05", Description = null, Type = PinType.Other},
            new Pin() {Number = 30, Name = "Ground", Description = null, Type = PinType.Ground},
            new Pin() {Number = 31, Name = "GPIO06", Description = null, Type = PinType.Other},
            new Pin() {Number = 32, Name = "GPIO12", Description = null, Type = PinType.Other},
            new Pin() {Number = 33, Name = "GPIO13", Description = null, Type = PinType.Other},
            new Pin() {Number = 34, Name = "Ground", Description = null, Type = PinType.Ground},
            new Pin() {Number = 35, Name = "GPIO19", Description = null, Type = PinType.Other},
            new Pin() {Number = 36, Name = "GPIO16", Description = null, Type = PinType.Other},
            new Pin() {Number = 37, Name = "GPIO26", Description = null, Type = PinType.Other},
            new Pin() {Number = 38, Name = "GPIO20", Description = null, Type = PinType.Other},
            new Pin() {Number = 39, Name = "Ground", Description = null, Type = PinType.Ground},
            new Pin() {Number = 40, Name = "GPIO21", Description = null, Type = PinType.Other},
        }.AsReadOnly();

        #endregion
        
        public GpioHelper(IGpioPort gpioPort)
        {
            this.gpioPort = gpioPort;
        }

        public IList<Pin> Pins{get { return pins; }}
    }
}