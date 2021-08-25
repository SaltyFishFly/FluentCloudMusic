using FluentNetease.Classes;
using System;
using System.Collections.Generic;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Pickers;
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
        public int CurrentPlayIndex { get; set; }
        public PlayModeEnum PlayMode { get; set; }
        public DispatcherTimer Timer { get; set; }
        public enum PlayModeEnum
        {
            Random, Loop, Sequence
        }
        public MusicPlayer()
        {
            this.InitializeComponent();
            InitalizePlayer();
            InitalizeTimer();
        }

        /// <summary>
        /// 初始化播放器
        /// Initalize Player.
        /// </summary>
        private void InitalizePlayer()
        {
            object LocalVolume = ApplicationData.Current.LocalSettings.Values["Volume"];
            PlayMode = PlayModeEnum.Sequence;
            Player = new MediaPlayer
            {
                AudioCategory = MediaPlayerAudioCategory.Media,
                AutoPlay = true,
                IsLoopingEnabled = false,
                Volume = VolumeSlider.Value
            };
            VolumeSlider.Value = LocalVolume == null ? 100 : (double)LocalVolume;
            Player.PlaybackSession.PlaybackStateChanged += Player_PlaybackSession_PlaybackStateChanged;
        }

        /// <summary>
        /// 初始化计时器
        /// Initalize Timer.
        /// </summary>
        private void InitalizeTimer()
        {
            Timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1),
            };
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }

        /// <summary>
        /// 将时间轴与播放位置做同步
        /// Sync timeline with position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, object e)
        {
            Bindings.Update();
        }

        /// <summary>
        /// 导航到新页面展示当前音乐信息
        /// Navigate to a new page to show info of current music.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MusicButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //测试用
            FileOpenPicker picker = new FileOpenPicker()
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.MusicLibrary
            };
            picker.FileTypeFilter.Add(".mp3");
            picker.FileTypeFilter.Add(".aac");
            picker.FileTypeFilter.Add(".flac");
            IReadOnlyList<StorageFile> files = await picker.PickMultipleFilesAsync();
            if (files.Count > 0)
            {
                MediaPlaybackList playbackList = new MediaPlaybackList()
                {
                    AutoRepeatEnabled = true
                };
                foreach (StorageFile file in files)
                {
                    playbackList.Items.Add(new MediaPlaybackItem(MediaSource.CreateFromStorageFile(file)));
                }
                Player.Source = playbackList;
            }
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

        /// <summary>
        /// 同步时间滑块与播放器时间
        /// Sync time between time slider and player.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timeline_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            Player.PlaybackSession.Position = TimeSpan.FromSeconds(e.NewValue);
        }

        private void Player_PlaybackSession_PlaybackStateChanged(MediaPlaybackSession sender, object args)
        {
            switch (sender.PlaybackState)
            {
                case MediaPlaybackState.Playing:
                    _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                      {
                          PlayButton.IsEnabled = true;
                          PlayButton_Icon.Symbol = Symbol.Pause;
                      });
                    break;
                case MediaPlaybackState.Paused:
                    _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                      {
                          PlayButton.IsEnabled = true;
                          PlayButton_Icon.Symbol = Symbol.Play;
                      });
                    break;
                default:
                    _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        PlayButton.IsEnabled = false;
                        PlayButton_Icon.Symbol = Symbol.Refresh;
                    });
                    break;
            }
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

        public async void PlaySingle(string id)
        {
            var (IsSuccess, RequestResult) = await Network.GetMusicUrlAsync(id);
            if (IsSuccess)
            {
                Player.Source = RequestResult;
            }
        }

        public async void PlaySingle(Song song)
        {
            var (IsSuccess, RequestResult) = await Network.GetMusicUrlAsync(song.Music.ID);
            if (IsSuccess)
            {
                Player.Source = RequestResult;
            }
        }

        public async void PlayList(IList<Song> songs)
        {
            var (IsSuccess, RequestResult) = await Network.GetMusicUrlAsync(songs);
            if (IsSuccess)
            {
                Player.Source = RequestResult;
            }
        }
    }
}