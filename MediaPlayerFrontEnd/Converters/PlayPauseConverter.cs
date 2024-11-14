using System.Globalization;
using System.Windows.Data;

namespace MediaPlayerPL
{
    /// <summary>
    /// this class is a converter for the togglebutton of the play pause button
    /// </summary>
    public class PlayPauseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = (bool)value; //checks the bool value
            return isChecked ? "\uF8AE" : "\uF5B0"; //if true set symbol to play, otherwise pause
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value == "\uF8AE"; //convert back to play
        }
    }
}
