namespace StealAllTheCats.API.Models.Data
{
    public interface ICatRepository : IRepository
    {
        Task<bool> ExistsAsync(string id);
        Task AddAsync(CatEntity cat);
        Task<CatEntity?> GetCatEntityAsync(string id);
    }
}
