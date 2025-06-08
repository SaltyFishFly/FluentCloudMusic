using FluentCloudMusic.Classes;
using FluentCloudMusic.DataModels;
using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.DataModels.JSONModels.Responses;
using FluentCloudMusic.Services;
using FluentCloudMusic.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace FluentCloudMusic.Pages
{
    public sealed partial class CloudPage : Page
    {
        private Section CurrentSection
        {
            get => _CurrentSection;
            set
            {
                _CurrentSection = value;
                CurrentSectionViewModel.Source = value;
            }
        }

        private readonly ObservableCollection<UserCloudSong> Songs;
        private readonly SectionViewModel CurrentSectionViewModel;
        private Section _CurrentSection;

        public CloudPage()
        {
            Songs = new ObservableCollection<UserCloudSong>();
            CurrentSectionViewModel = new SectionViewModel();

            InitializeComponent();
        }

        private async void GetUserCloud(Section section)
        {
            try
            {
                CurrentSection = section;

                var (pageCount, songs) = await NetworkService.GetUserCloudAsync(section);
                CurrentSectionViewModel.MaxPage = pageCount;

                Songs.Clear();
                foreach (var item in songs) Songs.Add(item);
            }
            catch (ResponseCodeErrorException) { }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GetUserCloud(new Section());
        }

        private void PlayAllButton_Click(object sender, RoutedEventArgs e)
        {
            var playlist = new List<ISong>(Songs);
            _ = App.Player.PlayAsync(playlist);
        }

        private void FilterInputBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            SongList.ApplyFilter(sender.Text);
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            GetUserCloud(CurrentSection.Prev());
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            GetUserCloud(CurrentSection.Next());
        }
    }
}
