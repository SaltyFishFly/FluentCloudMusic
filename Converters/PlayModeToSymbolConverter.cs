using FluentCloudMusic.Controls;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace FluentCloudMusic.Converters
{
    class PlayModeToSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (PlayMode)value switch
            {
                PlayMode.RepeatList => Symbol.RepeatAll,
                PlayMode.RepeatOne => Symbol.RepeatOne,
                PlayMode.Shuffle => Symbol.Shuffle,
                _ => throw new NotImplementedException(),
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
