using System.Collections.Generic;

namespace FluentCloudMusic.Classes
{
    public class Artists
    {
        private List<(string ID, string Name)> ArtistsList;

        public string MainArtistName;

        public string MainArtistID;

        public Artists()
        {
            ArtistsList = new List<(string ID, string Name)> { };
        }

        public void AddArtist(string id, string name)
        {
            ArtistsList.Add((id, name));
            MainArtistName = ArtistsList[0].Name;
            MainArtistID = ArtistsList[0].ID;
        }
    }
}
