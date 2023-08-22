using FluentCloudMusic.Classes;
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
    public enum PlayMode
    {
        RepeatList, RepeatOne, Shuffle
    }

    public sealed partial class MusicPlayerControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MediaPlayer Player { get; private set; }
        public List<Song> PlaybackItemList { get; private set; } = new List<Song>();
        public List<Song> ShuffledPlaybackItemList { get; private set; } = new List<Song>();
        public MediaPlaybackState PlayState { get => Player.PlaybackSession.PlaybackState; }
        public TimeSpan NaturalDuration { get => Player.PlaybackSession.NaturalDuration; }
        public TimeSpan Position
        {
            get => Player.PlaybackSession.Position;
            set => Player.PlaybackSession.Position = value;
        }
        public int PlayIndex 
        {
            get => _PlayIndex;
            private set
            {
                _PlayIndex = value;
                Notify(nameof(HasPrevious));
                Notify(nameof(HasNext));
            }
        }
        public PlayMode PlayMode
        {
            get => _PlayMode;
            private set
            {
                _PlayMode = value;
                Notify(nameof(PlayMode));
            }
        }
        // VolumeSlider的范围为[0.0, 1000.0]，需要映射到Player.Volume的范围[0.0, 1.0]
        public double Volume
        {
            get => Player.Volume * 1000;
            private set
            {
                Player.Volume = value / 1000;
                Notify(nameof(Volume));
            }
        }
        public bool HasPrevious => PlayIndex > 0;
        public bool HasNext => PlayIndex < PlaybackItemList.Count - 1;

        private int _PlayIndex;
        private PlayMode _PlayMode;
        private bool IsDragging;
        private SMTCController SMTCController;
        
        public MusicPlayerControl()
        {
            Player = new MediaPlayer
            {
                AudioCategory = MediaPlayerAudioCategory.Media,
                AutoPlay = true,
                IsLoopingEnabled = false,
            };
            Player.PlaybackSession.PositionChanged += 
                (sender, args) => { if (!IsDragging) DispatcherNotify(nameof(Position)); };
            Player.PlaybackSession.NaturalDurationChanged += 
                (sender, args) => DispatcherNotify(nameof(NaturalDuration));
            Player.PlaybackSession.PlaybackStateChanged +=
                (sender, args) => DispatcherNotify(nameof(PlayState));
            Player.MediaEnded += Player_MediaEnded;

            SMTCController = new SMTCController(this);

            PlayMode = StorageService.HasSetting("PlayMode") ? StorageService.GetSetting<PlayMode>("PlayMode") : PlayMode.RepeatList;
            Volume = StorageService.HasSetting("Volume") ? StorageService.GetSetting<double>("Volume") : 500.0;

            this.InitializeComponent();
        }

        public void Play(List<Song> songs)
        {
            PlaybackItemList = songs;
            ShuffledPlaybackItemList = songs.Shuffle();
            Play(0);
        }

        public void Previous()
        {
            if (!HasPrevious) return;
            _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Play(--PlayIndex));
        }

        public void Next()
        {
            if (!HasNext) return;
            _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Play(++PlayIndex));
        }

        public void Replay()
        {
            Play(PlayIndex);
        }

        public void Dispose()
        {
            Player.Dispose();
        }

        private async void Play(int index)
        {
            if (PlaybackItemList.Count <= index) return;

            PlayIndex = index;
            Player.Source =
                PlayMode == PlayMode.Shuffle ?
                await ShuffledPlaybackItemList[index].ToMediaPlaybackItem() :
                await PlaybackItemList[index].ToMediaPlaybackItem();

            Player.Play();
        }

        private void Player_MediaEnded(MediaPlayer sender, object args)
        {
            if (PlayMode == PlayMode.RepeatOne) Replay();
            else Next();
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

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            Previous();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            Next();
        }

        private void Timeline_Loaded(object sender, RoutedEventArgs e)
        {
            var thumb = VisualTreeUtils.FindChildByName(Timeline, "HorizontalThumb") as Thumb;
            thumb.DragStarted += (sender, args) => IsDragging = true;
            thumb.DragCompleted += (sender, args) => IsDragging = false;
        }

        private void Timeline_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            Position = TimeSpan.FromSeconds(Timeline.Value);
        }

        private void PlayModeButton_Click(object sender, RoutedEventArgs e)
        {
            PlayMode = PlayMode switch
            {
                PlayMode.RepeatList => PlayMode.RepeatOne,
                PlayMode.RepeatOne => PlayMode.Shuffle,
                PlayMode.Shuffle => PlayMode.RepeatList,
                _ => throw new InvalidEnumArgumentException()
            };
            StorageService.SetSetting("PlayMode", (int)PlayMode);
        }

        private void VolumeSlider_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            StorageService.SetSetting("Volume", VolumeSlider.Value);
        }

        private void DispatcherNotify(string caller)
        {
            _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Notify(caller);
            });
        }

        private void Notify(string caller)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}