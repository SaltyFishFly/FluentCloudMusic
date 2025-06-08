using FluentCloudMusic.Classes;
using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.Services;
using System.Collections.ObjectModel;

namespace FluentCloudMusic.ViewModels
{
    public class RecommendSongsPageViewModel
    {
        public readonly ObservableCollection<Song> Songs = new ObservableCollection<Song>();

        public async void LoadContents()
        {
            try
            {
                var songs = await SongService.GetDailyRecommendSongsAsync();
                songs?.ForEach(song => Songs.Add(song));
            }
            catch (ResponseCodeErrorException) { }
        }
    }
}
