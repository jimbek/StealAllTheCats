namespace StealAllTheCats.API.Jobs.Payloads
{
    public class CancellationTokenPayload
    {
        public Guid Id { get; set; }
        public string ApiKey { get; set; } = string.Empty;

        public CancellationTokenPayload(Guid id, string key)
        {
            Id = id;
            ApiKey = key;
        }
    }
}
