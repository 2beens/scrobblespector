using System;
using System.Collections.Generic;

namespace Scrobblespector.Models.Artists
{
    public class Artist
    {
        public string MBID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public bool IsStreamable { get; set; }
        public int ListenersCount { get; set; }
        public int PlaysCount { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public DateTime? Published { get; set; }
        public List<string> Links { get; set; }
        public List<LastFmImage> Images { get; set; }
        public List<ArtistInfo> SimilarArtists { get; set; }
        public List<ArtistTag> Tags { get; set; }
    }
}
