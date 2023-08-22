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

using SMTC = Windows.Media.SystemMediaTransportControls;

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

        public MediaPlayer Player { get; private set; }
        public List<Song> PlaybackItemList { get; private set; } = new List<Song>();
        public List<Song> ShuffledPlaybackItemList { get; private set; } = new List<Song>();
        public int PlayIndex 
        {
            get => _PlayIndex;
            private set
            {
                _PlayIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasPrevious)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasNext)));
            }
        }
        public PlayMode PlayMode
        {
            get => _PlayMode;
            private set
            {
                _PlayMode = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PlayMode)));
            }
        }
        // VolumeSlider的范围为[0.0, 1000.0]，需要映射到Player.Volume的范围[0.0, 1.0]
        public double Volume
        {
            get => Player.Volume * 1000;
            private set
            {
                Player.Volume = value / 1000;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Volume)));
            }
        }
        public bool HasPrevious => PlayIndex > 0;
        public bool HasNext => PlayIndex < PlaybackItemList.Count - 1;

        private int _PlayIndex;
        private PlayMode _PlayMode;
        private SMTCController SMTCController;
        private readonly DispatcherTimer Timer;
        

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

            SMTCController = new SMTCController(this);

            PlayMode = StorageService.HasSetting("PlayMode") ? StorageService.GetSetting<PlayMode>("PlayMode") : PlayMode.RepeatList;
            Volume = StorageService.HasSetting("Volume") ? StorageService.GetSetting<double>("Volume") : 500.0;

            this.InitializeComponent();

            Timer = new DispatcherTimer(){ Interval = TimeSpan.FromSeconds(1) };
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }

        public void Play(Song song)
        {
            Play(new List<Song>() { song });
        }

        public void Play(List<Song> songs)
        {
            PlaybackItemList = songs;
            ShuffledPlaybackItemList = songs.Shuffle();
            Play(0);
        }

        public void Previous()
        {
            if (HasPrevious) Play(--PlayIndex);
        }

        public void Next()
        {
            if (HasNext) Play(++PlayIndex);
        }

        public void Replay()
        {
            Play(PlayIndex);
        }

        public void Dispose()
        {
            Timer.Stop();
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
            // MediaPlayer类拉起的事件不能直接操作UI线程，必须使用Dispatcher.RunAsync()包一层
            _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                PlayButton.IsEnabled = isEnabled;
                PlayButton_Icon.Symbol = IconSymbol;
            });
        }

        private void Player_MediaEnded(MediaPlayer sender, object args)
        {
            if (PlayMode == PlayMode.RepeatOne)
                _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Replay());
            else
                _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Next());
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

        private void Timeline_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            Player.PlaybackSession.Position = TimeSpan.FromSeconds(((Slider)sender).Value);
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
    }

    public class SMTCController
    {
        private readonly MusicPlayer MusicPlayerControl;
        private MediaPlayer Player => MusicPlayerControl.Player;
        private MediaPlaybackCommandManager CommandManager => Player.CommandManager;
        private SMTC SMTC => Player.SystemMediaTransportControls;

        public SMTCController(MusicPlayer musicPlayer)
        {
            MusicPlayerControl = musicPlayer;
            Player.SourceChanged += Player_SourceChanged;
            CommandManager.PreviousReceived += (sender, args) => MusicPlayerControl.Previous();
            CommandManager.NextReceived += (sender, args) => MusicPlayerControl.Next();
        }

        private void Player_SourceChanged(MediaPlayer sender, object args)
        {
            CommandManager.PreviousBehavior.EnablingRule =
                MusicPlayerControl.HasPrevious ? MediaCommandEnablingRule.Always : MediaCommandEnablingRule.Never;
            CommandManager.NextBehavior.EnablingRule =
                MusicPlayerControl.HasNext ? MediaCommandEnablingRule.Always : MediaCommandEnablingRule.Never;
        }
    }
}