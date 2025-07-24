namespace StealAllTheCats.API.Models.DTOs
{
    public class Image
    {
        public static readonly string Prefix = "https://cdn2.thecatapi.com/images/";
        public static readonly string Suffix = ".jpg";

        private static int _prefixLength = Prefix.Length;
        private static int _suffixLength = Suffix.Length;

        public string id { get; set; } = string.Empty;
        public int width { get; set; }
        public int height { get; set; }
        public string url { get; set; } = string.Empty;

        public virtual IList<Breed> breeds { get; set; } = Array.Empty<Breed>();

        public string GetImageName()
        {
            if (string.IsNullOrWhiteSpace(url) || url.Length <= _prefixLength + _suffixLength)
            {
                return string.Empty;
            }

            string lastUrlPart = url.Substring(_prefixLength);

            int imageNameLength = lastUrlPart.Length - _suffixLength;

            return lastUrlPart[..imageNameLength];
        }
    }
}
