using Newtonsoft.Json.Linq;

namespace FluentNetease.Classes
{
    public class Song
    {
        public Music Music { get; set; }
        public Album Album { get; set; }
        public Artists Artists { get; set; }

        public Song(JToken jsonSongObject)
        {
            Music = new Music()
            {
                ID = jsonSongObject["id"].ToString(),
                Name = jsonSongObject["name"].ToString(),
            };
            if (jsonSongObject["tns"] != null && jsonSongObject["tns"].HasValues)
            {
                Music.Translation = "(" + jsonSongObject["tns"].First.ToString() + ")";
            }
            if (jsonSongObject["alia"] != null && jsonSongObject["alia"].HasValues)
            {
                Music.Alias = "(" + jsonSongObject["alia"].First.ToString() + ")";
            }

            Album = new Album()
            {
                ID = jsonSongObject["al"]["id"].ToString(),
                Name = jsonSongObject["al"]["name"].ToString()
            };

            Artists = new Artists();
            foreach (var Item in jsonSongObject["ar"])
            {
                Artists.AddArtist(Item["id"].ToString(), Item["name"].ToString());
            }
        }
    }
}
