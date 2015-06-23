using System;
using System.Collections.ObjectModel;

namespace RobotSharp.Control
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            Users = new ObservableCollection<User>()
            {
                new User() {Lastname="Beccavin", Firstname="Cyril", Birthday=new DateTime(1982,7,29) },
                new User() {Lastname="Franco", Firstname="Capucine", Birthday=new DateTime(1982,7,11) },
                new User() {Lastname="Dupont", Firstname="Jean", Birthday=new DateTime(1947, 10,24) },
            };
        }

        public ObservableCollection<User> Users { get; set; }
    }
}
