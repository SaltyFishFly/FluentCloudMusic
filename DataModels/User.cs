using FluentCloudMusic.DataModels.JSONModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace FluentCloudMusic.DataModels
{
    // User -> 指正在使用软件的用户信息
    // Account -> 指网易云音乐内的任意一个账户信息
    public class User : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Account Account
        {
            set
            {
                _Account = value;
                Notify(nameof(HasLogin));
                Notify(nameof(UserId));
                Notify(nameof(Nickname));
                Notify(nameof(Signature));
                Notify(nameof(AvatarUrl));
                Notify(nameof(HasVip));
            }
        }
        public bool HasLogin => _Account != null;
        public string UserId => HasLogin ? _Account.UserId : string.Empty;
        public string Nickname => HasLogin ? _Account.Nickname : string.Empty;
        public string Signature => HasLogin ? _Account.Signature : string.Empty;
        public string AvatarUrl => HasLogin ? _Account.AvatarUrl : string.Empty;
        public bool HasVip => HasLogin && _Account.VipType != 0;

        private Account _Account;
        private List<Playlist> _PlaylistsCache;

        public async Task<List<Playlist>> GetPlaylistsAsync()
        {
            _PlaylistsCache ??= await _Account.GetPlaylistsAsync();
            return _PlaylistsCache;
        }

        private void Notify(string member)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(member));
        }
    }
}
