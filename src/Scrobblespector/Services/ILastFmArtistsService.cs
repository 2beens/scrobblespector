using Scrobblespector.Models.Artists;
using System.Collections.Generic;

namespace Scrobblespector.Services
{
    public interface ILastFmArtistsService
    {
        SearchArtistsResult SearchArtists(string queryString);
        Artist GetArtist(string mbid);
    }
}
