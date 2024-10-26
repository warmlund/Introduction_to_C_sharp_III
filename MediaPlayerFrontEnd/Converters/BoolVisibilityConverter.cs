using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MediaPlayerPL
{

    /// <summary>
    /// This is a class that converts a boolean to a visibility enumeration
    /// </summary>
    internal class BoolVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is bool boolValue) //checks if the value is a booelan
            {
                return boolValue ? Visibility.Visible : Visibility.Collapsed; //returns visible if true, else collapsed
            }

            return Visibility.Collapsed; // Default to Collapsed if the value is not a boolean.
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility) //checks if the value is a visibility enumeration
            {
                return visibility == Visibility.Visible; //returns true if visible
            }

            return false; // Return false if the value is not a Visibility type.
        }
    }
}

