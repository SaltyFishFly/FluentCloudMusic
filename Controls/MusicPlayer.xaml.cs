using FluentCloudMusic.DataModels;
using FluentCloudMusic.Services;
using FluentCloudMusic.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.Media.Playback;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FluentCloudMusic.Controls
{
    public sealed partial class MusicPlayer : UserControl
    {
        public MediaPlayer Player { get; set; }
        public List<Song> PlaybackItemList { get; set; } = new List<Song>();
        public List<Song> ShuffledPlaybackItemList { get; set; } = new List<Song>();
        public int PlayPositionIndex { get; set; }
        public PlayMode PlayMode { get; set; }
        private DispatcherTimer Timer { get; set; }
        
        public MusicPlayer()
        {
            Player = new MediaPlayer
            {
                AudioCategory = MediaPlayerAudioCategory.Media,
                AutoPlay = true,
                IsLoopingEnabled = false,
            };
            Player.PlaybackSession.PlaybackStateChanged += Player_PlaybackSession_PlaybackStateChanged;
            Player.MediaEnded += Player_MediaEnded;

            var playmode =
                StorageService.HasSetting("PlayMode") ? StorageService.GetSetting<PlayModeEnum>("PlayMode") : PlayModeEnum.RepeatList;
            PlayMode = new PlayMode(playmode);

            Timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            Timer.Tick += Timer_Tick;
            Timer.Start();

            this.InitializeComponent();

            VolumeSlider.Value =
                StorageService.HasSetting("Volume") ? StorageService.GetSetting<double>("Volume") : 500.0;       
        }

        private void Timer_Tick(object sender, object e)
        {
            Bindings.Update();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            switch (Player.PlaybackSession.PlaybackState)
            {
                case MediaPlaybackState.Playing:
                    Player.Pause();
                    break;
                case MediaPlaybackState.Paused:
                    Player.Play();
                    break;
            }
        }

        private void Timeline_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            Player.PlaybackSession.Position = TimeSpan.FromSeconds(((Slider)sender).Value);
        }

        private void Player_PlaybackSession_PlaybackStateChanged(MediaPlaybackSession sender, object args)
        {
            bool isEnabled = sender.PlaybackState switch
            {
                MediaPlaybackState.Playing => true,
                MediaPlaybackState.Paused => true,
                _ => false
            };
            Symbol IconSymbol = sender.PlaybackState switch
            {
                MediaPlaybackState.Playing => Symbol.Pause,
                MediaPlaybackState.Paused => Symbol.Play,
                _ => Symbol.Download
            };
            _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                PlayButton.IsEnabled = isEnabled;
                PlayButton_Icon.Symbol = IconSymbol;
            });
        }

        private void PlayModeButton_Click(object sender, RoutedEventArgs e)
        {
            PlayMode.Next();
            StorageService.SetSetting("PlayMode", (int)PlayMode);
        }

        private void VolumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            Player.Volume = e.NewValue / 1000;
            StorageService.SetSetting("Volume", e.NewValue);
        }

        private void Player_MediaEnded(MediaPlayer sender, object args)
        {
            if (PlayMode.Mode == PlayModeEnum.RepeatOne) RePlay();
            else PlayNext();
        }

        public void Play(Song song)
        {
            Play(new List<Song>(){ song });
        }

        public void Play(List<Song> songs)
        {
            PlaybackItemList = songs;
            ShuffledPlaybackItemList = PlaybackItemList.Shuffle();
            Play(0);
        }

        private async void Play(int index)
        {
            if (PlaybackItemList.Count > index)
            {
                PlayPositionIndex = index;
                if (PlayMode.Mode == PlayModeEnum.Shuffle)
                    Player.Source = await ShuffledPlaybackItemList[index].ToMediaPlaybackItem();
                else
                    Player.Source = await PlaybackItemList[index].ToMediaPlaybackItem();
                Player.Play();
            }
        }

        private void PlayPrevious()
        {
            if (PlayPositionIndex > 0) Play(--PlayPositionIndex);
        }

        private void PlayNext()
        {
            if (PlayPositionIndex < PlaybackItemList.Count - 1) Play(++PlayPositionIndex);
        }

        private void RePlay()
        {
            Play(PlayPositionIndex);
        }

        public void Dispose()
        {
            Timer.Stop();
            Player.Dispose();
        }
    }

    public class PlayMode : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private PlayModeEnum _Mode;

        public PlayModeEnum Mode { get => _Mode; }

        public PlayMode(PlayModeEnum mode = PlayModeEnum.RepeatList)
        {
            _Mode = mode;
        }

        public void Next()
        {
            _Mode = _Mode switch
            {
                PlayModeEnum.RepeatList => PlayModeEnum.RepeatOne,
                PlayModeEnum.RepeatOne => PlayModeEnum.Shuffle,
                PlayModeEnum.Shuffle => PlayModeEnum.RepeatList,
                _ => throw new NotImplementedException(),
            };
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mode)));
        }

        public static explicit operator int(PlayMode mode)
        {
            return (int)mode._Mode;
        }
    }

    public enum PlayModeEnum
    {
        RepeatList, RepeatOne, Shuffle,
    }
}