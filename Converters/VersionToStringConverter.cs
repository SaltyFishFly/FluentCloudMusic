using System;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Data;

namespace FluentCloudMusic.Converters
{
    public class VersionToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var version = (PackageVersion)value;
            return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
