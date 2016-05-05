using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrobblespector
{
    public class SharedConfigs
    {
        private const string LAST_FM_API_KEY = "b46bf61085abeaaad64c2e795c90e976";

        public const string SCROBBLER_BASE_ADDR = "http://ws.audioscrobbler.com/2.0/";
        private const string SCROBBLER_GET_USER_PATH = "?method=user.getinfo&user={0}&api_key=" + LAST_FM_API_KEY + "&format=json";
        private const string SCROBBLER_SEARCH_ARTIST_PATH = "?method=artist.search&artist={0}&api_key=" + LAST_FM_API_KEY + "&format=json";
        private const string SCROBBLER_GET_ARTIST_PATH = "?method=artist.getinfo&mbid={0}&api_key=" + LAST_FM_API_KEY + "&format=json";

        private const string SCROBBLER_GET_SIMILAR_ARTISTS_RESULTS_LIMIT = "15";
        private const string SCROBBLER_GET_SIMILAR_ARTISTS_PATH = "?method=artist.getsimilar&mbid={0}&api_key=" + LAST_FM_API_KEY + "&limit=" + 
            SCROBBLER_GET_SIMILAR_ARTISTS_RESULTS_LIMIT + "&format=json";

        public static string GetSearchArtistRequestPath(string artistName)
        {
            if (string.IsNullOrEmpty(artistName))
                throw new ArgumentNullException("artistName");

            return string.Format(SCROBBLER_SEARCH_ARTIST_PATH, artistName);
        }

        public static string GetSimilarArtistsRequestPath(string mbid)
        {
            if (string.IsNullOrEmpty(mbid))
                throw new ArgumentNullException("mbid");

            return string.Format(SCROBBLER_GET_SIMILAR_ARTISTS_PATH, mbid);
        }

        public static string GetArtistRequestPath(string mbid)
        {
            if (string.IsNullOrEmpty(mbid))
                throw new ArgumentNullException("mbid");

            return string.Format(SCROBBLER_GET_ARTIST_PATH, mbid);
        }

        public static string GetUserRequestPath(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException("userId");

            return string.Format(SCROBBLER_GET_USER_PATH, userId);
        }

    }
}
