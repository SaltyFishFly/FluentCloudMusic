using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.Services;
using System.Collections.ObjectModel;

namespace FluentCloudMusic.DataModels.ViewModels
{
    public class RecommendSongsPageViewModel
    {
        public readonly ObservableCollection<Song> Songs = new ObservableCollection<Song>();

        public async void LoadContents()
        {
            var songs = await SongService.GetDailyRecommendSongsAsync();
            songs?.ForEach(song => Songs.Add(song));
        }
    }
}
