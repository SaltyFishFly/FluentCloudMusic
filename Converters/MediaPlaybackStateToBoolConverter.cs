using System;
using Windows.Media.Playback;
using Windows.UI.Xaml.Data;

namespace FluentCloudMusic.Converters
{
    public class MediaPlaybackStateToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (MediaPlaybackState)value switch
            {
                MediaPlaybackState.Playing => true,
                MediaPlaybackState.Paused => true,
                _ => false
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
