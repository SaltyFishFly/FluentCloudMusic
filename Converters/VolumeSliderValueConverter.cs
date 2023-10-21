using FluentCloudMusic.Controls;
using System;
using Windows.UI.Xaml.Data;

namespace FluentCloudMusic.Converters
{
    public class VolumeSliderValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (double)value * MusicPlayerControl.VolumeSliderScalingFactor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (double)value / MusicPlayerControl.VolumeSliderScalingFactor;
        }
    }
}
