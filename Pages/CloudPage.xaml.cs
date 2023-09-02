using FluentCloudMusic.DataModels;
using FluentCloudMusic.DataModels.JSONModels;
using FluentCloudMusic.DataModels.JSONModels.Responses;
using FluentCloudMusic.DataModels.ViewModels;
using FluentCloudMusic.Services;
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
            CurrentSection = section;

            var (isSuccess, pageCount, songs) = await NetworkService.GetUserCloudAsync(section);
            if (!isSuccess) return;

            CurrentSectionViewModel.MaxPage = pageCount;
            Songs.Clear();

            foreach (var item in songs) Songs.Add(item);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GetUserCloud(new Section());
        }

        private void PlayAllButton_Click(object sender, RoutedEventArgs e)
        {
            var playlist = new List<ISong>(Songs);
            _ = MainPage.Player.PlayAsync(playlist);
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
