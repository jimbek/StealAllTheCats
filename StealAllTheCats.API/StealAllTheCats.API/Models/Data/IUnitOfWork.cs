namespace StealAllTheCats.API.Models.Data
{
    public interface IUnitOfWork
    {
        ICatRepository CatRepository { get; }
        ITagRepository TagRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
