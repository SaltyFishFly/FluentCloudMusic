using FluentCloudMusic.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.Media.Playback;
using Windows.Storage;
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
        public List<AbstractMusic> PlaybackItemList { get; set; } = new List<AbstractMusic>();
        public List<AbstractMusic> ShuffledPlaybackItemList { get; set; } = new List<AbstractMusic>();
        public int CurrentPlayIndex { get; set; }
        public ObservablePlayMode PlayMode { get; set; }
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
                Storage.HasSetting("PlayMode") ? Storage.GetSetting<PlayModeEnum>("PlayMode") : PlayModeEnum.RepeatList;
            PlayMode = new ObservablePlayMode(playmode);

            Timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            Timer.Tick += Timer_Tick;
            Timer.Start();

            this.InitializeComponent();

            VolumeSlider.Value =
                Storage.HasSetting("Volume") ? Storage.GetSetting<double>("Volume") : 500.0;       
        }

        private void Timer_Tick(object sender, object e)
        {
            Bindings.Update();
        }

        private void MusicButton_Click(object sender, RoutedEventArgs e)
        {

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
            Storage.SetSetting("PlayMode", (int)PlayMode);
        }

        private void VolumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            Player.Volume = e.NewValue / 1000;
            Storage.SetSetting("Volume", e.NewValue);
        }

        private void Player_MediaEnded(MediaPlayer sender, object args)
        {
            if (PlayMode.Mode == PlayModeEnum.RepeatOne) RePlay();
            else PlayNext();
        }

        private async void Play(int index)
        {
            if (PlaybackItemList.Count > index)
            {
                CurrentPlayIndex = index;
                if (PlayMode.Mode == PlayModeEnum.Shuffle)
                    Player.Source = await ShuffledPlaybackItemList[index].ToMediaPlaybackItem();
                else
                    Player.Source = await PlaybackItemList[index].ToMediaPlaybackItem();
                Player.Play();
            }
        }

        public void Play(AbstractMusic music)
        {
            PlaybackItemList = new List<AbstractMusic> { music };
            Play(0);
        }

        public void Play(List<AbstractMusic> musicList)
        {
            PlaybackItemList = musicList;
            ShuffledPlaybackItemList = new List<AbstractMusic>(PlaybackItemList).Shuffle();
            Play(0);
        }

        private void PlayPrevious()
        {
            if (CurrentPlayIndex > 0) Play(--CurrentPlayIndex);
        }

        private void PlayNext()
        {
            if (CurrentPlayIndex <= PlaybackItemList.Count) Play(++CurrentPlayIndex);
        }

        private void RePlay()
        {
            Play(CurrentPlayIndex);
        }

        public void Dispose()
        {
            Timer.Stop();
            Player.Dispose();
        }
    }

    public class ObservablePlayMode : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private PlayModeEnum _Mode;

        public PlayModeEnum Mode { get { return _Mode; } }

        public ObservablePlayMode(PlayModeEnum mode = PlayModeEnum.RepeatList)
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

        public static explicit operator int(ObservablePlayMode mode)
        {
            return (int)mode._Mode;
        }
    }

    public enum PlayModeEnum
    {
        RepeatList, RepeatOne, Shuffle,
    }
}