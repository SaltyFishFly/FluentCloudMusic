using FluentCloudMusic.Classes;
using System;
using System.ComponentModel;

namespace FluentCloudMusic.Controls
{
    public class MusicPlayerControlPositionWrapper : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MusicPlayer Source
        {
            set
            {
                _Source = value;
                _Source.PropertyChanged += (sender, args) =>
                {
                    if (IsDragging) return;
                    if (args.PropertyName != nameof(MusicPlayer.Position)) return;
                    Notify(nameof(Position));
                };
                Notify(nameof(Position));
            }
        }

        public bool IsDragging { get; set; }
        public TimeSpan Position => _Source.Position;

        private MusicPlayer _Source;

        private void Notify(string caller)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}
