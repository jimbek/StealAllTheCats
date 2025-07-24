namespace StealAllTheCats.API.Jobs.Payloads
{
    public class CancellationTokenPayload
    {
        public string ApiKey { get; set; } = string.Empty;

        public CancellationTokenPayload(string key)
        {
            ApiKey = key;
        }
    }
}
