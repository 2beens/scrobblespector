using System.Collections.Generic;

namespace Scrobblespector.Models.Artists
{
    public class ArtistTrack
    {
        public string Name { get; set; }
        public string PlayCount { get; set; }
        public string ListenersCount { get; set; }
        public List<LastFmImage> Images { get; set; }
        public string Rank { get; set; }
    }
}
