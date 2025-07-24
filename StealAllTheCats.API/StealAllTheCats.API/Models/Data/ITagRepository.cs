namespace StealAllTheCats.API.Models.Data
{
    public interface ITagRepository
    {
        Task<bool> ExistsAsync(string name);
        Task AddAsync(TagEntity tag);
    }
}
