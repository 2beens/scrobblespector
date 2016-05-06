using Scrobblespector.Models.Artists;
using System.Collections.Generic;

namespace Scrobblespector.Services
{
    public interface ILastFmArtistsService
    {
        SearchArtistsResult SearchArtists(string queryString, int page, int resultsLimit);
        Artist GetArtist(string mbid);
        SearchArtistsResult GetSimilarArtists(string mbid);
        List<ArtistTag> GetArtistTopTags(string mbid);
    }
}
