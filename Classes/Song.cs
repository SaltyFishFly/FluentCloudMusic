using Newtonsoft.Json.Linq;

namespace FluentNetease.Classes
{
    public class Song
    {
        public Music Music { get; set; }
        public Album Album { get; set; }
        public Artists Artists { get; set; }

        public Song(JToken jsonSongList)
        {
            Music = new Music()
            {
                ID = jsonSongList["id"].ToString(),
                Name = jsonSongList["name"].ToString()
            };
            Album = new Album()
            {
                ID = jsonSongList["al"]["id"].ToString(),
                Name = jsonSongList["al"]["name"].ToString()
            };
            Artists = new Artists();
            foreach (var Item in jsonSongList["ar"])
            {
                Artists.AddArtist(Item["id"].ToString(), Item["name"].ToString());
            }
        }
    }
}
