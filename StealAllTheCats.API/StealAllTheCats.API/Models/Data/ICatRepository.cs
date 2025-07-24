namespace StealAllTheCats.API.Models.Data
{
    public interface ICatRepository
    {
        Task<bool> ExistsAsync(CancellationToken token, string id);
        Task AddAsync(CancellationToken token, CatEntity cat);
        Task<CatEntity?> GetCatEntityAsync(CancellationToken token, string id);
    }
}
