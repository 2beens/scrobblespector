using System.Collections.Generic;

namespace Scrobblespector.Models.Artists
{
    public class ArtistInfo
    {
        public string MBID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public int ListenersCount { get; set; }
        public List<LastFmImage> Images { get; set; }
    }
}
