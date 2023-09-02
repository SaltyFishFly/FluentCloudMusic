using FluentCloudMusic.Controls;
using FluentCloudMusic.Pages;
using FluentCloudMusic.Services;
using NeteaseCloudMusicApi;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace FluentCloudMusic
{
    sealed partial class App : Application
    {
        public static CloudMusicApi API = new CloudMusicApi();

        public App()
        {
            InitializeComponent();

            Suspending += OnSuspending;
        }

        public static void UpdateTheme()
        {
            if (!StorageService.HasSetting(SettingsPage.ThemeSetting)) return;

            var theme = StorageService.GetSetting<ElementTheme>(SettingsPage.ThemeSetting);
            (Window.Current.Content as FrameworkElement).RequestedTheme = theme;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            CoreApplication.EnablePrelaunch(false);

            TransparentTitleBar();
            SetWindowMinSize(500, 400);
            InitRootFrame();
            UpdateTheme();

            Window.Current.Activate();
        }

        private void TransparentTitleBar()
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }

        private void SetWindowMinSize(double width, double height)
        {
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(width, height));
        }

        private void InitRootFrame()
        {
            var frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                frame = new Frame();
                Window.Current.Content = frame;
            }
            frame.PreviewKeyDown += GlobalHotkey_Pressed;

            if (frame.Content == null)
            {
                frame.Navigate(typeof(MainPage), null);
            }
        }

        private void GlobalHotkey_Pressed(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Space)
            {
                MusicPlayerControl.Instance.SwitchPlayStatus();
                e.Handled = true;
            }
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动
            deferral.Complete();
        }
    }
}
