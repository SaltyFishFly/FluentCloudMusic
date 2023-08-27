using FluentCloudMusic.DataModels.JSONModels;
using System.ComponentModel;

namespace FluentCloudMusic.DataModels.ViewModels
{
    public class UserProfileViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Profile Source
        {
            set
            {
                _Source = value;
                Notify(nameof(HasLogin));
                Notify(nameof(UserId));
                Notify(nameof(Nickname));
                Notify(nameof(AvatarUrl));
                Notify(nameof(HasVip));
            }
        }
        public bool HasLogin => _Source != null;
        public string UserId => HasLogin ? _Source.UserId : string.Empty;
        public string Nickname => HasLogin ? _Source.Nickname : string.Empty;
        public string AvatarUrl =>
            _Source != null && _Source.AvatarUrl != null ?
            _Source.AvatarUrl :
            "ms-appx:///Assets/LargeTile.scale-400.png";
        public bool HasVip => HasLogin && _Source.VipType != 0;

        private Profile _Source;

        private void Notify(string member)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(member));
        }
    }

}
