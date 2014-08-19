using System;
using System.ComponentModel;

namespace Betolto.Model
{
    public class LogItem : INotifyPropertyChanged
    {
        private DateTime _messageDateTime;
        public DateTime MessageDateTime
        {
            get { return _messageDateTime; }
            set
            {
                _messageDateTime = value;
                OnPropertyChanged("MessageDateTime");
            }
        }

        private string _message;
        public string  Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
