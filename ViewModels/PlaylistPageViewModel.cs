﻿using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.Utils;
using System.ComponentModel;

namespace FluentCloudMusic.ViewModels
{
    public class PlaylistPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Playlist Source
        {
            set
            {
                var oldImgUrl = _Source?.ImageUrl;

                _Source = value;
                Notify(nameof(Id));
                Notify(nameof(Name));
                Notify(nameof(Description));
                if (!UrlUtil.IsSameImage(oldImgUrl, ImageUrl)) Notify(nameof(ImageUrl));
            }
        }
        public string Id => _Source != null ? _Source.Id : string.Empty;
        public string Name => _Source != null ? _Source.Name : string.Empty;
        public string Description => _Source != null ? _Source.Description : string.Empty;
        public string ImageUrl => _Source != null ? _Source.ImageUrl : string.Empty;

        private Playlist _Source;

        private void Notify( string caller = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}
