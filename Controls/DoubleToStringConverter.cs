using System;
using Windows.UI.Xaml.Data;

namespace FluentNetease.Controls
{
    public class DoubleToStringConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            int Seconds = Convert.ToInt32((double)value);
            return (Seconds / 60) + " : " + (Seconds % 60);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
