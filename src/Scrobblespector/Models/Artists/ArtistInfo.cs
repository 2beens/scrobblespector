using System.Collections.Generic;

namespace Scrobblespector.Models.Artists
{
    public class ArtistInfo
    {
        public string Name { get; set; }
        public string URL { get; set; }
        public List<LastFmImage> Images { get; set; }
    }
}
