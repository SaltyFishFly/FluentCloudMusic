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
using Windows.UI.Xaml.Input;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FluentCloudMusic.Controls
{
    public enum PlayMode
    {
        RepeatList, RepeatOne, Shuffle
    }

    public sealed partial class MusicPlayer : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MediaPlayer Player { get; set; }
        // VolumeSlider的范围为[0.0, 1000.0]，需要映射到Player.Volume的范围[0.0, 1.0]
        public double Volume
        {
            get => Player.Volume * 1000;
            set
            {
                Player.Volume = value / 1000;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Volume)));
            }
        }
        public List<Song> PlaybackItemList { get; set; } = new List<Song>();
        public List<Song> ShuffledPlaybackItemList { get; set; } = new List<Song>();
        public int PlayPositionIndex { get; set; }
        public PlayMode PlayMode
        {
            get => _PlayMode;
            set
            {
                _PlayMode = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PlayMode)));
            }
        }

        private SystemMediaTransportControlsController PlayerSMTCController;
        private DispatcherTimer Timer;
        private PlayMode _PlayMode;

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

            PlayMode =
                StorageService.HasSetting("PlayMode") ? StorageService.GetSetting<PlayMode>("PlayMode") : Controls.PlayMode.RepeatList;
            Volume =
                StorageService.HasSetting("Volume") ? StorageService.GetSetting<double>("Volume") : 500.0;

            PlayerSMTCController = new SystemMediaTransportControlsController(Player);

            this.InitializeComponent();

            Timer = new DispatcherTimer(){ Interval = TimeSpan.FromSeconds(1) };
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            Bindings.Update();
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

        private void Player_MediaEnded(MediaPlayer sender, object args)
        {
            if (PlayMode == Controls.PlayMode.RepeatOne) RePlay();
            else PlayNext();
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

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            PlayNext();
        }

        private void Timeline_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            Player.PlaybackSession.Position = TimeSpan.FromSeconds(((Slider)sender).Value);
        }

        private void PlayModeButton_Click(object sender, RoutedEventArgs e)
        {
            PlayMode = PlayMode switch
            {
                Controls.PlayMode.RepeatList => Controls.PlayMode.RepeatOne,
                Controls.PlayMode.RepeatOne => Controls.PlayMode.Shuffle,
                Controls.PlayMode.Shuffle => Controls.PlayMode.RepeatList,
                _ => throw new InvalidEnumArgumentException()
            };
            StorageService.SetSetting("PlayMode", (int)PlayMode);
        }

        private void VolumeSlider_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            StorageService.SetSetting("Volume", VolumeSlider.Value);
        }

        public void Play(Song song)
        {
            Play(new List<Song>(){ song });
        }

        public void Play(List<Song> songs)
        {
            PlaybackItemList = songs;
            ShuffledPlaybackItemList = songs.Shuffle();
            Play(0);
        }

        private async void Play(int index)
        {
            if (PlaybackItemList.Count <= index) return;

            if (PlayMode == Controls.PlayMode.Shuffle)
                Player.Source = await ShuffledPlaybackItemList[index].ToMediaPlaybackItem();
            else
                Player.Source = await PlaybackItemList[index].ToMediaPlaybackItem();
            PlayPositionIndex = index;

            Player.Play();
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

    public class SystemMediaTransportControlsController
    {
        public SystemMediaTransportControlsController(MediaPlayer player)
        {
                
        }
    }
}