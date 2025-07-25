namespace StealAllTheCats.API.Models.Data
{
    public interface ICatRepository
    {
        Task<bool> ExistsAsync(string id);
        Task AddAsync(CatEntity cat);
        Task<CatEntity?> GetCatEntityAsync(CancellationToken token, string id);
        Task<IList<CatEntity>> GetCatEntitiesAsync(CancellationToken token, int page, int pageSize);
    }
}
