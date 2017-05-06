using LTHT.PeopleManagement.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace LTHT.PeopleManagement.Helpers
{
    /// <summary>
    /// Value converter to assist binding.
    /// </summary>
    public class ColoursToStringConverter : IValueConverter
    {
        /// <summary>
        /// Converts a list of colours to a comma seperated string representation
        /// </summary>
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            IEnumerable<Colour> colours = (IEnumerable<Colour>)value;
            if (colours.Any())
            {
                return string.Join(", ", colours.OrderBy(c => c.Name).Select(c => c.Name));
            }
            else
            {
                return "None";
            }
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}