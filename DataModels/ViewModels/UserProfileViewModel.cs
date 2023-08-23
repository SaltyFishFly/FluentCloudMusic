using FluentCloudMusic.DataModels.JSONModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Notify(nameof(UserID));
                Notify(nameof(Nickname));
                Notify(nameof(AvatarUrl));
                Notify(nameof(HasVip));
            }
        }
        public bool HasLogin => _Source != null;
        public string UserID => HasLogin ? _Source.UserId : string.Empty;
        public string Nickname => HasLogin ? _Source.Nickname : string.Empty;
        public string AvatarUrl => HasLogin ? _Source.AvatarUrl : "ms-appx:///Assets/LargeTile.scale-400.png";
        public bool HasVip => HasLogin && _Source.VipType != 0;

        private Profile _Source;

        private void Notify(string member)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(member));
        }
    }

}
