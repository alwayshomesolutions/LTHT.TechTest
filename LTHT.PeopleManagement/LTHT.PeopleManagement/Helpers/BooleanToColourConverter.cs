using System;
using System.Globalization;

using Xamarin.Forms;

namespace LTHT.PeopleManagement.Helpers
{
    /// <summary>
    /// Value converter to assist binding
    /// </summary>
    public class BooleanToColourConverter : IValueConverter
    {
        /// <summary>
        /// Convert from Boolean to a Xamarin Colour
        /// </summary>
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            return (bool)value ? Color.Green : Color.Red;
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}