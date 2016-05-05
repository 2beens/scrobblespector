using Microsoft.AspNet.Mvc;
using Newtonsoft.Json.Linq;
using Scrobblespector.Models;
using Scrobblespector.Models.Artists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Reflection;

namespace Scrobblespector.Controllers.api
{
    [Route("api/lastfm/artists")]
    public class LastFmArtistsController : Controller
    {
        [HttpGet("search/{artistName}", Name = "SearchArtist")]
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

                return this.Ok(Json(artistsJson.results));
            }
        }

        [HttpGet("{mbid}", Name = "GetArtist")]
        public IActionResult GetArtist(string mbid)
        {
            if (string.IsNullOrEmpty(mbid))
                return this.HttpBadRequest("Artist MBID cannot be null/empty");

            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri(SharedConfigs.SCROBBLER_BASE_ADDR);
                HttpResponseMessage response = client.GetAsync(GetArtistRequestPath(mbid)).Result;
                response.EnsureSuccessStatusCode();

                if (response.StatusCode != HttpStatusCode.OK)
                    return Json(new { errorMessage = "Wrong data received from LastFM server." });

                string artistJsonString = response.Content.ReadAsStringAsync().Result;

                dynamic artistJson = JObject.Parse(artistJsonString);

                try
                {
                    Artist artist = GetArtistByDynamicJson(artistJson.artist);
                    return this.Ok(Json(artist));
                }
                catch(Exception ex)
                {
                    Console.Write(ex.GetType());
                }

                return this.HttpNotFound();
            }
        }

        [NonAction]
        private Artist GetArtistByDynamicJson(dynamic artistJson)
        {
            List<LastFmImage> images = new List<LastFmImage>();
            foreach(JObject image in artistJson.image)
            {
                string imageSize = image.GetValue("size").Value<string>();
                string imageUrl = image.GetValue("#text").Value<string>();
                images.Add(new LastFmImage(imageSize, imageUrl));
            }

            List<ArtistInfo> similarArtists = new List<ArtistInfo>();
            foreach(dynamic similarArtist in artistJson.similar.artist)
            {
                string saName = similarArtist.name;
                string saUrl = similarArtist.url;
                List<LastFmImage> saImages = new List<LastFmImage>();
                foreach (JObject image in similarArtist.image)
                {
                    string imageSize = image.GetValue("size").Value<string>();
                    string imageUrl = image.GetValue("#text").Value<string>();
                    saImages.Add(new LastFmImage(imageSize, imageUrl));
                }

                similarArtists.Add(new ArtistInfo
                {
                    Name = saName,
                    URL = saUrl,
                    Images = saImages
                });
            }

            string linkHref = artistJson.bio.links.link.href;
            List<string> links = new List<string>() { linkHref };

            List<ArtistTag> tags = new List<ArtistTag>();
            foreach(dynamic tag in artistJson.tags.tag)
            {
                tags.Add(new ArtistTag
                {
                    Name = tag.name,
                    Url = tag.url
                });
            }

            DateTime? published;
            DateTime publishedDate;
            string datePublishedStr = artistJson.bio.published;
            if (!DateTime.TryParse(datePublishedStr, out publishedDate))
                published = null;
            else
                published = publishedDate;

            string content = artistJson.bio.content;
            bool isStreamable = artistJson.streamable == "1";

            string listenersCountStr = artistJson.stats.listeners;
            int listenersCount;
            if (!Int32.TryParse(listenersCountStr, out listenersCount))
                listenersCount = 0;

            string playsCountStr = artistJson.stats.playcount;
            int playsCount;
            if (!Int32.TryParse(playsCountStr, out playsCount))
                playsCount = 0;

            return new Artist
            {
                Content = content,
                Images = images,
                IsStreamable = isStreamable,
                Tags = tags,
                Links = links,
                Name = artistJson.name,
                MBID = artistJson.mbid,
                ListenersCount = listenersCount,
                PlaysCount = playsCount,
                Published = published,
                Summary = artistJson.bio.summary,
                URL = artistJson.url,
                SimilarArtists = similarArtists
            };
        }
        
        [NonAction]
        private string GetSearchArtistRequestPath(string artistName)
        {
            if (string.IsNullOrEmpty(artistName))
                throw new ArgumentNullException("artistName");

            return string.Format(SharedConfigs.SCROBBLER_SEARCH_ARTIST_PATH, artistName);
        }

        [NonAction]
        private string GetArtistRequestPath(string mbid)
        {
            if (string.IsNullOrEmpty(mbid))
                throw new ArgumentNullException("mbid");

            return string.Format(SharedConfigs.SCROBBLER_GET_ARTIST_PATH, mbid);
        }
    }
}
