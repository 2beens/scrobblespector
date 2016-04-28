using System;

namespace Scrobblespector.Models
{
    public class LastFmUser
    {
        public const char USER_MALE = 'm';
        public const char USER_FEMALE = 'f';

        public string Name { get; set; }
        public string RealName { get; set; }
        public LastFmUserImage Image { get; set; }
        public string Url { get; set; }
        public string Country { get; set; }
        public int Age { get; set; }
        public char Gender { get; set; }
        public bool IsSubscriber { get; set; }
        public int PlayCount { get; set; }
        public int Playlists { get; set; }
        public int Bootstrap { get; set; }
        public DateTime RegisteredAt { get; set; }
        public string Type { get; set; }
    }
}
