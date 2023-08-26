using FluentCloudMusic.Controls;
using Windows.Media.Playback;

using SMTC = Windows.Media.SystemMediaTransportControls;

namespace FluentCloudMusic.Classes
{
    public class SMTCController
    {
        private readonly MusicPlayerControl MusicPlayerControl;
        private MediaPlayer Player => MusicPlayerControl.Player;
        private MediaPlaybackCommandManager CommandManager => Player.CommandManager;
        private SMTC SMTC => Player.SystemMediaTransportControls;

        public SMTCController(MusicPlayerControl musicPlayer)
        {
            MusicPlayerControl = musicPlayer;

            Player.SourceChanged += (sender, args) =>
            {
                CommandManager.PreviousBehavior.EnablingRule =
                    MusicPlayerControl.HasPrevious ? MediaCommandEnablingRule.Always : MediaCommandEnablingRule.Never;
                CommandManager.NextBehavior.EnablingRule =
                    MusicPlayerControl.HasNext ? MediaCommandEnablingRule.Always : MediaCommandEnablingRule.Never;
            };

            CommandManager.PreviousReceived += async (sender, args) =>
            {
                var defer = args.GetDeferral();
                await MusicPlayerControl.Previous();
                args.Handled = true;
                defer.Complete();
            };

            CommandManager.NextReceived += async (sender, args) =>
            {
                var defer = args.GetDeferral();
                await MusicPlayerControl.Next();
                args.Handled = true;
                defer.Complete();
            };
        }
    }
}
