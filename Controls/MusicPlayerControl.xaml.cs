using FluentCloudMusic.Classes;
using FluentCloudMusic.DataModels.ViewModels;
using FluentCloudMusic.Services;
using FluentCloudMusic.Utils;
using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace FluentCloudMusic.Controls
{
    public enum PlayMode
    {
        RepeatList, RepeatOne, Shuffle
    }

    public sealed partial class MusicPlayerControl : UserControl
    {
        public const double VolumeSliderScalingFactor = 1000.0;

        private readonly MusicPlayer _Player;
        private readonly MusicPlayerControlViewModel ViewModel;

        public MusicPlayerControl()
        {
            _Player = App.Player;
            ViewModel = new MusicPlayerControlViewModel() { Source = _Player };

            InitializeComponent();

            _Player.PlayMode =
                StorageService.HasSetting("PlayMode") ?
                StorageService.GetSetting<PlayMode>("PlayMode") :
                PlayMode.RepeatList;
            _Player.Volume = 
                StorageService.HasSetting("Volume") ?
                StorageService.GetSetting<double>("Volume") :
                0.5;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            _Player.SwitchPlayStatus();
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            _ = _Player.Previous();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            _ = _Player.Next();
        }

        private void Timeline_Loaded(object sender, RoutedEventArgs e)
        {
            var thumb = VisualTreeUtil.FindChildByName(Timeline, "HorizontalThumb") as Thumb;
            thumb.DragStarted += (s, a) => ViewModel.IsDragging = true;
            thumb.DragCompleted += (s, a) => ViewModel.IsDragging = false;
        }

        private void Timeline_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            _Player.Position = TimeSpan.FromSeconds(Timeline.Value);
        }

        private void PlayModeButton_Click(object sender, RoutedEventArgs e)
        {
            _Player.PlayMode = _Player.PlayMode switch
            {
                PlayMode.RepeatList => PlayMode.RepeatOne,
                PlayMode.RepeatOne => PlayMode.Shuffle,
                PlayMode.Shuffle => PlayMode.RepeatList,
                _ => throw new InvalidEnumArgumentException()
            };
            StorageService.SetSetting("PlayMode", (int)_Player.PlayMode);
        }

        private void VolumeSlider_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            StorageService.SetSetting("Volume", _Player.Volume);
        }
    }
}