using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MediaPlayerPL
{

    /// <summary>
    /// This is a class that converts a integer value to a visibility enumeration
    /// </summary>
    internal class CountVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int count) //checks if value is an integer
            {
                return count<1 ? Visibility.Visible : Visibility.Collapsed; //returns visible if integer is less than 1
            }

            return Visibility.Collapsed; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Visibility.Collapsed; //converts back to collapsed
        }
    }
}
