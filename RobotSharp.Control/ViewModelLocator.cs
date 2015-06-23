namespace RobotSharp.Control
{
    public class ViewModelLocator
    {
        private readonly MainViewModel main = new MainViewModel();

        public MainViewModel Main
        {
            get
            {
                return main;
            }
        }
    }
}
