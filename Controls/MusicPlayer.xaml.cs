using FluentNetease.Classes;
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
        public List<AbstractMusic> PlayItemList { get; set; }
        public int CurrentPlayIndex { get; set; }
        public int CurrentPosition { get; set; }
        public PlayModeEnum PlayMode { get; set; }
        public DispatcherTimer Timer { get; set; }
        public enum PlayModeEnum
        {
            RepeatAll, RepeatOne, Shuffle
        }
        public MusicPlayer()
        {
            this.InitializeComponent();
            InitalizePlayer();
        }

        /// <summary>
        /// 初始化内容
        /// Initalize Player.
        /// </summary>
        private void InitalizePlayer()
        {
            //初始化播放器
            PlayItemList = new List<AbstractMusic>();

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
                Interval = TimeSpan.FromSeconds(0.1),
            };
            Timer.Tick += Timer_Tick;
            Timer.Start();

            //从设置中读取音量
            object LocalVolume = ApplicationData.Current.LocalSettings.Values["Volume"];
            VolumeSlider.Value = LocalVolume == null ? 100 : (double)LocalVolume;
        }

        /// <summary>
        /// 更新数据绑定
        /// Sync timeline with position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        private void PlayModeButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PlayMode = PlayMode switch
            {
                PlayModeEnum.RepeatAll => PlayModeEnum.RepeatOne,
                PlayModeEnum.RepeatOne => PlayModeEnum.Shuffle,
                PlayModeEnum.Shuffle => PlayModeEnum.RepeatAll,
                _ => throw new NotImplementedException()
            };
        }

        /// <summary>
        /// 同步音量滑块与播放器音量
        /// Sync volume between volume slider and player.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VolumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            Player.Volume = e.NewValue / 100;
            ApplicationData.Current.LocalSettings.Values["Volume"] = e.NewValue;
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
            if (PlayItemList.Count > index)
            {
                CurrentPlayIndex = index;
                Player.Source = await PlayItemList[index].ToMediaPlaybackItem();
                Player.Play();
            }
        }

        public void Play(AbstractMusic music)
        {
            PlayItemList = new List<AbstractMusic> { music };
            Play(0);
        }

        public void Play(List<AbstractMusic> musicList)
        {
            PlayItemList = musicList;
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
            if (CurrentPlayIndex <= PlayItemList.Count)
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