namespace StealAllTheCats.API.Models.Data
{
    public interface IRepository
    {
        Task<int> SaveChangesAsync();
    }
}
