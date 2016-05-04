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
        public const string SCROBBLER_GET_USER_PATH = "?method=user.getinfo&user={0}&api_key=" + LAST_FM_API_KEY + "&format=json";
        public const string SCROBBLER_SEARCH_ARTIST_PATH = "?method=artist.search&artist={0}&api_key=" + LAST_FM_API_KEY + "&format=json";
    }
}
