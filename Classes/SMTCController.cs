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
            Player.SourceChanged += Player_SourceChanged;
            CommandManager.PreviousReceived += (sender, args) =>
            {
                MusicPlayerControl.Previous();
                args.Handled = true;
            };
            CommandManager.NextReceived += (sender, args) =>
            {
                MusicPlayerControl.Next();
                args.Handled = true;
            };
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
