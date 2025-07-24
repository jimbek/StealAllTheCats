namespace StealAllTheCats.API.Models.Data
{
    public interface ITagRepository
    {
        Task<bool> ExistsAsync(CancellationToken token, string name);
        Task AddAsync(CancellationToken token, TagEntity tag);
    }
}
