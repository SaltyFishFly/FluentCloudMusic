namespace FluentNetease.Classes
{
    public class Playlist
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string CoverPictureUrl { get; set; }

        public Playlist(string ID, string Name, string CoverPictureUrl)
        {
            this.ID = ID;
            this.Name = Name;
            this.CoverPictureUrl = CoverPictureUrl;
        }
    }
}
