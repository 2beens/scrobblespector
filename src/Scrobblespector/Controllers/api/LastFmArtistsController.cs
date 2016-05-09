using Microsoft.AspNet.Mvc;
using Scrobblespector.Models.Artists;
using Scrobblespector.Services;

namespace Scrobblespector.Controllers.api
{
    [Produces("application/json")]
    [Route("api/lastfm/artists")]
    public class LastFmArtistsController : Controller
    {
        private readonly ILastFmArtistsService _lastFmArtistsService;

        public LastFmArtistsController(ILastFmArtistsService lastFmArtistsService)
        {
            this._lastFmArtistsService = lastFmArtistsService;
        }

        [HttpGet("search/{artistName}", Name = "SearchArtist")]
        public IActionResult SearchArtist(string artistName)
        {
            if (string.IsNullOrEmpty(artistName))
                return this.HttpBadRequest("Artist name cannot be null/empty");

            int page = 1;
            int resultsLimit = 10;
            var foundArtists = _lastFmArtistsService.SearchArtists(artistName, page, resultsLimit);

            return this.Ok(Json(foundArtists));
        }

        [HttpGet("similar/{mbid}", Name = "GetSimilarArtists")]
        public IActionResult GetSimilarArtists(string mbid)
        {
            if (string.IsNullOrEmpty(mbid))
                return this.HttpBadRequest("Artist MBID cannot be null/empty");

            var similarArtists = _lastFmArtistsService.GetSimilarArtists(mbid);

            return this.Ok(Json(similarArtists));
        }

        [HttpGet("tags/top/{mbid}", Name = "GetTopTags")]
        public IActionResult GetTopTags(string mbid)
        {
            if (string.IsNullOrEmpty(mbid))
                return this.HttpBadRequest("Artist MBID cannot be null/empty");

            var topTags = _lastFmArtistsService.GetArtistTopTags(mbid);

            return this.Ok(Json(topTags));
        }

        [HttpGet("tracks/top/{mbid}", Name = "GetTopTracks")]
        public IActionResult GetTopTracks(string mbid)
        {
            if (string.IsNullOrEmpty(mbid))
                return this.HttpBadRequest("Artist MBID cannot be null/empty");

            int page = 1;
            int resultsLimit = 15;
            var topTracks = _lastFmArtistsService.GetArtistTopTracks(mbid, page, resultsLimit);

            return this.Ok(Json(topTracks));
        }

        [HttpGet("{mbid}", Name = "GetArtist")]
        public IActionResult GetArtist(string mbid)
        {
            if (string.IsNullOrEmpty(mbid))
                return this.HttpBadRequest("Artist MBID cannot be null/empty");
            
            Artist artist = _lastFmArtistsService.GetArtist(mbid);
            if (artist == null)
                return this.HttpNotFound();

            return this.Ok(Json(artist));
        }
    }
}
