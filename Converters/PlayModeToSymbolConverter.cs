using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using static FluentNetease.Controls.MusicPlayer;

namespace FluentNetease.Converters
{
    class PlayModeToSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (PlayModeEnum)value switch
            {
                PlayModeEnum.RepeatAll => Symbol.RepeatAll,
                PlayModeEnum.RepeatOne => Symbol.RepeatOne,
                PlayModeEnum.Shuffle => Symbol.Shuffle,
                _ => throw new NotImplementedException(),
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
