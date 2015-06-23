using System;
using System.ComponentModel;

namespace RobotSharp.Control
{
    public class User : INotifyPropertyChanged
    {
        protected void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged == null) return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private string firstname;
        public string Firstname
        {
            get { return firstname; }
            set
            {
                if (value == firstname) return;
                firstname = value;
                NotifyPropertyChange("Firstname");
            }
        }

        private string lastname;
        public string Lastname
        {
            get { return lastname; }
            set
            {
                if (value == lastname) return;
                lastname = value;
                NotifyPropertyChange("Lastname");
            }
        }

        private DateTime birthday;
        public DateTime Birthday
        {
            get { return birthday; }
            set
            {
                if (value == birthday) return;
                birthday = value;
                NotifyPropertyChange("Birthday");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
