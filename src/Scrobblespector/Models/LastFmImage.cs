using System;

namespace Scrobblespector.Models
{
    public enum LastFmImageSize
    {
        SMALL = 1,
        MEDIUM = 2,
        LARGE = 3,
        EXTRALARGE = 4,
        MEGA = 5,
        DEFAULT = 6
    }

    public class LastFmImage
    {
        public string Url { get; set; }
        public LastFmImageSize ImageSize { get; set; }

        public LastFmImage(LastFmImageSize imageSize, string url)
        {
            this.ImageSize = imageSize;
            this.Url = url;
        }

        public LastFmImage(string imageSizeString, string url)
        {
            LastFmImageSize imageSize;
            if (imageSizeString.Length == 0 || !Enum.TryParse(imageSizeString.ToUpper(), out imageSize))
                imageSize = LastFmImageSize.DEFAULT;

            this.ImageSize = imageSize;
            this.Url = url;
        }
    }
}
