using System.Collections.Generic;

namespace Scrobblespector.Models.Artists
{
    public class ArtistInfo
    {
        public string MBID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public int ListenersCount { get; set; }

        // used whith getting similar artists (represents a value [0 -> 1] of how similar the artists are)
        public string Match { get; set; }

        public List<LastFmImage> Images { get; set; }
    }
}
