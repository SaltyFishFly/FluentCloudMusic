using System;
using System.ComponentModel;
using Windows.Media.Playback;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace FluentCloudMusic.Converters
{
    public class MediaPlaybackStateToSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (MediaPlaybackState)value switch
            {
                MediaPlaybackState.None => Symbol.Stop,
                MediaPlaybackState.Opening => Symbol.Download,
                MediaPlaybackState.Buffering => Symbol.Download,
                MediaPlaybackState.Playing => Symbol.Pause,
                MediaPlaybackState.Paused => Symbol.Play,
                _ => throw new InvalidEnumArgumentException()
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
