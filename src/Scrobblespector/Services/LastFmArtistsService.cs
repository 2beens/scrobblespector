using Newtonsoft.Json.Linq;
using Scrobblespector.Models;
using Scrobblespector.Models.Artists;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace Scrobblespector.Services
{
    public class LastFmArtistsService : ILastFmArtistsService
    {
        public SearchArtistsResult SearchArtists(string queryString, int page, int resultsLimit)
        {
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri(SharedConfigs.SCROBBLER_BASE_ADDR);
                HttpResponseMessage response = client.GetAsync(SharedConfigs.GetSearchArtistRequestPath(queryString, page, resultsLimit)).Result;
                response.EnsureSuccessStatusCode();

                //if (response.StatusCode != HttpStatusCode.OK)
                //    return Json(new { errorMessage = "Wrong data received from LastFM server." });

                // TODO: throw adequate exception
                if (response.StatusCode != HttpStatusCode.OK)
                    return new SearchArtistsResult { TotalCount = 0, FoundArtists = new List<ArtistInfo>() };

                string artistsJsonString = response.Content.ReadAsStringAsync().Result;
                dynamic artistsJson = JObject.Parse(artistsJsonString);

                List<ArtistInfo> foundArtists = new List<ArtistInfo>();
                foreach(dynamic artist in artistsJson.results.artistmatches.artist)
                {
                    string listenersCountStr = artist.listeners;
                    int listenersCount;
                    if (!Int32.TryParse(listenersCountStr, out listenersCount))
                        listenersCount = -1;

                    foundArtists.Add(new ArtistInfo
                    {
                        MBID = artist.mbid,
                        Name = artist.name,
                        ListenersCount = listenersCount,
                        Images = new List<LastFmImage>()
                    });
                }

                JObject artistsJsonResults = artistsJson.results as JObject;
                string totalArtistsCountStr = artistsJsonResults.GetValue("opensearch:totalResults").Value<string>();
                string currentPageStr = artistsJsonResults.GetValue("opensearch:startIndex").Value<string>();
                string itemsPerPageStr = artistsJsonResults.GetValue("opensearch:itemsPerPage").Value<string>();

                return new SearchArtistsResult
                {
                    FoundArtists = foundArtists,
                    TotalCount = Int32.Parse(totalArtistsCountStr),
                    ItemsPerPage = Int32.Parse(itemsPerPageStr),
                    Page = Int32.Parse(currentPageStr)
                };
            }
        }

        public Artist GetArtist(string mbid)
        {
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri(SharedConfigs.SCROBBLER_BASE_ADDR);
                HttpResponseMessage response = client.GetAsync(SharedConfigs.GetArtistRequestPath(mbid)).Result;
                response.EnsureSuccessStatusCode();

                //if (response.StatusCode != HttpStatusCode.OK)
                //    return Json(new { errorMessage = "Wrong data received from LastFM server." });

                // TODO: throw adequate exception
                if (response.StatusCode != HttpStatusCode.OK)
                    return null;

                string artistJsonString = response.Content.ReadAsStringAsync().Result;
                dynamic artistJson = JObject.Parse(artistJsonString);

                try
                {
                    return GetArtistByDynamicJson(artistJson.artist);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.GetType());
                }

                return null;
            }
        }

        public SearchArtistsResult GetSimilarArtists(string mbid)
        {
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri(SharedConfigs.SCROBBLER_BASE_ADDR);
                HttpResponseMessage response = client.GetAsync(SharedConfigs.GetSimilarArtistsRequestPath(mbid)).Result;
                response.EnsureSuccessStatusCode();

                //if (response.StatusCode != HttpStatusCode.OK)
                //    return Json(new { errorMessage = "Wrong data received from LastFM server." });

                // TODO: throw adequate exception
                if (response.StatusCode != HttpStatusCode.OK)
                    return new SearchArtistsResult { TotalCount = 0, FoundArtists = new List<ArtistInfo>() };

                string artistsJsonString = response.Content.ReadAsStringAsync().Result;
                dynamic artistsJson = JObject.Parse(artistsJsonString);

                List<ArtistInfo> foundArtists = new List<ArtistInfo>();
                foreach (dynamic artist in artistsJson.similarartists.artist)
                {
                    foundArtists.Add(new ArtistInfo
                    {
                        MBID = artist.mbid,
                        Name = artist.name,
                        Match = artist.match,
                        URL = artist.url,
                        Images = new List<LastFmImage>()
                    });
                }

                return new SearchArtistsResult { FoundArtists = foundArtists, TotalCount = foundArtists.Count };
            }
        }

        public List<ArtistTag> GetArtistTopTags(string mbid)
        {
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri(SharedConfigs.SCROBBLER_BASE_ADDR);
                HttpResponseMessage response = client.GetAsync(SharedConfigs.GetArtistTopTagsRequestPath(mbid)).Result;
                response.EnsureSuccessStatusCode();

                //if (response.StatusCode != HttpStatusCode.OK)
                //    return Json(new { errorMessage = "Wrong data received from LastFM server." });

                // TODO: throw adequate exception
                if (response.StatusCode != HttpStatusCode.OK)
                    return new List<ArtistTag>();

                string topTagsJsonStr = response.Content.ReadAsStringAsync().Result;
                dynamic topTagsJson = JObject.Parse(topTagsJsonStr);

                List<ArtistTag> foundTags = new List<ArtistTag>();
                foreach (dynamic tag in topTagsJson.toptags.tag)
                {
                    foundTags.Add(new ArtistTag
                    {
                        Count = tag.count,
                        Name = tag.name,
                        Url = tag.url
                    });
                }

                return foundTags;
            }
        }

        public List<ArtistTrack> GetArtistTopTracks(string mbid, int page, int resultsLimit)
        {
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri(SharedConfigs.SCROBBLER_BASE_ADDR);
                HttpResponseMessage response = client.GetAsync(SharedConfigs.GetArtistTopTracksRequestPath(mbid, page, resultsLimit)).Result;
                response.EnsureSuccessStatusCode();

                //if (response.StatusCode != HttpStatusCode.OK)
                //    return Json(new { errorMessage = "Wrong data received from LastFM server." });

                // TODO: throw adequate exception
                if (response.StatusCode != HttpStatusCode.OK)
                    return new List<ArtistTrack>();

                string topTracksJsonStr = response.Content.ReadAsStringAsync().Result;
                dynamic topTracksJson = JObject.Parse(topTracksJsonStr);

                List<ArtistTrack> foundTrakcs = new List<ArtistTrack>();
                foreach (dynamic track in topTracksJson.toptracks.track)
                {
                    List<LastFmImage> images = new List<LastFmImage>();
                    foreach (JObject image in track.image)
                    {
                        string imageSize = image.GetValue("size").Value<string>();
                        string imageUrl = image.GetValue("#text").Value<string>();
                        images.Add(new LastFmImage(imageSize, imageUrl));
                    }

                    foundTrakcs.Add(new ArtistTrack
                    {
                        Name = track.name,
                        PlayCount = track.playcount,
                        ListenersCount = track.listeners,
                        Images = images,
                        Rank = ((track as JObject).GetValue("@attr").First as JProperty).Value.ToString()
                    });
                }

                return foundTrakcs;
            }
        }

        private Artist GetArtistByDynamicJson(dynamic artistJson)
        {
            List<LastFmImage> images = new List<LastFmImage>();
            foreach (JObject image in artistJson.image)
            {
                string imageSize = image.GetValue("size").Value<string>();
                string imageUrl = image.GetValue("#text").Value<string>();
                images.Add(new LastFmImage(imageSize, imageUrl));
            }

            List<ArtistInfo> similarArtists = new List<ArtistInfo>();
            foreach (dynamic similarArtist in artistJson.similar.artist)
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
            foreach (dynamic tag in artistJson.tags.tag)
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

    }
}
