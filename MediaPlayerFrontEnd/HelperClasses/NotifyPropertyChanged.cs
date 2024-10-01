using System.ComponentModel;

namespace MediaPlayerPL
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {

        //Event to notify if a proprety has changed
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the propertychanged event for a named property
        /// This method is called if the property has changed
        /// </summary>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
