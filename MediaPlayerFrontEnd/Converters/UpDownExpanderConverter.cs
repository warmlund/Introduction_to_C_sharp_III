using System.Globalization;

namespace MediaPlayerPL.Converters
{
    class UpDownExpanderConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = (bool)value; //checks the bool value
            return isChecked ? "\uE70D" : "\uE70E"; //if true set symbol to down, otherwise up
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value == "\uE70D"; //convert back to down
        }
    }
}
