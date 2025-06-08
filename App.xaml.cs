using FluentCloudMusic.Classes;
using FluentCloudMusic.Pages;
using FluentCloudMusic.Services;
using FluentCloudMusic.Utils;
using NeteaseCloudMusicApi;
using Newtonsoft.Json;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FluentCloudMusic
{
    sealed partial class App : Application
    {
        public static CloudMusicApi API = new CloudMusicApi();
        public static MusicPlayer Player
        {
            get => _Player.Value;
        }

        private static readonly Lazy<MusicPlayer> _Player = new Lazy<MusicPlayer>();

        public App()
        {
            InitializeComponent();

            Suspending += OnSuspending;
        }

        public static void ReloadTheme()
        {
            if (!StorageService.HasSetting(SettingsPage.ThemeSetting)) return;

            var theme = StorageService.GetSetting<int>(SettingsPage.ThemeSetting);
            (Window.Current.Content as FrameworkElement).RequestedTheme = (ElementTheme)theme;
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            CoreApplication.EnablePrelaunch(false);

            SetDefaultJsonSerializer();
            TransparentTitleBar();
            SetWindowMinSize(500, 400);
            InitRootFrame();
            // BUG: 启用快捷键会导致应用内无法输入空格
            // RegisterGlobalHotkeys();
            ReloadTheme();

            Window.Current.Activate();

            await AccountService.LoginByCookieAsync();
        }

        private void SetDefaultJsonSerializer()
        {
            JsonConvert.DefaultSettings = 
                () => new JsonSerializerSettings()
                {
                    ContractResolver = new JsonMultiplePropertyContractResolver()
                };
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
            if (frame.Content == null)
            {
                frame.Navigate(typeof(MainPage), null);
            }
        }

        private void RegisterGlobalHotkeys()
        {
            var frame = Window.Current.Content as Frame;
            if (frame == null) return;
            frame.PreviewKeyDown += (sender, args) =>
            {
                if (args.Key == VirtualKey.Space) Player.SwitchPlayStatus();
            };
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动
            deferral.Complete();
        }
    }
}
