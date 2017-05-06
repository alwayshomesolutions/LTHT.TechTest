using System;
using System.Globalization;
using Xamarin.Forms;

namespace LTHT.PeopleManagement.Helpers
{
    /// <summary>
    /// Value converter to assist binding
    /// </summary>
    public class BooleanToYesNoConverter : IValueConverter
    {
        /// <summary>
        /// Convert source boolean to "Yes" or "No" text for use when binding
        /// </summary>
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            return (bool)value ? "Yes" : "No";
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}