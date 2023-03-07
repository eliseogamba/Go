using System;
using System.Globalization;
using Xamarin.Forms;

namespace Go.Converters
{
    public class IsEmptyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null)
            {
                if(string.IsNullOrEmpty((string)value))
                {
                    return true;
                }

                return false;
            }

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}