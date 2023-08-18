using System;
using Windows.UI.Xaml.Data;

namespace FluentCloudMusic.Converters
{
    public class VolumeSliderToolTipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ((int)((double)value / 10.0)).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
