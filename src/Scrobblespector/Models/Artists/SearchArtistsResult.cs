using System.Collections.Generic;

namespace Scrobblespector.Models.Artists
{
    public class SearchArtistsResult
    {
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalCount { get; set; }
        public List<ArtistInfo> FoundArtists { get; set; }
    }
}
