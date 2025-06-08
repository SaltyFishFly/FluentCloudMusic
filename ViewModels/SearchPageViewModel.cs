using FluentCloudMusic.DataModels;
using System.ComponentModel;

namespace FluentCloudMusic.ViewModels
{
    public class SearchPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public SearchRequest Source
        {
            set
            {
                _Source = value;
                Notify(nameof(PageString));
                Notify(nameof(HasPrev));
                Notify(nameof(HasNext));
            }
        }
        public int MaxPage
        {
            set
            {
                _MaxPage = value;
                Notify(nameof(PageString));
                Notify(nameof(HasNext));
            }
        }
        public string PageString => _Source != null ? $"{_Source.Page} / {_MaxPage}" : string.Empty;
        public bool HasPrev => _Source != null && _Source.Page > 1;
        public bool HasNext => _Source != null && _Source.Page < _MaxPage;

        private SearchRequest _Source;
        private int _MaxPage;

        private void Notify(string caller = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}
