using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MediaPlayerPL
{
    public class PlayPauseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = (bool)value;
            return isChecked ? "\uF8AE" : "\uF5B0"; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value == "\uF8AE"; 
        }
    }
}
