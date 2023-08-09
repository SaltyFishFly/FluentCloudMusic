using System;
using Windows.UI.Xaml.Data;

namespace FluentCloudMusic.Converters
{
    public class DoubleToStringConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            int totalSeconds = Convert.ToInt32((double)value);
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            return $"{minutes} : {seconds}";
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
