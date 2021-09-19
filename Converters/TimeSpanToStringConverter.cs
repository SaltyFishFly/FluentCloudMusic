using System;
using Windows.UI.Xaml.Data;

namespace FluentNetease.Converters
{
    public class TimeSpanToStringConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            int Seconds = (int)((TimeSpan)value).TotalSeconds;
            return (Seconds / 60) + " : " + (Seconds % 60);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
