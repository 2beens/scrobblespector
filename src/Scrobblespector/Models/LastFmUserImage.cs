namespace Scrobblespector.Models
{
    public class LastFmUserImage
    {
        public const int SMALL_IMAGE = 1;
        public const int MEDIUM_IMAGE = 2;
        public const int LARGE_IMAGE = 3;
        public const int HUGE_IMAGE = 4;

        public string Url { get; set; }
        public int ImageSize { get; set; }

        public LastFmUserImage(int imageSize, string url)
        {
            this.ImageSize = imageSize;
            this.Url = url;
        }
    }
}
