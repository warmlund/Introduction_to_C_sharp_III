using System.Globalization;
using System.Windows.Data;

namespace MediaPlayerPL
{
    class EditFinishConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = (bool)value; //checks the bool value
            return isChecked ? "\uE73E" : "\uE70F"; //if true set symbol to checkmark, otherwise edit
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value == "\uE73E"; //convert back to edit
        }
    }
}
