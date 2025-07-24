namespace StealAllTheCats.API.Models.Data
{
    public interface ITagRepository : IRepository
    {
        Task<bool> ExistsAsync(string name);
        Task AddAsync(TagEntity tag);
    }
}
