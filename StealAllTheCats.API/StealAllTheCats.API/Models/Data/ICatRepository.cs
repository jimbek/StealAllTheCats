namespace StealAllTheCats.API.Models.Data
{
    public interface ICatRepository
    {
        Task<bool> ExistsAsync(string id);
        Task<int> AddAsync(CatEntity cat);
        Task<CatEntity?> GetCatEntityAsync(int id);
    }
}
