namespace StealAllTheCats.API.Models.Data
{
    public interface ITagRepository
    {
        Task<bool> ExistsAsync(string name);
        Task<int> AddAsync(TagEntity tag);
    }
}
