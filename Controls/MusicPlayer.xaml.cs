using FluentNetease.Classes;
using FluentNetease.Converters;
using System;
using System.Collections.Generic;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FluentNetease.Controls
{
    public sealed partial class MusicPlayer : UserControl
    {
        public MediaPlayer Player { get; set; }
        public List<AbstractMusic> PlaybackItemList { get; set; } = new List<AbstractMusic>();
        public List<AbstractMusic> ShuffledPlaybackItemList { get; set; } = new List<AbstractMusic>();
        public int CurrentPlayIndex { get; set; }
        public int CurrentPosition { get; set; }
        public PlayModeEnum PlayMode { get; set; }
        private PlayModeToSymbolConverter PlayModeToSymbolConverter { get; set; } = new PlayModeToSymbolConverter();
        private DispatcherTimer Timer { get; set; }
        public enum PlayModeEnum
        {
            RepeatAll, RepeatOne, Shuffle
        }
        public MusicPlayer()
        {
            this.InitializeComponent();
            InitalizePlayer();
        }

        private void InitalizePlayer()
        {
            PlayMode = PlayModeEnum.RepeatOne;
            Player = new MediaPlayer
            {
                AudioCategory = MediaPlayerAudioCategory.Media,
                AutoPlay = true,
                IsLoopingEnabled = false,
                Volume = VolumeSlider.Value
            };
            Player.PlaybackSession.PlaybackStateChanged += Player_PlaybackSession_PlaybackStateChanged;
            Player.MediaEnded += Player_MediaEnded;

            //初始化计时器
            Timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            Timer.Tick += Timer_Tick;
            Timer.Start();

            //读取设置
            object LocalVolume = ApplicationData.Current.LocalSettings.Values["Volume"];
            VolumeSlider.Value = LocalVolume == null ? 100 : (double)LocalVolume;
            object LocalPlayMode = ApplicationData.Current.LocalSettings.Values["PlayMode"];
            PlayMode = LocalPlayMode == null ? PlayModeEnum.RepeatAll : (PlayModeEnum)(int)LocalPlayMode;
            PlayModeButton_Icon.Symbol = (Symbol)PlayModeToSymbolConverter.Convert(PlayMode, null, null, null);
        }

        private void Timer_Tick(object sender, object e)
        {
            Bindings.Update();
        }

        private void MusicButton_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void PlayButton_Tapped(object sender, TappedRoutedEventArgs e)
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
            bool EnableFlag = sender.PlaybackState switch
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
                PlayButton.IsEnabled = EnableFlag;
                PlayButton_Icon.Symbol = IconSymbol;
            });
        }

        private void PlayModeButton_Click(object sender, RoutedEventArgs e)
        {
            PlayMode = PlayMode switch
            {
                PlayModeEnum.RepeatAll => PlayModeEnum.RepeatOne,
                PlayModeEnum.RepeatOne => PlayModeEnum.Shuffle,
                PlayModeEnum.Shuffle => PlayModeEnum.RepeatAll,
                _ => throw new NotImplementedException()
            };
            PlayModeButton_Icon.Symbol = (Symbol)PlayModeToSymbolConverter.Convert(PlayMode, null, null, null);
            ApplicationData.Current.LocalSettings.Values["PlayMode"] = (int)PlayMode;
        }

        private void VolumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            Player.Volume = e.NewValue / 100;
            ApplicationData.Current.LocalSettings.Values["Volume"] = e.NewValue;
        }

        private void PlayModeButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (PlayMode == PlayModeEnum.RepeatOne)
            {
                RePlay();
            }
            else
            {
                PlayNext();
            }
        }

        private void Player_MediaEnded(MediaPlayer sender, object args)
        {
            switch (PlayMode)
            {
                case PlayModeEnum.RepeatAll:
                    PlayNext();
                    break;
                case PlayModeEnum.RepeatOne:
                    RePlay();
                    break;
            }
        }

        private async void Play(int index)
        {
            if (PlaybackItemList.Count > index)
            {
                CurrentPlayIndex = index;
                if (PlayMode != PlayModeEnum.Shuffle)
                {
                    Player.Source = await PlaybackItemList[index].ToMediaPlaybackItem();
                }
                else
                {
                    Player.Source = await ShuffledPlaybackItemList[index].ToMediaPlaybackItem();
                }
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
            if (CurrentPlayIndex > 0)
            {
                Play(--CurrentPlayIndex);
            }
        }

        private void PlayNext()
        {
            if (CurrentPlayIndex <= PlaybackItemList.Count)
            {
                Play(++CurrentPlayIndex);
            }
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
}