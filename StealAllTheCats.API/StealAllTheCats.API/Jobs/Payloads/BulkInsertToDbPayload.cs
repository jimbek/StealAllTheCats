namespace StealAllTheCats.API.Jobs.Payloads
{
    public class BulkInsertToDbPayload
    {
        public Guid Id { get; set; }
        public string ApiKey { get; set; } = string.Empty;

        public BulkInsertToDbPayload(Guid id, string key)
        {
            Id = id;
            ApiKey = key;
        }
    }
}
