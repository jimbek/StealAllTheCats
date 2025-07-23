namespace StealAllTheCats.API.Models.DTOs
{
    public class Image
    {
        private static readonly string _prefix = "https://cdn2.thecatapi.com/images/";
        private static readonly string _suffix = ".jpg";

        public string Id { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public string Url { get; set; } = string.Empty;

        public virtual IList<Breed> Breeds { get; set; } = Array.Empty<Breed>();

        public string GetImageName()
        {
            if (string.IsNullOrWhiteSpace(Url))
            {
                return string.Empty;
            }

            string lastUrlPart = Url.Substring(_prefix.Length);

            int imageNameLength = lastUrlPart.Length - _suffix.Length;

            return lastUrlPart.Substring(imageNameLength);
        }
    }
}
