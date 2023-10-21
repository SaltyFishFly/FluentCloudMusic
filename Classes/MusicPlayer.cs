using FluentCloudMusic.Controls;
using FluentCloudMusic.DataModels.JSONModels.Responses;
using FluentCloudMusic.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.Media.Playback;
using Windows.UI.Core;

namespace FluentCloudMusic.Classes
{
    public class MusicPlayer : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<ISong> PlaybackItemList { get; private set; } = new List<ISong>();
        public List<ISong> ShuffledPlaybackItemList { get; private set; } = new List<ISong>();       
        public MediaPlaybackState PlayState { get => _Player.PlaybackSession.PlaybackState; }
        public TimeSpan NaturalDuration { get => _Player.PlaybackSession.NaturalDuration; }
        public TimeSpan Position
        {
            get => _Player.PlaybackSession.Position;
            set => _Player.PlaybackSession.Position = value;
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
            set
            {
                _PlayMode = value;
                Notify(nameof(PlayMode));
            }
        }
        public double Volume
        {
            get => _Player.Volume;
            set
            {
                _Player.Volume = value;
                Notify(nameof(Volume));
            }
        }
        public bool HasPrevious => PlayIndex > 0;
        public bool HasNext => PlayIndex < PlaybackItemList.Count - 1;

        private readonly CoreDispatcher _Dispatcher;
        private readonly MediaPlayer _Player;
        private readonly SMTCController _SMTCController;
        private int _PlayIndex;
        private PlayMode _PlayMode;

        public MusicPlayer()
        {
            _Dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;

            _Player = new MediaPlayer()
            {
                AudioCategory = MediaPlayerAudioCategory.Media,
                AutoPlay = true,
                IsLoopingEnabled = false,
            };
            _Player.MediaEnded += Player_MediaEnded;

            _Player.PlaybackSession.PositionChanged +=
                (s, a) => DispatcherNotify(nameof(Position));
            _Player.PlaybackSession.NaturalDurationChanged +=
                (s, a) => DispatcherNotify(nameof(NaturalDuration));
            _Player.PlaybackSession.PlaybackStateChanged +=
                (s, a) => DispatcherNotify(nameof(PlayState));

            _SMTCController = new SMTCController(this);
        }

        public void SwitchPlayStatus()
        {
            switch (_Player.PlaybackSession.PlaybackState)
            {
                case MediaPlaybackState.Playing:
                    _Player.Pause();
                    break;
                case MediaPlaybackState.Paused:
                    _Player.Play();
                    break;
            }
        }

        public async Task PlayAsync(List<ISong> songs, int startIndex = 0)
        {
            PlaybackItemList = songs;
            ShuffledPlaybackItemList = songs.Shuffle(startIndex);
            await PlayAsync(startIndex);
        }

        public async Task Previous()
        {
            if (!HasPrevious) return;
            await _Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await PlayAsync(--PlayIndex));
        }

        public async Task Next()
        {
            if (!HasNext) return;
            await _Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await PlayAsync(++PlayIndex));
        }

        public async Task Replay()
        {
            if (!HasNext) return;
            await _Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await PlayAsync(PlayIndex));
        }

        public void Dispose()
        {
            _Player.Dispose();
        }

        private async Task PlayAsync(int index)
        {
            if (PlaybackItemList.Count <= index) return;

            try
            {
                PlayIndex = index;
                _Player.Source =
                    PlayMode == PlayMode.Shuffle ?
                    await ShuffledPlaybackItemList[index].ToMediaPlaybackItem() :
                    await PlaybackItemList[index].ToMediaPlaybackItem();

                _Player.Play();
            }
            catch (ResponseCodeErrorException) { }
        }

        private void Player_PositionChanged(MediaPlaybackSession sender, object args)
        {
            DispatcherNotify(nameof(Position));
        }

        private void Player_MediaEnded(MediaPlayer sender, object args)
        {
            if (PlayMode == PlayMode.RepeatOne) _ = Replay();
            else _ = Next();
        }

        private void Notify(string caller)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

        private void DispatcherNotify(string caller)
        {
            _ = _Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Notify(caller);
            });
        }

        public class SMTCController
        {
            private readonly MusicPlayer MusicPlayer;
            private MediaPlayer Player => MusicPlayer._Player;
            private MediaPlaybackCommandManager CommandManager => Player.CommandManager;

            public SMTCController(MusicPlayer player)
            {
                MusicPlayer = player;

                Player.SourceChanged += (sender, args) =>
                {
                    CommandManager.PreviousBehavior.EnablingRule =
                        MusicPlayer.HasPrevious ? MediaCommandEnablingRule.Always : MediaCommandEnablingRule.Never;
                    CommandManager.NextBehavior.EnablingRule =
                        MusicPlayer.HasNext ? MediaCommandEnablingRule.Always : MediaCommandEnablingRule.Never;
                };

                CommandManager.PreviousReceived += async (sender, args) =>
                {
                    var defer = args.GetDeferral();
                    await MusicPlayer.Previous();
                    args.Handled = true;
                    defer.Complete();
                };

                CommandManager.NextReceived += async (sender, args) =>
                {
                    var defer = args.GetDeferral();
                    await MusicPlayer.Next();
                    args.Handled = true;
                    defer.Complete();
                };
            }
        }
    }
}
