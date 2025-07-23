namespace StealAllTheCats.API.Models.DTOs
{
    public class Image
    {
        private static readonly string _prefix = "https://cdn2.thecatapi.com/images/";
        private static readonly string _suffix = ".jpg";

        public string id { get; set; } = string.Empty;
        public int width { get; set; }
        public int height { get; set; }
        public string url { get; set; } = string.Empty;

        public virtual IList<Breed> breeds { get; set; } = Array.Empty<Breed>();

        public string GetImageName()
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }

            string lastUrlPart = url.Substring(_prefix.Length);

            int imageNameLength = lastUrlPart.Length - _suffix.Length;

            return lastUrlPart.Substring(imageNameLength);
        }
    }
}
