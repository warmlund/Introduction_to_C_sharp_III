using System.Globalization;
using System.Windows.Data;

namespace MediaPlayerPL
{
    public class UpDownExpanderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = (bool)value; //checks the bool value
            return isChecked ? "\uE70E" : "\uE70D"; //if true set symbol to down, otherwise up
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value == "\uE70E"; //convert back to down
        }
    }
}
