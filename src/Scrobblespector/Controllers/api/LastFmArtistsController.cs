using Microsoft.AspNet.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Scrobblespector.Controllers.api
{
    [Route("api/lastfm/artists")]
    public class LastFmArtistsController : Controller
    {
        [HttpGet("{artistName}", Name = "SearchArtist")]
        public IActionResult SearchArtist(string artistName)
        {
            if (string.IsNullOrEmpty(artistName))
                return this.HttpBadRequest("Artist name cannot be null/empty");

            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri(SharedConfigs.SCROBBLER_BASE_ADDR);
                HttpResponseMessage response = client.GetAsync(GetSearchArtistRequestPath(artistName)).Result;
                response.EnsureSuccessStatusCode();

                if (response.StatusCode != HttpStatusCode.OK)
                    return Json(new { errorMessage = "Wrong data received from LastFM server." });

                string artistsJsonString = response.Content.ReadAsStringAsync().Result;

                dynamic artistsJson = JObject.Parse(artistsJsonString);

                return Json(artistsJson.results);
            }
        }

        private string GetSearchArtistRequestPath(string artistName)
        {
            if (string.IsNullOrEmpty(artistName))
                throw new ArgumentNullException("artistName");

            return string.Format(SharedConfigs.SCROBBLER_SEARCH_ARTIST_PATH, artistName);
        }
    }
}
