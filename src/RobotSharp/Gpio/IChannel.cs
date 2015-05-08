using RobotSharp.Gpio;

namespace RobotSharp
{
    public interface IChannel
    {
        IGpioController GpioController { get; }
        Direction Direction { get; }
        void ChangeDirection(Direction direction, PullUpDown pullUpDown = PullUpDown.Off);
        HighLow Read();
        void Write(HighLow value);
        
        // TODO : mettre à disposition un event
    }
}