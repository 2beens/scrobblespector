using System.Collections.Generic;

namespace Scrobblespector.Models.Artists
{
    public class SearchArtistsResult
    {
        public List<ArtistInfo> FoundArtists { get; set; }
        public int TotalCount { get; set; }
    }
}
