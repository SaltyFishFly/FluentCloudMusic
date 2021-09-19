using System;
using Windows.UI.Xaml.Data;

namespace FluentNetease.Converters
{
    public class TimeSpanToDoubleConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            return ((TimeSpan)value).TotalSeconds;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return TimeSpan.FromSeconds((double)value);
        }
    }
}
